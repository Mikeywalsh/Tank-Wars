using UnityEngine;

public class Entity : MonoBehaviour {

    public int maxHealth;
    public int health;

    private static Vector2 mapStart = new Vector2(-9.5f, -7.5f);
    private static Vector2 mapEnd = new Vector2(9.5f, 7.5f);

	protected virtual void Start () {
        health = MaxHealth;
	}	

    protected virtual void FixedUpdate()
    {
        //Debug.Log(((Tank)this).PlayerName + (GameObject.Find("Player").transform.FindChild("Tank").GetComponent<Tank>() == ((Tank)this)).ToString());
        if (transform.position.x < mapStart.x || transform.position.y < mapStart.y || transform.position.x > mapEnd.x || transform.position.y > mapEnd.y)
            DestroyEntity();
    }

    public void TakeDamage(DamageContainer d)
    {
        //Change health of entity
        health = Mathf.Clamp(health - d.DamageAmount, 0, maxHealth);

        //Update health bar if entity is tank
        if (this is Tank)
        {
            //Debug.Log();
            transform.parent.GetComponent<TankManager>().UpdateHealthBar(maxHealth, health, d.DamageAmount);
        }

        if (health == 0)
        {
            //If a tank is being destroyed, update the killfeed
            if(this is Tank)
            {
                //Debug.Log(d.Source.PlayerName);
                if(d.Source == (Tank)this || d.Source == null)
                    KillFeedController.AddToQueue(d.Weapon.KillString(d.Source == (Tank)this, d.Source == null), (Tank)this);
                else
                    KillFeedController.AddToQueue(d.Weapon.KillString(d.Source == this, d.Source == null), d.Source, (Tank)this);
            }

            //Now destroy the entity
            DestroyEntity();
        }
    }

    public void Heal(int amount)
    {
        health = Mathf.Clamp(Health + amount, 0, maxHealth);
    }

    protected virtual void DestroyEntity()
    {
        Destroy(gameObject);
    }

    //Getters
    public int MaxHealth
    {
        get { return maxHealth; }
    }

    public int Health
    {
        get { return health; }
    }
}
