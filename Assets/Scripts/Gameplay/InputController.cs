using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	public Vector2 MoveDirection()
    {
        Vector2 direction = Vector2.zero;

        if (ControllerConntected)
        {
            if(new Vector2(Input.GetAxis("LeftStickH"), Input.GetAxis("LeftStickV")).magnitude > 0.3f)
                direction = new Vector2(Input.GetAxis("LeftStickH"), Input.GetAxis("LeftStickV"));
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
                direction += new Vector2(0, 1);
            if (Input.GetKey(KeyCode.S))
                direction += new Vector2(0, -1);
            if (Input.GetKey(KeyCode.A))
                direction += new Vector2(-1, 0);
            if (Input.GetKey(KeyCode.D))
                direction += new Vector2(1, 0);
        }

        return direction;        
    }

    public Vector2 AimDirection(Vector2 currentDirection)
    {
        Vector2 direction = currentDirection;

        if (ControllerConntected)
        {
            if (new Vector2(Input.GetAxis("RightStickH"), Input.GetAxis("RightStickV")).magnitude > 0.3f)
                direction = new Vector2(Input.GetAxis("RightStickH"), Input.GetAxis("RightStickV"));
        }
        else
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            direction = mousePos - transform.Find("Tank Cannon").position;
        }

        return direction;
    }

    public bool FirePrimary()
    {
        bool fire = false;

        if(ControllerConntected)
        {
            fire = Input.GetAxis("RightTrigger") > 0.3f;
        }
        else
        {
            fire = Input.GetMouseButton(0);
        }

        return fire;
    }

    public bool FireSecondary()
    {
        bool fire = false;

        if (ControllerConntected)
        {
            fire = Input.GetAxis("LeftTrigger") > 0.3f;
        }
        else
        {
            fire = Input.GetMouseButton(1);
        }

        return fire;
    }

    public bool SpeedBoost()
    {
        bool boost = false;

        if (ControllerConntected)
        {
            boost = Input.GetButton("Boost");            
        }
        else
        {
            boost = Input.GetKey(KeyCode.LeftShift);
        }

        return boost;
    }

    private static bool ControllerConntected
    {
        get { return Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0].Contains("Controller"); }
    }
}
