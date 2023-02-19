using System;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{

    [SerializeField] private bool canRecord = false;

    [SerializeField] private float minDisplacementToSave;

    [SerializeField] private Transform alwaysBackTo;    

    [SerializeField]private float maxPoints = 5f;

    [SerializeField] private Volume postProcessing;

    [SerializeField] private RewindSpeed[] rewindPointsPerFrame;

    [SerializeField] List<PointInTime> pointsInTime;


    [Header("Visual")]
    [Space(8)]
    [SerializeField] private bool useVisual;
    [SerializeField] private float distorcionEndValue;
    [SerializeField] private float distorcionInTime;
    [SerializeField] private Renderer render;
    [SerializeField] private Material material;
    
    public Action OnEnd;

    private Vector3 lastPosition;

    private Transform _transform;

    private Rigidbody rb;

    private int rewindSpeedValue = -1;

    bool isRewinding = false;
    
    void Start()
    {
        if(postProcessing)
            postProcessing.enabled = false;
        if(render)
            render.enabled = false;

        pointsInTime = new List<PointInTime>();
        rb = GetComponent<Rigidbody>();

        _transform = GetComponent<Transform>();

    }

    void Update()
    {
       if(isRewinding)
            Rewind();

    }

    void FixedUpdate()
    {      
       if(isRewinding==false)
            Record();
    }

    void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            if (rewindSpeedValue < 0)
            {
                rewindSpeedValue = ChooseRewindSpeed();
            }

            for (int i = 0; i < rewindSpeedValue; i++)
            {
                if (pointsInTime.Count > 0)
                {
                    PointInTime pointInTime = pointsInTime[0];
                    transform.position = pointInTime.position;
                    transform.rotation = pointInTime.rotation;
                    pointsInTime.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }           
        }
        else
        {
            StopRewind();
        }

    }

    void Record()
    {
        if (canRecord == false)
            return;

        if ((_transform.position - lastPosition).magnitude >= minDisplacementToSave)
        {
            if (pointsInTime.Count > maxPoints)
            {
                pointsInTime.RemoveAt(pointsInTime.Count - 1);
            }
            pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));         
        }

        if(pointsInTime.Count>0)
        lastPosition = pointsInTime[0].position;
    }


    public void StartRewind()
    {
        //visual
        if (useVisual)
        {
            postProcessing.enabled = true;
            render.enabled = true;
        }

        isRewinding = true;
        //rb.isKinematic = true;
    }

    public void StopRewind()
    {
        //visual
        if (useVisual)
        {
            postProcessing.enabled = false;
            render.enabled = false;
        }
        rewindSpeedValue = -1;
        
        transform.position = alwaysBackTo.position;

        isRewinding = false;

        OnEnd?.Invoke();

    }

  

    public int ChooseRewindSpeed()
    {
        int value = 1;

        for(int i =0;i < rewindPointsPerFrame.Length; i++)
        {
            if (rewindPointsPerFrame[i].pointsAmount <= pointsInTime.Count)
                value = rewindPointsPerFrame[i].rewindValue;
            else
                break;
        }

        return value;
    }

    public void ResetAll()
    {
        
        pointsInTime.Clear();
    }

    public void CanRecord(bool can)
    {
        canRecord = can;
    }
}

[Serializable]
public struct RewindSpeed
{
    public int pointsAmount;
    public int rewindValue;
}

[Serializable]
public class PointInTime
{
    public Vector3 position;
    public Quaternion rotation;

    public PointInTime(Vector3 _position, Quaternion _rotation)
    {
        position = _position;
        rotation = _rotation;
    }

}
