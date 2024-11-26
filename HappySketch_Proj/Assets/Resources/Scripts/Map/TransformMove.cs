using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JaeHoon
{
    public class TransformMove : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 0.0f;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            float moveDelta = moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * moveDelta);
        }
    }
}
