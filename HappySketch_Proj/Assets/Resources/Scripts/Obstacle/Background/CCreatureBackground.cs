using MyeongJin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyeongJin
{
    public class CCreatureBackground : CCreatureHerd
    {
        private Vector3 startPosition;

        private float moveSpeed = 12f;       // 이동 속도

        private void Update()
        {
            Gliding();
        }
        private void OnDisable()
        {
            ResetObstacle();
        }
        public new void ResetObstacle()
        {
            SetStartPosition();
        }
        private void SetStartPosition()
        {
                startPosition = this.transform.position;
                startPosition.y = 15;
                startPosition.z = -10;
                this.transform.position = startPosition;
        }
        private void Gliding()
        {
            transform.Translate(-transform.forward * moveSpeed * Time.deltaTime);
            if (transform.position.z < -10)
                ReturnToPool();
        }
    }
}