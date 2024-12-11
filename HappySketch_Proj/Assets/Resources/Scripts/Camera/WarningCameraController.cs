using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningCameraController : MonoBehaviour
{
    public Canvas canvas;
  
    void Update()
    {
        Camera mainCamera = Camera.main;
        canvas.worldCamera = mainCamera;
    }
}
