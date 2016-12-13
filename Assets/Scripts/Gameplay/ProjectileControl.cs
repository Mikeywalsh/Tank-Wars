using UnityEngine;

sealed public class ProjectileControl : Entity {

    public GameObject owner;
    public Color shellColor;
    public Vector3 direction;
    public float timeCreated;
    public float speed;
    public int maxBounce;

    private Vector2 lastWallHitNormal;
    private int bounceCount;

	protected override void Start () {
        base.Start();
        timeCreated = Time.time;
        transform.up = direction;
        bounceCount = 0;

        transform.GetChild(0).GetComponent<SpriteRenderer>().color = shellColor;
	}
	
	protected override void FixedUpdate () {
        base.FixedUpdate();

        if(Time.time - timeCreated > 1.5f)
        {
            DestroyEntity();
        }
        transform.position += transform.up * speed * Time.deltaTime;        
    }

    protected override void DestroyEntity()
    {
        Instantiate(Resources.Load("Explosion"), transform.position, Quaternion.Euler(0,0,transform.rotation.eulerAngles.z + 135));
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject == owner || col.gameObject.tag == "Ignore Projectiles")
            return;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject != owner || bounceCount > 0)
        {
            if (col.gameObject.tag == "Ignore Projectiles")
                return;

            if (col.gameObject.GetComponent<Entity>() != null)
            {
                col.GetComponent<Entity>().TakeDamage(new DamageContainer(owner.GetComponent<Tank>(), Equipment.Cannon));
            }
            else if(col.gameObject.tag == "Wall")
            {
                RaycastHit2D wallHit = Physics2D.Raycast(transform.position, col.transform.position - transform.position);
                if (wallHit.normal == lastWallHitNormal)
                {
                    return;
                }
                else if (wallHit.collider != null && bounceCount < maxBounce)
                {
                    lastWallHitNormal = wallHit.normal;
                    transform.up = Quaternion.AngleAxis(180, wallHit.normal) * transform.up * -1;
                    bounceCount++;
                    return;
                }
            }

            DestroyEntity();
        }
    }
}
