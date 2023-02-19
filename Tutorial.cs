using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private bool active;

    [SerializeField] private GameObject tutorialPanel;

    private PointAndClick pointAndClick;
    private PointAndClickInventoryVisual pointAndClickInventoryVisual;

    private List<IMovementGeneral> allMovementScripts = new List<IMovementGeneral>();

    private AnimationManager animationManager;

    private void Awake()
    {
        pointAndClick = FindObjectOfType<PointAndClick>();
        pointAndClickInventoryVisual = FindObjectOfType<PointAndClickInventoryVisual>();

        var reference = FindObjectsOfType<MonoBehaviour>().OfType<IMovementGeneral>();

        foreach (IMovementGeneral mg in reference)
        {
            allMovementScripts.Add(mg);
        }

        animationManager = FindObjectOfType<AnimationManager>();
    }

    private void Start()
    {
        if (active)
            StartTutorial();
    }

    public void StartTutorial()
    {
        if (pointAndClick != null)
            pointAndClick.SetDetectionActive(false);

        if (pointAndClickInventoryVisual)
            pointAndClickInventoryVisual.SetDetectionActive(false);

        if (animationManager)
            animationManager.SetActive(false);

        if (allMovementScripts.Count > 0)
        {
            foreach (IMovementGeneral mg in allMovementScripts)
            {
                mg.Disable();
            }
        }

        tutorialPanel.SetActive(true);
    }

    public void StopTutorial()
    {
        if (pointAndClick != null)
            pointAndClick.SetDetectionActive(true);

        if (pointAndClickInventoryVisual)
            pointAndClickInventoryVisual.SetDetectionActive(true);

        if (animationManager)
            animationManager.SetActive(true);

        if (allMovementScripts.Count > 0)
        {
            foreach (IMovementGeneral mg in allMovementScripts)
            {
                mg.Enable();
            }
        }

        tutorialPanel.SetActive(false);
    }

}
