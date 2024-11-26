using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkyboxManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Material skybox1;
    [SerializeField] Material skybox2;
    [SerializeField] Material skybox3;
    [SerializeField] GameObject Lamp;

    [SerializeField] GameObject directionalLight;
    int hours;      // ½Ã°£

    void Start()
    {
        hours = DateTime.Now.Hour;
        StartCoroutine("HourCheck");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
