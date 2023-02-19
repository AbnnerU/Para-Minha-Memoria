using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridContent : MonoBehaviour
{
   
    protected Transform _transform;

    protected virtual void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    public Transform GetTransform()
    {
        return _transform;
    }
}
