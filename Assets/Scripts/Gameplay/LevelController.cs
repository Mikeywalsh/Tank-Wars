using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A singleton used to contain data about and control the current level.
/// </summary>
public class LevelController : MonoBehaviour {

    public static LevelController controller;

    public float deathDuration;
    public float spawnDuration;
    public GameObject spawnLocationHolder;
    public int mapWidth;
    public int mapHeight;

    private List<Transform> spawnLocations = new List<Transform>();

    private void Awake()
    {
        Application.runInBackground = true;
        controller = this;
    }

    private void Start () {
        //Load all possible spawn locations into the holder array
        for(int i = 0; i < spawnLocationHolder.transform.childCount; i++)
        {
            spawnLocations.Add(spawnLocationHolder.transform.GetChild(i));
        }
	}

    /// <summary>
    /// Finds the optimal spawn location for the next tank to spawn. The optimal location is the spawn point furthest from all existing tanks.
    /// </summary>
    /// <returns>The 3D coordinates of the optimal spawn location.</returns>
    public static Vector3 OptimalSpawnLocation()
    {
        float maxDistance = float.MinValue;
        int maxDistanceIndex = 0;

        for(int i = 0; i < controller.spawnLocations.Count; i++)
        {
            foreach(Tank t in Tank.activeTanks)
            {
                if((controller.spawnLocations[i].position - t.transform.position).magnitude > maxDistance)
                {
                    maxDistance = (controller.spawnLocations[i].position - t.transform.position).magnitude;
                    maxDistanceIndex = i;
                }
            }
        }

        return controller.spawnLocations[maxDistanceIndex].position;
    }

    public static float DeathDuration
    {
        get { return controller.deathDuration; }
    }

    public static float SpawnDuration
    {
        get { return controller.spawnDuration; }
    }

    public static int MapWidth
    {
        get { return controller.mapWidth; }
    }

    public static int MapHeight
    {
        get { return controller.mapHeight; }
    }
}