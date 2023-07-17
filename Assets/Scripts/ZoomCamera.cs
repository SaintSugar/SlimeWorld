using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
    // Start is called before the first frame update
    new Camera camera;
    Follow follow;
    [SerializeField]
    float maxSize, minSize, scale;
    float startRadius, startSize;
    void Start()
    {
        camera = GetComponent<Camera>();
        follow = GetComponent<Follow>();
        startRadius = follow.Radius;
        startSize = camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float size = camera.orthographicSize;
        size -= Input.mouseScrollDelta.y * scale;
        if (size < minSize || size > maxSize) return;
        camera.orthographicSize = size;
        follow.Radius = size/startSize * startRadius;
    }
}
