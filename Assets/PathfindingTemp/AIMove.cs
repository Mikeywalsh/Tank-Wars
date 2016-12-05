using UnityEngine;
using System.Collections.Generic;

public class AIMove : MonoBehaviour {

    List<Vector3> path = new List<Vector3>();
    bool pathFound;
    Vector3 next;
    bool stop;

	// Use this for initialization
	void Start () {
        pathFound = false;
        stop = true;
	}
	
	// Update is called once per frame
	void Update () {
	    if(!pathFound)
        {
            if(GameObject.Find("Controller").GetComponent<Pathfind>().path.Count > 0)
            {
                path = GameObject.Find("Controller").GetComponent<Pathfind>().path;
                pathFound = true;
                stop = false;
            }
            return;
        }

        if (stop)
            return;

        next = path[0];

        transform.Translate((next - transform.position).normalized * 0.05f);
        if((next - transform.position).magnitude < 0.1f)
        {
            path.RemoveAt(0);
            if (path.Count > 0)
                next = path[0];
            else
                stop = true;
        }
	}
}
