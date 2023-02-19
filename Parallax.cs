using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxSpeed;
    private Transform cam;
    private float startPos;

    private Transform _transform;

    private float axisY;
    private float axisZ;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;

        startPos = _transform.position.x;

        cam = Camera.main.transform;

        axisY = _transform.position.y;
        axisZ = _transform.position.z;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float distance = cam.position.x * parallaxSpeed;

        _transform.position = new Vector3(startPos + distance, axisY, axisZ);
    }
}
