using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConfig : MonoBehaviour
{

    [Header("Camera")]
    [SerializeField] private Camera cameraReference;
    [SerializeField] private float cameraSize;

    [Header("Camera Follow")]
    [Space(10)]
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private bool enableFollow;


    [Header("Camera position")]
    [Space(10)]
    [SerializeField] private Vector3 position;

    public void SetConfigs()
    {
        cameraReference.orthographicSize = cameraSize;

        if (cameraFollow)
        {
            cameraFollow.enabled = enableFollow;

            if (enableFollow == false)
                cameraReference.transform.position = position;
        }
        else
        {
            cameraReference.transform.position = position;
        }
    }

    public void SetCameraFollowConfig()
    {
        if (cameraFollow)
        {
            cameraFollow.enabled = enableFollow;      
        }  
    }

    public Vector3 GetConfigPosition()
    {
        return position;
    }

    public float GetCameraSizeConfig()
    {
        return cameraSize;
    }

    public Camera GetCamera()
    {
        return cameraReference;
    }

    public void SetConfigs(float newCameraSize, Vector3 newCameraPosition)
    {
        cameraReference.orthographicSize = newCameraSize;

        if (cameraFollow)
        {
            cameraFollow.enabled = enableFollow;

            if (enableFollow == false)
                cameraReference.transform.position = newCameraPosition;
        }
        else
        {
            cameraReference.transform.position = position;
        }
    }
}
