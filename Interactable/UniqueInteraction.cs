using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueInteraction : InteractionEvents, IInteractable
{
    [SerializeField] private bool alreadyInteract = false;

    [SerializeField] private InteractionAction[] actions;

    public void OnClick()
    {
        if (alreadyInteract)
            return;

        foreach(InteractionAction i in actions)
        {
            i.ExecuteAction();
        }

        OnClickEvent?.Invoke();

        alreadyInteract = true;

    }

    public void OnEnter()
    {
        if (alreadyInteract)
            return;

        OnEnterEvent?.Invoke();

        print("Enter" + gameObject.name);
    }

    public void OnExit()
    {
        if (alreadyInteract)
            return;

        OnExitEvent?.Invoke();

        print("Exit" + gameObject.name);
    }
}
