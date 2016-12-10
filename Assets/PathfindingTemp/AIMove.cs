using UnityEngine;
using System.Collections.Generic;

public class AIMove : MonoBehaviour {

    List<Vector3> path = new List<Vector3>();
    public bool ShowInEditor;
    bool pathFound;
    Vector3 next;
    bool stop;
    float lastPathCalc;
    Pathfinder pathFinder;

	// Use this for initialization
	void Start () {
        Pathfinder.SetMap(new bool[,] { { true, true, true, true, true, true, true, true }, { true, true, true, true, true, false, true, true }, { true, true, true, true, true, false, true, true }, { false, false, false, false, false, false, true, true }, { true, true, true, true, true, false, true, true }, { true, true, true, true, true, false, true, true }, { true, true, true, true, true, true, true, true }, { true, true, true, true, true, false, true, true } }, new Vector3(-3.5f, 0.5f, 3.5f));
        pathFinder = new Pathfinder();
        pathFound = false;
        stop = true;
	}
	
	// Update is called once per frame
	void Update () {
	    if(!pathFound || Time.time - lastPathCalc > 2)
        {
            path = pathFinder.FindPath(transform.position, GameObject.Find("Target").transform.position);
            if(path.Count != 0)
            {
                pathFound = true;
                lastPathCalc = Time.time;
                stop = false;
            }
            return;
        }

        if (stop)
        {
            pathFound = false;
            return;
        }

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

    void OnDrawGizmos()
    {
        if (!ShowInEditor)
            return;

        Gizmos.color = Color.magenta;
        if (path.Count > 0)
            Gizmos.DrawLine(GameObject.Find("AI").transform.position, path[0]);

        for (int i = 0; i < path.Count - 1; i++)
        {
            Gizmos.DrawLine(path[i], path[i + 1]);
        }
    }
}
