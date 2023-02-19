using System;
using System.Collections.Generic;
using UnityEngine;
 

public class PointAndClickInventory : InventoryGeneral
{
    [SerializeField] private int numberOfSlots;

    [SerializeField]private List<InventoryItem> items = new List<InventoryItem>();

    public override bool CanAddNewItem()
    {
        if (items.Count < numberOfSlots)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void AddItem(InventoryItem newItem)
    {
        if (CanAddNewItem() == false)
            return;

        items.Add(newItem);

        OnAddNewItem?.Invoke(newItem);
    }

    public override void RemoveItem(InventoryItem itemReference)
    {
        if (HaveItem(itemReference) == false)
            return;

        items.Remove(itemReference);


        OnRemoveItem?.Invoke(itemReference);
    }

    public override bool HaveItem(InventoryItem itemReference)
    {
        if (items.Contains(itemReference))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
