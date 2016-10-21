using UnityEngine;
using System.Collections;

public class ExplosionControl : MonoBehaviour {

    public float timeCreated;

    void Start()
    {
        timeCreated = Time.time;
    }

    void Update()
    {
        if (Time.time - timeCreated > 0.33f)
        {
            Destroy(gameObject);
        }
    }
}
