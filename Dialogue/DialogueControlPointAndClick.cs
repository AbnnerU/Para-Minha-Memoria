using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueControlPointAndClick : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;

    private PointAndClick pointAndClick;
    private PointAndClickInventoryVisual pointAndClickInventoryVisual;

    private void Awake()
    {
        if (dialogueManager == null)
            dialogueManager = FindObjectOfType<DialogueManager>();

        pointAndClick = FindObjectOfType<PointAndClick>();
        pointAndClickInventoryVisual = FindObjectOfType<PointAndClickInventoryVisual>();

        dialogueManager.OnNewDialogueSetence += DialogueManager_OnNewDialogue;

        dialogueManager.OnEnd += DialogueManager_OnEnd;
    }

    private void DialogueManager_OnNewDialogue(string arg1, string arg2)
    {
        StartCoroutine(WaitOneFrame());
    }

    IEnumerator WaitOneFrame()
    {
        yield return null;
        pointAndClick.SetDetectionActive(false);

        if (pointAndClickInventoryVisual)
            pointAndClickInventoryVisual.SetDetectionActive(false);
    }

    private void DialogueManager_OnEnd()
    {
        print("Entrou 2");
        if (pointAndClick != null)
            pointAndClick.SetDetectionActive(true);

        if (pointAndClickInventoryVisual)
            pointAndClickInventoryVisual.SetDetectionActive(true);

        print("Habilita");

    }

}
