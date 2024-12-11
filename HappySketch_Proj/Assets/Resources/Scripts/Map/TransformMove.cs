using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JaeHoon
{
    public class TransformMove : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 0.0f;
        void Update()
        {
            float moveDelta = moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * moveDelta);
        }
    }
}
