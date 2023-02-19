using System;
using UnityEngine;

public abstract class InventoryGeneral : MonoBehaviour
{
    public Action<InventoryItem> OnAddNewItem;

    public Action<InventoryItem> OnRemoveItem;

    public abstract bool CanAddNewItem();
   
    public abstract void AddItem(InventoryItem newItem);

    public abstract void RemoveItem(InventoryItem itemReference);

    public abstract bool HaveItem(InventoryItem itemReference);
    

}
