using UnityEngine;
using System.Collections.Generic;

public abstract class Tank : Entity {

    public static List<Tank> activeTanks = new List<Tank>();

    public bool controlledLocally;
    public Rigidbody2D body;
    public Color color;
    public float speed;
    public float fireCooldown;
    public bool canFire;
    public bool reversing;
    public bool spawning;
    public bool ignoreProjectiles;

    protected TankManager manager;
    protected Dictionary<Equipment, int> equipment;
    protected bool onConveyor;
    protected bool moving;
    protected Vector2 direction;
    protected Vector2 conveyorDirection;
    protected Vector2 movementThisFrame;
    protected GameObject currentLaser;
    protected GameObject laserTarget;

    private void Awake()
    {
        //Assign tank manager and tank parts
        manager = transform.parent.GetComponent<TankManager>();
        transform.FindChild("Tank Cannon").GetComponent<SpriteRenderer>().color = color;
        transform.FindChild("Tank Body").GetComponent<SpriteRenderer>().color = color;

        //Create the equipment dictionary with starting values
        equipment = new Dictionary<Equipment, int>();
        equipment[Equipment.Laser] = 0;
        equipment[Equipment.Mine] = 0;
    }

    protected override void Start()
    {
        base.Start();
        Spawn(GetComponent<Tank>(), 50, 50);  //Test only
        reversing = false;
        body = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Called at the beginning of every frame before other logic.
    /// </summary>
    protected void StartUpdate()
    {
        direction = Vector2.zero;
        movementThisFrame = body.position;
        fireCooldown -= Time.deltaTime;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        //Move the tank
        if (direction != Vector2.zero)
        {
            //Reverse if movement direction is reverse of facing direction, otherwise go forward
            if (Vector2.Angle(transform.up, direction) < 20)
                reversing = false;

            if (Vector2.Angle(transform.up, direction) > 160 || reversing)
            {
                transform.up = Vector2.Lerp(transform.up, -direction, 0.1f);
                movementThisFrame -= ((Vector2)transform.up * speed * Time.deltaTime);
                transform.Find("Left Tank Tread").GetComponent<Animator>().SetInteger("Direction", -1);
                transform.Find("Right Tank Tread").GetComponent<Animator>().SetInteger("Direction", -1);
                reversing = true;
            }
            else
            {
                transform.up = Vector2.Lerp(transform.up, direction, 0.1f);
                movementThisFrame += ((Vector2)transform.up * speed * Time.deltaTime);
                transform.Find("Left Tank Tread").GetComponent<Animator>().SetInteger("Direction", 1);
                transform.Find("Right Tank Tread").GetComponent<Animator>().SetInteger("Direction", 1);
            }

        }
        else
        {
            reversing = false;
            transform.Find("Left Tank Tread").GetComponent<Animator>().SetInteger("Direction", 0);
            transform.Find("Right Tank Tread").GetComponent<Animator>().SetInteger("Direction", 0);
        }

        //If this tank is on a conveyor belt, then move along it
        if (onConveyor)
        {
            movementThisFrame += conveyorDirection * Time.deltaTime;
        }

        //Move the tank if movement is expected this frame
        if (movementThisFrame != body.position)
        {
            body.MovePosition(movementThisFrame);
        }

        //If laser has no ammo left, or its taget has died, then stop firing
        if (equipment[Equipment.Laser] <= 0)
            StopFiringLaser();        

        //If laser has a target, damage it
        if(laserTarget != null)
        {
            if (laserTarget.GetComponent<Entity>().Health > 0)
                laserTarget.GetComponent<Entity>().TakeDamage(1);
        }
    }

    public void Spawn(Tank t, float x, float z)
    {
        activeTanks.Add(t);
    }

    protected void FireCannon()
    {
        if(!canFire)
            return;

        GameObject missile = Instantiate(Resources.Load("Shell"), transform.position + transform.Find("Tank Cannon").up * 0.4f, Quaternion.identity) as GameObject;
        missile.GetComponent<ProjectileControl>().direction = transform.Find("Tank Cannon").up;
        missile.GetComponent<ProjectileControl>().owner = gameObject;
        missile.GetComponent<ProjectileControl>().shellColor = color;
        transform.Find("Tank Cannon").GetComponent<Animator>().Play("TankCannonRecoil");
        fireCooldown = 0.4f;
    }

    protected void FireLaser()
    {
        if (!canFire)
            return;

        if (equipment[Equipment.Laser] <= 0)
            return;

        RaycastHit2D hit = Physics2D.Raycast(body.transform.position, transform.Find("Tank Cannon").up);
        if(hit.collider != null)
        {  
            if(currentLaser == null)
                currentLaser = Instantiate(Resources.Load("Laser"), transform.position, Quaternion.identity) as GameObject;

            float laserLength = (hit.collider.transform.position - transform.position).magnitude * 2;
            currentLaser.transform.up = transform.Find("Tank Cannon").up;
            currentLaser.transform.position = body.transform.position + (currentLaser.transform.up * (laserLength / 4));
            currentLaser.transform.localScale = new Vector3(currentLaser.transform.localScale.x, laserLength, 1);

            if (hit.collider.tag == "Wall")
                laserTarget = null;
            else
                laserTarget = hit.collider.gameObject;
        }
        equipment[Equipment.Laser]--;

    }

    protected void StopFiringLaser()
    {
        if (currentLaser != null)
            Destroy(currentLaser);
        laserTarget = null;
    }

    protected void AddEquipment(Equipment e, int amount)
    {
        //Change the amount of the specified equipment the player has
        int amountAdded = Mathf.Clamp(equipment[e] + amount, 0, e.MaxAmount()) - equipment[e];
        equipment[e] = Mathf.Clamp(equipment[e] + amount, 0, e.MaxAmount());

        if (controlledLocally)
        {
            //Create a string describing how much was added
            string display = "+" + amountAdded.ToString() + " " + e.AmmoName() + (amountAdded > 1 ? "s" : "") + (equipment[e] == e.MaxAmount() ? "(Max)" : "");

            //Display the string next to the tank
            GameObject displayText = Instantiate(Resources.Load("Disposable Text"), transform.position + new Vector3(1, 0, 0), Quaternion.identity) as GameObject;
            displayText.GetComponent<DisposableText>().text = display;
        }
    }

    protected override void DestroyEntity()
    {
        //Remove this tank from the static list of active tanks and create an explosion at the tanks position
        activeTanks.Remove(this);
        GameObject explosion = Instantiate(Resources.Load("Explosion"), body.position, Quaternion.Euler(0, 0, Random.Range(-79, 180))) as GameObject;
        explosion.transform.localScale = new Vector3(4.5f, 4.5f, 4.5f);
        explosion.GetComponent<SpriteRenderer>().color = new Color32(255, 195, 0, 255);

        //Destroy the tank for now, and prepare for respawn
        StopFiringLaser();
        onConveyor = false;
        transform.parent.GetComponent<TankManager>().DestroyTank();
    }

    public void MakeTransparent()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Color32 c = transform.GetChild(i).GetComponent<SpriteRenderer>().color;
            c.a = 155;
            transform.GetChild(i).GetComponent<SpriteRenderer>().color = c;
        }
    }

    public void MakeOpaque()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Color32 c = transform.GetChild(i).GetComponent<SpriteRenderer>().color;
            c.a = 255;
            transform.GetChild(i).GetComponent<SpriteRenderer>().color = c;
        }
    }

    protected void OnTriggerStay2D(Collider2D col)
    {
        //Called only if the player finishes spawning on top of an item
        if (col.gameObject.tag == "Pickupable")
        {
            if (!spawning)
            {
                AddEquipment(col.GetComponent<PickupableItem>().item, col.GetComponent<PickupableItem>().quantity);
                Destroy(col.gameObject);
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Conveyor Belt"))
        {
            onConveyor = true;
            conveyorDirection = new Vector2(0, col.gameObject.transform.rotation.eulerAngles.z == 0? 1.15f : -1.15f);
        }
        else if(col.gameObject.tag == "Pickupable")
        {
            if (!spawning)
            {
                AddEquipment(col.GetComponent<PickupableItem>().item, col.GetComponent<PickupableItem>().quantity);
                Destroy(col.gameObject);
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Conveyor Belt"))
        {
            onConveyor = false;
            conveyorDirection = new Vector2(0, 0);
        }
    }
}
