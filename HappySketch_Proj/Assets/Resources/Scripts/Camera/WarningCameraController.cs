using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningCameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas canvas;
  
    // Update is called once per frame
    void Update()
    {
        Camera mainCamera = Camera.main;
        canvas.worldCamera = mainCamera;
    }
}
