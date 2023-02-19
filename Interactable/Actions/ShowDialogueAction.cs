using UnityEngine;

public class ShowDialogueAction : InteractionAction
{
    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] private DialogueAsset dialogueAsset;

    [SerializeField] private OnEndDialogueEvent OnEndDialogue;

    public override void ExecuteAction()
    {
        if (dialogueManager==null)
            dialogueManager = FindObjectOfType<DialogueManager>();

        dialogueManager.SetNewDialogue(dialogueAsset,OnEndDialogue);
    }
}
