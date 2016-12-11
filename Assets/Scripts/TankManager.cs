using UnityEngine;
using System.Collections;

public class TankManager : MonoBehaviour {

    private GameObject tank;
    private GameObject frame;
    private float deathTime;
    private float SpawnTime;
    private bool dead;

    void Start()
    {
        tank = transform.FindChild("Tank").gameObject;
        frame = transform.FindChild("Frame").gameObject;

        //StartSpawn();
    }

    void FixedUpdate()
    {
        frame.transform.position = tank.transform.position + new Vector3(0, 0.95f, 0);

        if (dead && Time.time - deathTime > LevelController.DeathDuration)
            Spawn();

        if (tank.GetComponent<Tank>().spawning && Time.time - SpawnTime > LevelController.SpawnDuration)
            EndSpawn();
    }

    public void UpdateHealthBar(int maxHealth, int health, int damage)
    {
        frame.GetComponent<FrameController>().UpdateHealth(maxHealth, health, damage);
    }

    public void DestroyTank()
    {
        tank.SetActive(false);
        frame.SetActive(false);
        deathTime = Time.time;
        dead = true;
    }

    private void Spawn()
    {
        //Enable this tank and set its location to the most optimal spawn location
        tank.SetActive(true);
        tank.transform.position = LevelController.OptimalSpawnLocation();

        //Enable the tank frame but disable the healthbar, so only the name is displayed
        frame.gameObject.SetActive(true);
        transform.Find("Frame/Health Bar").gameObject.SetActive(false);

        //Make this tank transparent, heal it to max health and disable firing and projectile collisions
        tank.GetComponent<Tank>().MakeTransparent();
        tank.GetComponent<Tank>().Heal(tank.GetComponent<Tank>().MaxHealth);
        tank.GetComponent<Tank>().canFire = false;
        tank.tag = "Ignore Projectiles";
        tank.layer = 2; //Ignore Raycast

        //Set the spawning flag and record the spawn time
        dead = false;
        SpawnTime = Time.time;
        tank.GetComponent<Tank>().spawning = true;

        //Add this tank to the static list of active tanks
        Tank.activeTanks.Add(tank.GetComponent<Tank>());
    }

    private void EndSpawn()
    {
        //Make the tank opaque and enable firing and projectile collisions
        tank.GetComponent<Tank>().MakeOpaque();
        tank.GetComponent<Tank>().canFire = true;
        tank.gameObject.tag = "Tank";
        tank.layer = 0; //Default

        //Enable the health bar and reset it to be full
        transform.Find("Frame/Health Bar").gameObject.SetActive(true);
        frame.GetComponent<FrameController>().ResetHealth();
        tank.GetComponent<Tank>().spawning = false;
    }
}
