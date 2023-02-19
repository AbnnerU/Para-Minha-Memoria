
using UnityEngine;

public class DialogueControlTextPage : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] private TextScreen textScreen;

    [SerializeField] private bool disableTextScreenOnStartDialogue = true;

    [SerializeField] private bool enableTextScreenOnEndDialogue = true;

    private bool textScreenEnabled = true;

    private void Awake()
    {
        if (dialogueManager == null)
            dialogueManager = FindObjectOfType<DialogueManager>();

        dialogueManager.OnNewDialogueSetence += DialogueManager_OnNewDialogue;

        dialogueManager.OnEnd += DialogueManager_OnEnd;
    }

    private void DialogueManager_OnNewDialogue(string arg1, string arg2)
    {
        if (textScreenEnabled && disableTextScreenOnStartDialogue)
        {
            textScreen.DisableTextScreen();

            textScreenEnabled = false;
        }    

    }

    private void DialogueManager_OnEnd()
    {
        if (enableTextScreenOnEndDialogue)
        {
            textScreen.EnableTextScreen();

            textScreenEnabled = true;
        }
    }

}
