
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAction : InteractionAction
{
    [SerializeField] private Transition transition;

    [SerializeField] private int transitionStartID;

    [SerializeField] private int transitionEndID;

    [Header("Additional Actions")]
    [SerializeField] private bool disableMovementsDuringTransition = true;
    [SerializeField] private bool disablePointAndClickDuringTransition = true;

    private PointAndClick pointAndClick;
    private PointAndClickInventoryVisual pointAndClickInventoryVisual;

    private List<IMovementGeneral> allMovementScripts = new List<IMovementGeneral>();

    private void Awake()
    {
        if (disablePointAndClickDuringTransition)
        {
            pointAndClick = FindObjectOfType<PointAndClick>();
            pointAndClickInventoryVisual = FindObjectOfType<PointAndClickInventoryVisual>();
        }

        if (disableMovementsDuringTransition)
        {
            var reference = FindObjectsOfType<MonoBehaviour>().OfType<IMovementGeneral>();

            foreach (IMovementGeneral mg in reference)
            {
                allMovementScripts.Add(mg);
            }
        }

    }

    public override void ExecuteAction()
    {
        transition.ExecuteTransition(transitionStartID, () => ExecuteTransitionEnd());

        print("Start");

        if (disablePointAndClickDuringTransition)
        {
            if(pointAndClick != null)
                pointAndClick.SetDetectionActive(false);

            if (pointAndClickInventoryVisual)
                pointAndClickInventoryVisual.SetDetectionActive(false);
        }

        if (disableMovementsDuringTransition)
        {
            foreach (IMovementGeneral mg in allMovementScripts)
            {
                mg.Disable();
            }
        }
    }

    public void ExecuteTransitionEnd()
    {
        transition.ExecuteTransition(transitionEndID, ()=>EnableAll());

        print("end");
    }


    public void EnableAll()
    {
        print("Enable All");

        if (disablePointAndClickDuringTransition)
        {
            if (pointAndClick != null)
                pointAndClick.SetDetectionActive(true);

            if (pointAndClickInventoryVisual)
                pointAndClickInventoryVisual.SetDetectionActive(true);
        }

        if (disableMovementsDuringTransition)
        {
            foreach (IMovementGeneral mg in allMovementScripts)
            {
                mg?.Enable();
            }
        }
    }
}
