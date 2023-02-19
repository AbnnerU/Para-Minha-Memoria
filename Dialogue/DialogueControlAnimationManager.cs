
using UnityEngine;

public class DialogueControlAnimationManager : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] private bool disableAnimationManagerOnStartDialogue = true;

    [SerializeField] private bool enableAnimationManagerOnEndDialogue = true;

    [Header("On disable")]
    [SerializeField] private string onDisableFParameter = "Input";
    [SerializeField] private float onDisableFValue = 0;
    [SerializeField] private string onDisableBParameter = "OnGround";
    [SerializeField] private bool onDisableBValue = true;

    private AnimationManager animationManager;

    private bool animationEnabled = true;

    private void Awake()
    {
        if (dialogueManager == null)
            dialogueManager = FindObjectOfType<DialogueManager>();

        animationManager = FindObjectOfType<AnimationManager>();

        dialogueManager.OnNewDialogueSetence += DialogueManager_OnNewDialogue;

        dialogueManager.OnEnd += DialogueManager_OnEnd;
    }

    private void DialogueManager_OnNewDialogue(string arg1, string arg2)
    {
        if ( disableAnimationManagerOnStartDialogue)
        {
            animationManager.SetActive(false);
            animationManager.SetFloat(onDisableFParameter, onDisableFValue);
            animationManager.SetBool(onDisableBParameter, onDisableBValue);
        }

    }

    private void DialogueManager_OnEnd()
    {
        if (enableAnimationManagerOnEndDialogue)
        {
            animationManager.SetActive(true);
        }
    }
}
