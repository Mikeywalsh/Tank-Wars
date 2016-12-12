using UnityEngine;
using System.Collections.Generic;

public class ItemCreator : MonoBehaviour {

    public List<GameObject> possibleItems;
    public Vector2 direction;
    public float lastItemTime;
    public float creationCooldown;

	void Start () {
        lastItemTime = -10;
	}
	
	void Update () {
        if(Time.time - lastItemTime > creationCooldown)
        {
            GameObject nextItem = Instantiate(possibleItems[Random.Range(0, possibleItems.Count)], transform.position, Quaternion.Euler(0, 0, Random.Range(-179, 180))) as GameObject;
            nextItem.GetComponent<PickupableItem>().direction = direction;
            lastItemTime = Time.time;
        }	
	}
}