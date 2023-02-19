using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistorcionEffect : MonoBehaviour
{
    [SerializeField] private Image render;

    [SerializeField] private DistorcionEffectAnimation[] effectAnimation;

    private Material material;


    private void Awake()
    {
        material = render.material;
        
        render.enabled = false;
    }


    public void ExecuteAnimation(int id)
    {
        for(int i=0; i < effectAnimation.Length; i++)
        {
            if (effectAnimation[i].id == id)
            {
                StartCoroutine(EffectAnimation(effectAnimation[i]));
                return;
            }
        }

        print("None animation effect whit id:" + id);
    }

    public void ExecuteAnimation(int id, Action endAction)
    {
        for (int i = 0; i < effectAnimation.Length; i++)
        {
            if (effectAnimation[i].id == id)
            {
                StartCoroutine(EffectAnimation(effectAnimation[i],endAction));
                return;
            }
        }

        print("None animation effect whit id:" + id);
    }

    IEnumerator EffectAnimation(DistorcionEffectAnimation distorcionEffect)
    {
        DistorcionEffecValues startValues = distorcionEffect.startEffect;
        DistorcionEffecValues endValues = distorcionEffect.endEffect;

        float duration = distorcionEffect.duration;

        float distorcionPower = endValues.distorcionPower - startValues.distorcionPower;
        float currentTime = 0;
        float percentage = 0;

        render.enabled = true;

        material.SetVector("_Offset", distorcionEffect.offset);
        material.SetFloat("_DistorcionPower", startValues.distorcionPower);
        material.SetColor("_Color", distorcionEffect.colorTransition.Evaluate(0));

        do
        {
            currentTime += Time.deltaTime;
            percentage = ((currentTime * 100) / duration) / 100;
            print(percentage);
            material.SetFloat("_DistorcionPower", startValues.distorcionPower + (distorcionPower * percentage));
            material.SetColor("_Color", distorcionEffect.colorTransition.Evaluate(percentage));

            yield return null;
        } while (currentTime < duration);

        material.SetFloat("_DistorcionPower", endValues.distorcionPower);
        material.SetColor("_Color", distorcionEffect.colorTransition.Evaluate(1));

        if (distorcionEffect.disableRenderOnEnd)
            render.enabled = false;

        yield break;

    }

    IEnumerator EffectAnimation(DistorcionEffectAnimation distorcionEffect, Action endAction)
    {
        DistorcionEffecValues startValues = distorcionEffect.startEffect;
        DistorcionEffecValues endValues = distorcionEffect.endEffect;

        float duration = distorcionEffect.duration;

        float distorcionPower = endValues.distorcionPower - startValues.distorcionPower;
        float currentTime = 0;
        float percentage = 0;

        render.enabled = true;

        material.SetVector("_Offset", distorcionEffect.offset);
        material.SetFloat("_DistorcionPower", startValues.distorcionPower);
        material.SetColor("_Color", distorcionEffect.colorTransition.Evaluate(0));

        do
        {
            currentTime += Time.deltaTime;
            percentage = ((currentTime * 100) / duration) / 100;
            print(percentage);
            material.SetFloat("_DistorcionPower", startValues.distorcionPower + (distorcionPower * percentage));
            material.SetColor("_Color", distorcionEffect.colorTransition.Evaluate(percentage));

            yield return null;
        } while (currentTime < duration);

        material.SetFloat("_DistorcionPower", endValues.distorcionPower);
        material.SetColor("_Color", distorcionEffect.colorTransition.Evaluate(1));

        if (distorcionEffect.disableRenderOnEnd)
            render.enabled = false;

        endAction?.Invoke();

        yield break;

    }

}

[System.Serializable]
public struct DistorcionEffectAnimation
{
    public int id;
    public float duration;
    public Vector2 offset;
    [Header("Start Values")]
    public DistorcionEffecValues startEffect;
    [Header("End Values")]
    public DistorcionEffecValues endEffect;
    public Gradient colorTransition;
    public bool disableRenderOnEnd;
}
[System.Serializable]
public struct DistorcionEffecValues
{
    public float distorcionPower;
}