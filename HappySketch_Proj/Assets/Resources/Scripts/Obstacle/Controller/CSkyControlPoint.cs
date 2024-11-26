using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyeongJin
{
    public class CSkyControlPoint : MonoBehaviour
    {
        public Transform[] controlPoints;

        private void Start()
        {
            SortWithName();
        }
        private void SortWithName()
        {
            for (int i = 0; i < controlPoints.Length; i++)
            {
                for (int j = i + 1; j < controlPoints.Length; j++)
                {
                    if (string.Compare(controlPoints[i].gameObject.name, controlPoints[j].gameObject.name) == 1)
                    {
                        Transform temp = controlPoints[i];
                        controlPoints[i] = controlPoints[j];
                        controlPoints[j] = temp;
                    }
                }
            }
        }
    }
}