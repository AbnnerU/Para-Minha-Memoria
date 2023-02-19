using System.Collections;
using UnityEngine;

public class EnableObjectAction : InteractionAction
{
    [SerializeField] private bool setActive;

    [SerializeField] private GameObject[] objectsToDisable;

    [SerializeField] private bool waitOneFrame = false;

    public override void ExecuteAction()
    {
        if (waitOneFrame == false)
        {
            for (int i = 0; i < objectsToDisable.Length; i++)
            {
                objectsToDisable[i].SetActive(setActive);
            }
        }
        else
        {
            StartCoroutine(DelayAction());
        }
    }

    IEnumerator DelayAction()
    {
        yield return null;

        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            objectsToDisable[i].SetActive(setActive);
        }
    }
}
