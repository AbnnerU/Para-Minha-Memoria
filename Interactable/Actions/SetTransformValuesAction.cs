
using UnityEngine;

public class SetTransformValuesAction : InteractionAction
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 scale = Vector3.one;

    public override void ExecuteAction()
    {
        if (_transform == null)
            return;

        _transform.position = position;
        _transform.localScale = scale;
    }
}
