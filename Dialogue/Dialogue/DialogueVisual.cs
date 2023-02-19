using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueVisual : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text namesText;
    [SerializeField] private TMP_Text setenceText;

    [SerializeField] private bool startDisabled;
    [SerializeField] private bool disableOnEnd;
  
    [SerializeField] private float textWriteSpeed = 0.05f;

    [HideInInspector]
    public List<Button> buttonsComponent;

    public Action OnNewText;

    private DialogueManager dialogueManager;

    private TMP_TextInfo _textInfo;

    public Action<bool> OnChangeDialogueActiveState;

    public Action OnWritingText;

    public Action OnStopWriting;

    private int level = 0;

    private int currentOptIndex;

    private string currentButtonText;


    
    private void Awake()
    {
        _textInfo = setenceText.textInfo;
   
        dialogueManager = FindObjectOfType<DialogueManager>();

        dialogueManager.OnNewDialogueSetence += DialogueManager_OnNewDialogue;

        dialogueManager.OnSkipWriting += DialogueManager_OnSkipWriting;

        dialogueManager.OnEnd += DialogueManager_OnEnd;


        if (startDisabled)
            dialoguePanel.SetActive(false);
     
    }

    private void DialogueManager_OnEnd()
    {
        if (disableOnEnd)
        { 
            dialoguePanel.SetActive(false);
            OnChangeDialogueActiveState?.Invoke(false);
            OnStopWriting?.Invoke();
        }
    }

    private void DialogueManager_OnSkipWriting()
    {      
        StopAllCoroutines();
        setenceText.ForceMeshUpdate();

        int totalCharacters = _textInfo.characterCount;

        setenceText.maxVisibleCharacters = totalCharacters;

        OnStopWriting?.Invoke();

        dialogueManager.FinishedWriting();
    }

    private void DialogueManager_OnNewDialogue(string name, string setence)
    {
        if (dialoguePanel.activeSelf == false)
        {
            dialoguePanel.SetActive(true);
            OnChangeDialogueActiveState?.Invoke(true);
        }

        namesText.text = name;
        
        setenceText.text = setence;

        setenceText.ForceMeshUpdate();

        OnNewText?.Invoke();

        //dialogueManager.FinishedWriting();
        setenceText.maxVisibleCharacters = 0;
       
        StartCoroutine(WriteSetence());
    }

    private int StringToInt(string text)
    {
        int textToInt;

        if (int.TryParse(text, out textToInt))
        {
            return textToInt;
        }
        else
        {
            Debug.LogError("O valor passado no texto '" + text + "' não pode ser convertido para int");
            return -1;
        }
    }

    IEnumerator WriteSetence()
    {
        setenceText.ForceMeshUpdate();

        int totalCharacters = _textInfo.characterCount;

        int count = 0;

        OnWritingText?.Invoke();

        while (count < totalCharacters)
        {
            count++;
            setenceText.maxVisibleCharacters = count;

            yield return new WaitForSecondsRealtime(textWriteSpeed);
        }

        OnStopWriting?.Invoke();

        dialogueManager.FinishedWriting();
    }

}
