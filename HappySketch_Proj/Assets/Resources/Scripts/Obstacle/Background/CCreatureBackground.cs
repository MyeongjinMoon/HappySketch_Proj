using MyeongJin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyeongJin
{
    public class CCreatureBackground : CCreatureHerd
    {
        private readonly float moveSpeed = 12f;

        private void Update()
        {
            Gliding();
        }
        private void OnDisable()
        {
            ResetObstacle();
        }
        public void ResetObstacle()
        {
            SetStartPosition();
        }
        private void SetStartPosition()
        {
            this.transform.position = new Vector3(transform.position.x, 15, -10);
        }
        private void Gliding()
        {
            transform.Translate(-transform.forward * moveSpeed * Time.deltaTime);
            if (transform.position.z < -10)
                ReturnToPool();
        }
    }
}