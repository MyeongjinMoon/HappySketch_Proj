﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Confetti2
{
    public class Confetti2Cycler : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> listOfEffects;

        [Header("Loop length in seconds")]
        [SerializeField]
        float loopTimeLength = 5f;

        float timeOfLastInstantiate;

        GameObject instantiatedEffect;

        int effectIndex = 0;

        void Start()
        {
            instantiatedEffect = Instantiate(listOfEffects[effectIndex], transform.position, transform.rotation) as GameObject;
            effectIndex++;
            timeOfLastInstantiate = Time.time;
        }

        void Update()
        {
            if (Time.time >= timeOfLastInstantiate + loopTimeLength)
            {
                Destroy(instantiatedEffect);
                instantiatedEffect = Instantiate(listOfEffects[effectIndex], transform.position, transform.rotation) as GameObject;
                timeOfLastInstantiate = Time.time;
                if (effectIndex < listOfEffects.Count - 1)
                    effectIndex++;
                else
                    effectIndex = 0;
            }
        }
    }
}
