using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueControlPlayerMovement : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] private bool disableMovementOnStartDialogue=true;

    [SerializeField] private bool enableMovementOnEndDialogue = true;

    private List<IMovementGeneral> allMovementScripts = new List<IMovementGeneral>();

    private bool movementsEnabled = true;

    private void Awake()
    {
        if (dialogueManager == null)
            dialogueManager = FindObjectOfType<DialogueManager>();

        var reference = FindObjectsOfType<MonoBehaviour>().OfType<IMovementGeneral>();

        foreach (IMovementGeneral mg in reference)
        {
            allMovementScripts.Add(mg);
        }

        dialogueManager.OnNewDialogueSetence += DialogueManager_OnNewDialogue;

        dialogueManager.OnEnd += DialogueManager_OnEnd;
    }

    private void DialogueManager_OnNewDialogue(string arg1, string arg2)
    {
        if(movementsEnabled && disableMovementOnStartDialogue)
        {            
            foreach(IMovementGeneral mg in allMovementScripts)
            {
                mg.Disable();
            }

            movementsEnabled = false;

        }

    }

    private void DialogueManager_OnEnd()
    {
        if (enableMovementOnEndDialogue)
        {         
            foreach (IMovementGeneral mg in allMovementScripts)
            {
                mg.Enable();
            }

            movementsEnabled = true;
        }
    }

    
}
