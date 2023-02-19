using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlimitedInteraction : InteractionEvents, IInteractable
{
    [SerializeField] private InteractionAction[] actions;

    public void OnClick()
    {      
        foreach (InteractionAction i in actions)
        {
            i.ExecuteAction();
        }

        OnClickEvent?.Invoke();
    }

    public void OnEnter()
    {       
        //print("Enter" + gameObject.name);

        OnEnterEvent?.Invoke();
    }

    public void OnExit()
    {      
        //print("Exit" + gameObject.name);

        OnExitEvent?.Invoke();
    }
}
