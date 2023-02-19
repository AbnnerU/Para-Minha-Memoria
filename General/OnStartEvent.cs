using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnStartCustomEvent : UnityEvent { }

public class OnStartEvent : MonoBehaviour
{
    [SerializeField] private OnStartCustomEvent onStartEvent;

    [SerializeField] private float delay;

    private void Start()
    {
        if (delay == 0)
            onStartEvent?.Invoke();
        else
            StartCoroutine(EventDelay());
    }

    IEnumerator EventDelay()
    {
        yield return new WaitForSeconds(delay);
        onStartEvent?.Invoke();
    }
}
