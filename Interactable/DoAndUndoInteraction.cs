using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoAndUndoInteraction : InteractionEvents,IInteractable
{
    [SerializeField] private InteractionState nextState;

    [SerializeField] private InteractionAction[] doActions;

    [Space(10)]
    [SerializeField] private InteractionAction[] undoActions;


    public enum InteractionState
    {
        DOACTION,
        UNDOACTION
    }

    public void OnClick()
    {
        print("Cliked" + gameObject.name);

        OnClickEvent?.Invoke();

        if (nextState == InteractionState.DOACTION)
        {
            foreach (InteractionAction i in doActions)
            {
                i.ExecuteAction();
            }

            nextState = InteractionState.UNDOACTION;
        }
        else
        {
            foreach (InteractionAction i in undoActions)
            {
                i.ExecuteAction();
            }

            nextState = InteractionState.DOACTION;
        }

    }

    public void OnEnter()
    {
        print("Enter" + gameObject.name);
        
        OnEnterEvent?.Invoke();
    }

    public void OnExit()
    {
        print("Exit" + gameObject.name);

        OnExitEvent?.Invoke();
    }
}
