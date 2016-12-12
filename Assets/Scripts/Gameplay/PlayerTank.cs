using UnityEngine;
using System.Collections;

sealed public class PlayerTank : Tank {

    private InputController inputController;

    public bool myTank;

	protected override void Start()
    {
        inputController = GetComponent<InputController>();
        base.Start();
        //Spawn(GetComponent<PlayerTank>(), 50, 50);  //Test only
	}

    protected override void FixedUpdate()
    {
        StartUpdate();

        if (!myTank)
            return;

        //Allow the player to speed up the tank
        if (inputController.SpeedBoost())
            speed = 4;
        else
            speed = 2;

        //Allow the player to move the tank
        direction = inputController.MoveDirection();

        //Aim the tanks gun at the mouse's position
        transform.Find("Tank Cannon").up = inputController.AimDirection(transform.Find("Tank Cannon").up);

        //Allow the player to fire the tanks gun
        if (inputController.FirePrimary() && fireCooldown <= 0)
        {
            FireCannon();
        }

        //Laser Testing
        if (inputController.FireSecondary())
        {
            FireLaser();
        }
        if(!inputController.FireSecondary())
        {
            StopFiringLaser();
        }

        base.FixedUpdate();
    }
}