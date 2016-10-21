using UnityEngine;

sealed public class PickupableItem : InteractableItem
{
    public int itemID;

    protected override void Start()
    {
        base.Start();
        itemID = Random.Range(0, 4);
    }
}