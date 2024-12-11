using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
	private float horizontalFoV = 60.0f;
	private Camera _camera;
    void Start()
    {
         _camera = GetComponent<Camera>();
    }

    void Update()
    {
       float halfWidth = Mathf.Tan(0.8562170f * horizontalFoV * Mathf.Deg2Rad);
       float halfHeight = halfWidth * Screen.height / Screen.width;
       float verticalFoV = 2.0f * Mathf.Atan(halfHeight) * Mathf.Rad2Deg;
       _camera.fieldOfView = verticalFoV; 
    }
}
