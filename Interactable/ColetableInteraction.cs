using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColetableInteraction : InteractionEvents,IInteractable
{
    //[SerializeField] private bool active = true;

    [SerializeField] private InventoryItem itemAttributes;

    [SerializeField] private bool disableWhenPicked=true;

    [SerializeField] private InteractionAction[] actions;

    private InventoryGeneral inventoryReference;

    private void Awake()
    {
        inventoryReference = GameObject.FindGameObjectWithTag(itemAttributes.inventoryTag).GetComponent<InventoryGeneral>();

        if (inventoryReference == null)
            Debug.LogError(GameObject.FindGameObjectWithTag(itemAttributes.inventoryTag).name +" inventory whit this name don't exist");
    }

    public void OnClick()
    {
        if (inventoryReference == null || inventoryReference.CanAddNewItem() == false)
            return;

        inventoryReference.AddItem(itemAttributes);

        foreach (InteractionAction i in actions)
        {
            i.ExecuteAction();
        }

        OnClickEvent?.Invoke();

        if (disableWhenPicked)
            gameObject.SetActive(false);

    }

    public void OnEnter()
    {
        OnEnterEvent?.Invoke();

        print("Enter" + gameObject.name);
    }

    public void OnExit()
    {
        OnExitEvent?.Invoke();

        print("Exit" + gameObject.name);
    }
}
