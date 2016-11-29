using UnityEngine;
using System.Collections;

public class Pathfind : MonoBehaviour {

    public bool ShowInEditor;
    
    private bool[,] Map = new bool[,] { { true,true,true,false,true,true,true,true}, { true, true, true, false, true, true, true, true }, { true, true, true, false, true, true, true, true }, { true, true, true, false, true, true, true, true }, { true, true, true, false, true, true, true, true }, {true, false, false, false, false, false, true, false }, {true,true,true,true,true,true,true,true }, { true, true, true, true, true, true, true, true } };
    private Vector3 MapStart = new Vector3(-3.5f, 0, 3.5f);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        if (!ShowInEditor)
            return;

        for(int y = 0; y < Map.GetLength(1); y++)
        {
            for(int x = 0; x < Map.GetLength(0); x++)
            {
                Gizmos.color = Map[y, x] ? Color.green : Color.red;
                Gizmos.DrawCube(new Vector3(MapStart.x + x, 0.125f + (Map[y, x] ? 0 : 1), MapStart.z - y), new Vector3(1, 0.25f, 1));
            }
        }

    }
}
