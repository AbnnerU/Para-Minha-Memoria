using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeAnimator : MonoBehaviour
{
    [SerializeField] private bool displacementActive;
    [SerializeField] private float maxLeft;
    [SerializeField] private float maxRight;
    [SerializeField] private float maxUp;
    [SerializeField] private float maxDown;
    [SerializeField] private float maxDisplacement;

    [Space(8)]
    [Header("Animations")]
    [SerializeField] private List<SmokeAnimation> smokeAnimations;
    private Material material;

    private float displacementX;
    private float displacementY;

    private void Awake()
    {
        material = gameObject.GetComponent<Renderer>().material;

        displacementX = NewDisplacementValue();
        displacementY = NewDisplacementValue();
    }

    private void Update()
    {
        if (displacementActive == false)
            return;

        Vector2 currentOffset = material.mainTextureOffset;

        currentOffset.x += displacementX *Time.deltaTime;

        if(currentOffset.x  < maxLeft)
        {
            displacementX = NewDisplacementValue();

            currentOffset.x = maxLeft;
        }
        else if(currentOffset.x  > maxRight)
        {
            displacementX = NewDisplacementValue();

            currentOffset.x = maxRight;
        }

        currentOffset.y += displacementY*Time.deltaTime;

        if(currentOffset.y < maxDown)
        {
            displacementY = NewDisplacementValue();

            currentOffset.y = maxDown;
        }
        else if(currentOffset.y  > maxUp)
        {
            displacementY = NewDisplacementValue();

            currentOffset.y = maxUp;
        }


        material.mainTextureOffset = currentOffset;
    }

    private float NewDisplacementValue()
    {
        float value = Random.Range(-maxDisplacement, maxDisplacement);

        if (value == 0)
            value += maxDisplacement * Time.deltaTime;

        return value;
    }


    public void ExecuteAnimation(int id)
    {
        int index = smokeAnimations.FindIndex(x => x.id == id);

        if (index < 0)
            return;

        StartCoroutine(AlphaAnimation(index));
    }


    IEnumerator AlphaAnimation(int id)
    {

        SmokeAnimation smokeAnimation = smokeAnimations[id];

        float startValue = smokeAnimation.startAlphaClipValue;
        float difference = smokeAnimation.endAlphaClipValue - smokeAnimation.startAlphaClipValue;

        float durantion = smokeAnimation.duration;
        float currentTime = 0;
        float percentage = 0;

        material.SetFloat("_Cutoff", startValue);

        do
        {
            currentTime += Time.deltaTime;
            percentage = ((currentTime * 100) / durantion) / 100;
            
            material.SetFloat("_Cutoff", startValue + (difference*percentage));
            yield return null;

        } while (currentTime < durantion);


        if (smokeAnimation.displacementEnabled)
            displacementActive = true;
        else
            displacementActive = false;
       
        yield break;
    }
}

[System.Serializable]
public struct SmokeAnimation
{
    public int id;
    public bool displacementEnabled;
    public float startAlphaClipValue;
    public float endAlphaClipValue;
    public float duration;
}
