using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Transform objectReference;

    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    public void SetObjectAtSpawnPoint()
    {
        if(_transform)
            objectReference.position = _transform.position;
        else
            objectReference.position = transform.position;
    }

 

    public void SetNewObjectParent(Transform newParent)
    {
        objectReference.SetParent(newParent, true);
    }

    public void SetNewParent(Transform newParent)
    {
        _transform.SetParent(newParent);
    }

    public void SetNewPosition(Vector3 newPosition)
    {
        _transform.position = newPosition;
    }

    public void SetNewPosition(Transform reference)
    {
        _transform.position = reference.position;
    }

    public Transform GetSpawnPointTransform()
    {
        if (_transform)
            return _transform;
        else
            return transform;
    }
}
