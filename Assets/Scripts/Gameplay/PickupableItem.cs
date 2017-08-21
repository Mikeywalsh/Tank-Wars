﻿using UnityEngine;
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
        item = equipmentList[UnityEngine.Random.Range(1,equipmentList.Length)];
        quantity = UnityEngine.Random.Range(item.PickupAmountMin(), item.PickupAmountMax() + 1);
    }          
}