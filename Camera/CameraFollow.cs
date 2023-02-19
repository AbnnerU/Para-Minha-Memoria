using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followTime;

    [Header("Limits")]
    [Space(8)]

    [SerializeField] private bool drawGizmos;
    [SerializeField] private bool useLimits;

    [SerializeField] private float rightLimit;

    [SerializeField] private float leftLimit;

    [SerializeField] private float topLimit;

    [SerializeField] private float bottomLimit;

    private Vector3 velocity = Vector3.zero;

    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        Vector3 movementValue = Vector3.SmoothDamp(_transform.position, new Vector3(target.position.x, target.position.y, -10), ref velocity, followTime);

        if (useLimits)
            movementValue = CalculateLimits(movementValue);

        _transform.position = movementValue;


    }

    private Vector3 CalculateLimits(Vector3 reference)
    {
        Vector3 finalValue = reference;

        if (reference.x < leftLimit)
            finalValue.x = leftLimit;
        else if (reference.x > rightLimit)
            finalValue.x = rightLimit;

        if (reference.y < bottomLimit)
            finalValue.y = bottomLimit;
        else if (reference.y > topLimit)
            finalValue.y = topLimit;

        return finalValue;
    }


    public void ChangeFollowTime(float value)
    {
        followTime = value;
    }


    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(new Vector3(leftLimit, bottomLimit), new Vector3(leftLimit, topLimit));

            Gizmos.DrawLine(new Vector3(rightLimit, bottomLimit), new Vector3(rightLimit, topLimit));

            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(leftLimit, bottomLimit), new Vector3(rightLimit, bottomLimit));
            Gizmos.DrawSphere(new Vector3(leftLimit, bottomLimit), 0.1f);
            

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(leftLimit, topLimit), new Vector3(rightLimit, topLimit));
            Gizmos.DrawSphere(new Vector3(rightLimit, topLimit), 0.1f);

           

        }
    }

}
