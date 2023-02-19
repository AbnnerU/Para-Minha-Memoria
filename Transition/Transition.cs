using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Transition : MonoBehaviour
{
    [SerializeField] private Volume postProcessingVolume;

    [Header("Defalt")]
    [SerializeField]private Vector2 startValue;

    [Header("Transitions")]
    [SerializeField] private List<TransitionConfig> transitionConfigs = new List<TransitionConfig>();

    private Vignette vignetteEffect;

    private void Awake()
    {
        postProcessingVolume.profile.TryGet<Vignette>(out vignetteEffect);

        vignetteEffect.center.value = startValue;
    }

    public void ExecuteTransition(int transitionID)
    {
        int index = transitionConfigs.FindIndex(x => x.id == transitionID);

        if (index < 0)
        {
            Debug.LogWarning(transitionID + "none transition whit this id");
            return;
        }

        StartCoroutine(TransitionAnimation(index));

    }

    public void ExecuteTransition(int transitionID, Action onEndAction)
    {
        int index = transitionConfigs.FindIndex(x => x.id == transitionID);

        if (index < 0)
        {
            Debug.LogWarning(transitionID + "none transition whit this id");
            return;
        }

        StartCoroutine(TransitionAnimation(index,onEndAction));

    }

    IEnumerator TransitionAnimation(int id)
    {
        TransitionConfig config = transitionConfigs[id];
        vignetteEffect.center.value = config.startCenter;

        Vector2 startValue = config.startCenter;
        Vector2 difference = config.finalCenter - config.startCenter;

        float durantion = config.durantion;
        float currentTime=0;
        float percentage = 0;

        do
        {
            if (durantion != 0)
            {
                currentTime += Time.deltaTime;
                percentage = ((currentTime * 100) / durantion) / 100;

                vignetteEffect.center.value = startValue + (difference * percentage);

                yield return null;
            }
            else
            {
                break;
            }

        } while (currentTime < durantion);

        vignetteEffect.center.value = config.finalCenter;

        //yield break;
    }


    IEnumerator TransitionAnimation(int id,Action onEndAction)
    {
        TransitionConfig config = transitionConfigs[id];
        vignetteEffect.center.value = config.startCenter;

        Vector2 startValue = config.startCenter;
        Vector2 difference = config.finalCenter - config.startCenter;

        float durantion = config.durantion;
        float currentTime = 0;
        float percentage = 0;

        do
        {
            if (durantion != 0)
            {
                currentTime += Time.deltaTime;
                percentage = ((currentTime * 100) / durantion) / 100;

                vignetteEffect.center.value = startValue + (difference * percentage);

                yield return null;
            }
            else
            {
                break;
            }

        } while (currentTime < durantion);

        onEndAction?.Invoke();

        vignetteEffect.center.value = config.finalCenter;

        ////yield break;
    }
}


[System.Serializable]
public struct TransitionConfig
{
    public int id;
    public float durantion;
    public Vector2 startCenter;
    public Vector2 finalCenter;    
}