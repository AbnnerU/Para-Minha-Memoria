using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBoxAnimation : MonoBehaviour
{
    [SerializeField] private DialogueVisual dialogueVisual;

    [Header("Components")]
    [Space(5)]
    [SerializeField] private RectTransform dialoguePanel;
    [SerializeField] private TMP_Text[] textsComponents;

    [Header("FadeIn")]
    [SerializeField] private float fadeInTime;
    [SerializeField] private float fadeInStartScale = 0.1f;
    [SerializeField] private bool disableTextsWhileFadeIn;
    [Header("FadeOut")]
    [SerializeField] private bool enableDialoguePanel = true;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float fadeOutEndScale = 0f;
    [SerializeField] private bool disableTextsWhileFadeOut;

    private void Awake()
    {
        if (dialogueVisual == null)
            dialogueVisual = FindObjectOfType<DialogueVisual>();

        dialogueVisual.OnChangeDialogueActiveState += DialogueVisual_OnChangeDialogueActiveState;  
    }

    private void DialogueVisual_OnChangeDialogueActiveState(bool active)
    {
        if (active)
        {
            StopAllCoroutines();

            StartCoroutine(FadeIn());
        }
        else
        {
            StopAllCoroutines();

            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        if (disableTextsWhileFadeIn)
        {
            for (int i=0; i < textsComponents.Length; i++)
            {
                textsComponents[i].enabled = false;
            }
        }

        float currentTime = 0;
        float scaleDiff = 1 - fadeInStartScale;
        float percentage = 0;
        float currentValue = 0;

        dialoguePanel.localScale = new Vector3(fadeInStartScale, fadeInStartScale, fadeInStartScale);

        do
        {
            currentTime += Time.deltaTime;
            percentage = ((currentTime * 100) / fadeInTime) / 100;

            currentValue = fadeInStartScale + (scaleDiff * percentage);

            dialoguePanel.localScale = new Vector3(currentValue, currentValue, currentValue);

            yield return null;

        } while (currentTime<fadeInTime);

        dialoguePanel.localScale = new Vector3(1, 1, 1);

        if (disableTextsWhileFadeIn)
        {
            for (int i = 0; i < textsComponents.Length; i++)
            {
                textsComponents[i].enabled = true;
            }
        }

        yield break;
    }

    IEnumerator FadeOut()
    {
        if (enableDialoguePanel)
            dialoguePanel.gameObject.SetActive(true);

        if (disableTextsWhileFadeOut)
        {
            for (int i = 0; i < textsComponents.Length; i++)
            {
                textsComponents[i].enabled = false;
            }
        }

        float currentTime = 0;
        float scaleDiff = fadeOutEndScale -1 ;
        float percentage = 0;
        float currentValue = 0;

        do
        {
            currentTime += Time.deltaTime;
            percentage = ((currentTime * 100) / fadeOutTime) / 100;

            currentValue = 1 + (scaleDiff * percentage);

            dialoguePanel.localScale = new Vector3(currentValue, currentValue, currentValue);

            yield return null;

        } while (currentTime < fadeOutTime);

        dialoguePanel.localScale = new Vector3(fadeOutEndScale, fadeOutEndScale, fadeOutEndScale);

        if (disableTextsWhileFadeOut)
        {
            for (int i = 0; i < textsComponents.Length; i++)
            {
                textsComponents[i].enabled = true;
            }
        }

        if (enableDialoguePanel)
            dialoguePanel.gameObject.SetActive(false);

        yield break;
    }
}
