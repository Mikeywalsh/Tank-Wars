using UnityEngine;
using System.Collections;

public class AITank : Tank {

    public bool aggressive;
    public bool avoiding;

    private Tank closestTarget;
    private float closestTargetDistance;

    protected override void Start()
    {
        base.Start();
        closestTargetDistance = float.MaxValue;
	}

    protected override void FixedUpdate()
    {
        StartUpdate();

        //Find nearest player
        if (activeTanks.Count != 1)
        {
            if (closestTarget != null && !activeTanks.Contains(closestTarget))
                closestTarget = null;

            foreach (Tank t in activeTanks)
            {
                if (t != this)
                {
                    if (closestTarget == null || closestTarget == t || (!t.spawning || closestTarget.spawning) && (body.position - t.body.position).magnitude < closestTargetDistance || closestTarget.spawning && !t.spawning)
                    {
                        if (closestTarget != t)
                            closestTarget = t;
                        direction = t.body.position - body.position;
                        closestTargetDistance = direction.magnitude;
                    }
                }
            }
        }
        else
        {
            closestTarget = null;
            closestTargetDistance = int.MaxValue;
            direction = Vector2.zero;
        }

        //If spawning, avoid other tanks
        avoiding = spawning;

        //Move away from closest target if avoiding
        if (avoiding)
            direction *= -1;

        //Aim cannon at nearest player and fire
        if (closestTarget != null)
        {
            transform.Find("Tank Cannon").up = (Vector3)closestTarget.body.position - transform.Find("Tank Cannon").position;

            //Only fire weapons if the tank has a clear shot on the target
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);

            if(hit.collider != null && hit.collider.tag == "Tank" && fireCooldown <= 0 && aggressive)
            {
                if (equipment[Equipment.Laser] > 0)
                    FireLaser();
                else
                    FireCannon();
            }
            else if(currentLaser != null)
            {
                StopFiringLaser();
            }
        }
        else if (currentLaser != null)
        {
            StopFiringLaser();
        }

        base.FixedUpdate();
    }
}
