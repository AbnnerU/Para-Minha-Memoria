using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropInteraction : InteractionEvents, IDragAndDropInteractable
{
    [SerializeField] private bool alreadyInteract = false;

    [SerializeField] private bool consumeItem;

    [SerializeField] private InventoryItem itemReference;

    [SerializeField] private InteractionAction[] actions;

    public void OnClick(InventoryItem item, out bool consumeItem)
    {
        consumeItem = false;

        if (itemReference != item|| alreadyInteract)
            return;

        consumeItem = this.consumeItem;

        foreach (InteractionAction i in actions)
        {
            i.ExecuteAction();
        }

        OnClickEvent?.Invoke();

        alreadyInteract = true;

       
    }

    public void OnEnter(InventoryItem item)
    {
        if (itemReference != item || alreadyInteract)
            return;

        OnEnterEvent?.Invoke();

        print("Enter" + gameObject.name);
    }

    public void OnExit(InventoryItem item)
    {
        if (itemReference != item || alreadyInteract)
            return;

        OnExitEvent?.Invoke();

        print("Exit" + gameObject.name);
    }
}
