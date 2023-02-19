using System.Collections;
using UnityEngine;

public class MoveObjectAction : InteractionAction
{
    [SerializeField] private Transform targetTransform;

    [SerializeField] private Vector3 moveTo;

    [SerializeField] private bool waitOneFrame = false;

    public override void ExecuteAction()
    {
        if(waitOneFrame==false)
            targetTransform.position = moveTo;
        else
            StartCoroutine(DelayAction());
    }

    IEnumerator DelayAction()
    {
        yield return null;

        targetTransform.position = moveTo;
    }
}
