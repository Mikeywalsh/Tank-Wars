using UnityEngine;

public class Entity : MonoBehaviour {

    public int maxHealth;

    public int health;

	protected virtual void Start () {
        health = MaxHealth;
	}	

    public void TakeDamage(int amount)
    {
        //Change health of entity
        health = Mathf.Clamp(health - amount, 0, maxHealth);

        //Update health bar if entity is tank
        if (this is Tank)
        {
            //Debug.Log();
            transform.parent.GetComponent<TankManager>().UpdateHealthBar(maxHealth, health, amount);
        }

        if (health == 0)
            DestroyEntity();
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
