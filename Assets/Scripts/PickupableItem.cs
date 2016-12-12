using UnityEngine;
using System;
using System.Collections.Generic;

sealed public class PickupableItem : InteractableItem
{
    public Equipment item;
    public int quantity;

    protected override void Start()
    {
        base.Start();

        Equipment[] equipmentList = (Equipment[])Enum.GetValues(typeof(Equipment));
        item = equipmentList[UnityEngine.Random.Range(0,equipmentList.Length)];
        quantity = UnityEngine.Random.Range(0, 100);
    }          
}