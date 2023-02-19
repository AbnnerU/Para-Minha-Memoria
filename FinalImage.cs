using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalImage : MonoBehaviour
{
    [SerializeField] private Transition transition;

    [SerializeField] private GameObject imageGameObject;

    [SerializeField] private int transitionStartID;

    [SerializeField] private int transitionEndID;

    public void ExecuteFinalImage()
    {
        transition.ExecuteTransition(transitionStartID,()=>ExecuteTransitionEnd());
    }

    private void ExecuteTransitionEnd()
    {
        imageGameObject.SetActive(true);

        transition.ExecuteTransition(transitionEndID);
    }
}
