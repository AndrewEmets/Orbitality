using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    
    private Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        var delta = Input.mouseScrollDelta.y;
        delta += scrollSpeed * Input.GetAxis("Vertical");
        
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize + delta, 15, 65);
    }
}
