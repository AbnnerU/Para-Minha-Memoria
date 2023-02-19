using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnEndDialogueEvent : UnityEvent { }

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private bool active = false;

    [SerializeField] private bool disableOnEnd=true;

    [SerializeField] private string gameLanguage;

    [SerializeField] private DialogueAsset dialogue;

    public List<string> dialogueText=new List<string>();

    public Dictionary<string, AnswersOptionsLine> answersGroup = new Dictionary<string, AnswersOptionsLine>();

    //[HideInInspector]
    public OnEndDialogueEvent OnEndActions = null;

    [SerializeField]
    private bool endEvent = false;

    private string[] mainTags;

    private string[] eventsTag;

    private string[] optionsTag;

    private string[] allFileLines;

    private int currentLine;

    private string currentAnswersId;

    public enum DialogueMode { NORMAL, WRITING, ANSWERSOPTIONS, ENDED };

    public enum EventType { IMAGE,SOUND,ANIMATION}

    private DialogueMode dialogueState = DialogueMode.NORMAL;

    //Events
    public Action<string, string> OnNewDialogueSetence;

    public Action<string> OnActiveAnswers;

    public Action<int> OnSaveEvent;

    public Action<string> OnUnlockEvent;

    public Action<int> OnImageEvent;

    public Action<int> OnSoundEvent;

    public Action<int> OnAnimationEvent;

    public Action OnSkipWriting;

    public Action OnEnd;

    public Action TempEnd=null;

    private InputController inputController;

    

    private void Awake()
    {
        mainTags = AllTags.mainTags;
        optionsTag = AllTags.optionsTag;
        eventsTag = AllTags.eventsTag;

        inputController = FindObjectOfType<InputController>();

        inputController.OnDialogueSkipEvent += InputController_NextLine;

        //SplitFile();

        //AssociateAnswers();
        if (dialogue != null)
        {
            dialogueText = dialogue.GetFileLenguageTexts().Find(x => x.languageID.Contains(gameLanguage)).textLines;

            currentLine = dialogueText.FindIndex(x => x.Contains(mainTags[0]));

            print(currentLine);
        }
    }

    private void Start()
    {
        if (dialogue != null)
            NextLine();
    }

    private void InputController_NextLine()
    {
        if (active)
        {
            NextLine();
        }
    }

    public void NextLine()
    {
        if (dialogueState == DialogueMode.WRITING)
        {
            OnSkipWriting?.Invoke();
            return;
        }
        if (dialogueState == DialogueMode.NORMAL)
        {
            currentLine++;
            string lineText = dialogueText[currentLine];

            if (lineText.Contains(mainTags[3]))//N
            {
                string name = lineText.Substring(mainTags[3].Length);
                print(name);
                currentLine++;

                string setence = dialogueText[currentLine].Substring(mainTags[4].Length);
                print(setence);
                OnNewDialogueSetence?.Invoke(name, setence);

                dialogueState = DialogueMode.WRITING;
            }
            else if (lineText.Contains(mainTags[5]))//answers
            {
                dialogueState = DialogueMode.ANSWERSOPTIONS;

                string answerId = lineText.Substring(mainTags[5].Length);

                currentAnswersId = answerId;

                print(dialogueText[currentLine + 1].Substring(mainTags[6].Length));

                OnActiveAnswers?.Invoke(dialogueText[currentLine + 1].Substring(mainTags[6].Length));
               
            }     
            else if (lineText.Contains(eventsTag[0]))//IMAGEEVENT
            {
                string valueText = lineText.Substring(eventsTag[0].Length);

                CallEvent(valueText, EventType.IMAGE);              
            }
            else if (lineText.Contains(eventsTag[1]))//SOUNDEVENT
            {
                string valueText = lineText.Substring(eventsTag[1].Length);

                CallEvent(valueText, EventType.SOUND);
            }
            else if (lineText.Contains(eventsTag[2]))//ANIMATIONEVENT
            {
                string valueText = lineText.Substring(eventsTag[2].Length);

                CallEvent(valueText, EventType.ANIMATION);
            }
            else if (lineText.Contains(mainTags[1]) || lineText.Contains(mainTags[2]))//ENDFILE || END
            {
                dialogueState = DialogueMode.ENDED;

                OnEnd?.Invoke();

                if (endEvent)
                {
                    OnEndActions?.Invoke();

                    endEvent = false;
                }

                if (TempEnd != null)
                {
                    TempEnd?.Invoke();

                    TempEnd = null;
                }

                if (disableOnEnd)
                    active = false;

                print("ENDED");
            }
        }

    }

    public void SelectedOption(int number)
    {
        AnswersOptionsLine temp = null;

        if(answersGroup.TryGetValue(currentAnswersId, out temp))
        {
            if (number == 1)
                currentLine = temp.opt1Line;
            else if (number == 2)
                currentLine = temp.opt2Line;
            else if (number == 3)
                currentLine = temp.opt3Line;
            else if (number == 4)
                currentLine = temp.opt4Line;

            dialogueState = DialogueMode.NORMAL;

            NextLine();
        }
        else
        {
            Debug.LogError("Esse id da resposta não existe: "+ currentAnswersId);
        }
    }

    private void CallEvent(string value, EventType eventType)
    {
        int toIntValue;

        if (int.TryParse(value, out toIntValue))
        {
            if(eventType == EventType.IMAGE)
            {
                OnImageEvent?.Invoke(toIntValue);
            }
            else if(eventType == EventType.SOUND)
            {
                OnSoundEvent?.Invoke(toIntValue);
            }
            else if(eventType == EventType.ANIMATION)
            {
                OnAnimationEvent?.Invoke(toIntValue);
            }
           
            NextLine();
        }
        else
        {
            Debug.LogError("Não foi possivel converter o id do evento (" + value + ")"+"("+eventType+")");
        }
    }

    public void SetCurrentLine(int value)
    {
        currentLine = value;
    }

    public void FinishedWriting()
    {
        dialogueState = DialogueMode.NORMAL;
    }

    public void SetNewDialogue(DialogueAsset dialogueAsset)
    {
        dialogue = dialogueAsset;

        dialogueText = dialogue.GetFileLenguageTexts().Find(x => x.languageID.Contains(gameLanguage)).textLines;

        currentLine = dialogueText.FindIndex(x => x.Contains(mainTags[0]));

        endEvent = false;

        dialogueState = DialogueMode.NORMAL;

        active = true;

        NextLine();
    }

    public void SetNewDialogue(DialogueAsset dialogueAsset, OnEndDialogueEvent onEnd)
    {

        dialogue = dialogueAsset;

        dialogueText = dialogue.GetFileLenguageTexts().Find(x => x.languageID.Contains(gameLanguage)).textLines;

        currentLine = dialogueText.FindIndex(x => x.Contains(mainTags[0]));
        print(currentLine);


        if (onEnd != null && onEnd.GetPersistentEventCount()>0)
        {
            endEvent = true;
            OnEndActions = onEnd;
        }
        else
        {
            endEvent = false;
            OnEndActions = null;
        }

        dialogueState = DialogueMode.NORMAL;

        active = true;

        NextLine();

    }

    public void SetNewDialogue(DialogueAsset dialogueAsset, Action onEnd)
    {

        dialogue = dialogueAsset;

        dialogueText = dialogue.GetFileLenguageTexts().Find(x => x.languageID.Contains(gameLanguage)).textLines;

        currentLine = dialogueText.FindIndex(x => x.Contains(mainTags[0]));
        print(currentLine);


        if (onEnd != null && onEnd.GetInvocationList().Length > 0)
        {
            TempEnd = onEnd;
        }
        else
        {
            TempEnd = null;
        }

        dialogueState = DialogueMode.NORMAL;

        active = true;

        NextLine();

    }

    #region OLD
    //private void SplitFile()
    //{
    //    allFileLines = textFile.text.Split('\n');

    //    foreach (string s in allFileLines)
    //    {
    //        bool haveMainTag = false;
    //        foreach (string tags1 in mainTags)
    //        {
    //            if (s.Contains(tags1))
    //            {
    //                allValidyFileLines.Add(s);
    //                haveMainTag = true;
    //                break;
    //            }        
    //        }

    //        if (haveMainTag == false)
    //        {
    //            foreach (string tags3 in eventsTag)
    //            {
    //                if (s.Contains(tags3))
    //                {
    //                    allValidyFileLines.Add(s);
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //}

    private void AssociateAnswers()
    {
        for (int i = 0; i < dialogueText.Count; i++)
        {
            if (dialogueText[i].Contains(mainTags[4]))
            {
                AnswersOptionsLine answerAssocied = new AnswersOptionsLine();
                string id = dialogueText[i].Substring(mainTags[4].Length);

                for (int y = i; y < dialogueText.Count; y++)
                {
                    foreach (string s in optionsTag)
                    {
                        if (dialogueText[y].Contains(s))
                        {
                            string optionId = dialogueText[y].Substring(s.Length);

                            if (optionId == id)
                            {
                                if (s == AllTags.optionsTag[0])
                                {
                                    answerAssocied.opt1Line = y;
                                }
                                else if (s == AllTags.optionsTag[1])
                                {
                                    answerAssocied.opt2Line = y;
                                }
                                else if (s == AllTags.optionsTag[2])
                                {
                                    answerAssocied.opt3Line = y;
                                }
                                else if (s == AllTags.optionsTag[3])
                                {
                                    answerAssocied.opt4Line = y;
                                }
                            }
                        }
                    }
                }

                answerAssocied.idLegth = id.Length;

                answersGroup.Add(id, answerAssocied);

            }
        }

    }
    #endregion
}

public class AnswersOptionsLine
{
    public int idLegth;
    //public int buttonOptTextLine;
    public int opt1Line;
    public int opt2Line;
    public int opt3Line;
    public int opt4Line;
}

