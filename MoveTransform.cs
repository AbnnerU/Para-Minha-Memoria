
using UnityEngine;

public class MoveTransform : MonoBehaviour
{
    [SerializeField] private Transform _transform;

    [SerializeField] private Vector3 position;

    public void SetPosition()
    {
        _transform.position = position;
    }

}
