using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyeongJin
{
	public class CSwatter : MonoBehaviour
	{
		private bool isGenerate = false;
		private int direction;
		private int moveDistance = 0;

		private void FixedUpdate()
		{
			if(isGenerate)
			{
				Move();
                if(moveDistance == 10)
                {
                    Destroy(this.gameObject);
                }
            }
		}
        private void OnTriggerEnter(Collider other)
        {
			Destroy(this.gameObject);
        }
        private void Move()
        {
            this.transform.position += new Vector3(direction * Time.deltaTime * 10, 0, 0);
            moveDistance++;
        }
        public void Init(int playerIndex, int vertical)
		{
			if(playerIndex == 0)
            {
				transform.position -= new Vector3(2, 0, 0);
            }
			else
            {
                transform.position += new Vector3(2, 0, 0);
            }
			if(Convert.ToBoolean(vertical))
			{
				direction = 1;
            }
			else
			{
                direction = -1;
            }
            isGenerate = true;
        }
    }
}