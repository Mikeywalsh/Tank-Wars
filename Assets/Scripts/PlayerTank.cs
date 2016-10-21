using UnityEngine;
using System.Collections;

public class PlayerTank : Tank {

    public bool myTank;

	protected override void Start()
    {
        base.Start();
        //Spawn(GetComponent<PlayerTank>(), 50, 50);  //Test only
	}

    protected override void FixedUpdate()
    {
        StartUpdate();

        if (!myTank)
            return;

        //Allow the player to speed up the tank
        if (Input.GetKey(KeyCode.LeftShift))
            speed = 4;
        else
            speed = 2;

        //Allow the player to move the tank
        if (Input.GetKey(KeyCode.W))
            direction += new Vector2(0, 1);
        if (Input.GetKey(KeyCode.S))
            direction += new Vector2(0, -1);
        if (Input.GetKey(KeyCode.A))
            direction += new Vector2(-1, 0);
        if (Input.GetKey(KeyCode.D))
            direction += new Vector2(1, 0);

        //Aim the tanks gun at the mouse's position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.Find("Tank Cannon").up = mousePos - transform.Find("Tank Cannon").position;

        //Allow the player to fire the tanks gun
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && fireCooldown <= 0)
        {
            FireCannon();
        }

        //Laser Testing
        if(Input.GetMouseButton(1))
        {
            FireLaser();
        }
        if(!Input.GetMouseButton(1))
        {
            StopFiringLaser();
        }

        base.FixedUpdate();
    }
}