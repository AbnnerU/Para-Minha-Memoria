using System.Collections;
using UnityEngine;

public class DisableRendererAction : InteractionAction
{
    [SerializeField] private bool setActive;

    [SerializeField] private Renderer[] renderers;

    [SerializeField] private bool waitOneFrame = false;

    public override void ExecuteAction()
    {
        if (waitOneFrame == false)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].enabled = setActive;
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

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = setActive;
        }
    }
}
