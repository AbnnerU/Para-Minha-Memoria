using System.Collections;
using UnityEngine;

public class TutorialInput : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private float maxTime = 4;

    private bool tutorialStarted = false;

    private void Awake()
    {
        if (inputController == null)
            inputController = FindObjectOfType<InputController>();

        inputController.OnChangeMode += InputController_OnChangeMode;
    }

    public void ShowPanel()
    {
        tutorialPanel.SetActive(true);
        tutorialStarted = true;
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        float currentTime = 0;

        do
        {
            currentTime += Time.deltaTime;
            yield return null;

        } while (currentTime<maxTime && tutorialStarted);

        tutorialPanel.SetActive(false);
        tutorialStarted = false;

        yield break;
    }

    private void InputController_OnChangeMode()
    {
        if (tutorialStarted == false)
            return;

        tutorialPanel.SetActive(false);
        tutorialStarted = false;
    }
}
