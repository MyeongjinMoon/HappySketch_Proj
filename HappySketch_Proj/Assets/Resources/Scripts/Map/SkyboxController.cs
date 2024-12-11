using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkyboxManager : MonoBehaviour
{
    [SerializeField] Material skybox1;
    [SerializeField] Material skybox2;
    [SerializeField] Material skybox3;
    [SerializeField] GameObject Lamp;

    [SerializeField] GameObject directionalLight;
    int hours;

    void Start()
    {
        hours = DateTime.Now.Hour;
        StartCoroutine("HourCheck");
    }
}
