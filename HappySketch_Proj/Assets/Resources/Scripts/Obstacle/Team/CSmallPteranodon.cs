using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Jaehoon;
using UnityEngine;

namespace MyeongJin
{
	public class CSmallPteranodon : CCreatureHerd
	{
		private Transform[] controlPoints;  // ������ (�ּ� 4�� �ʿ�)

		private float moveSpeed = 10f;       // �̵� �ӵ�
		private float t = 0f;               // Catmull-Rom ��� �ð� ����
		private int currentSegment = 0;     // ���� �̵� ���� � ����
											// <<
		private int hitCount = 1;

		private bool isSoundActivated = false;

        private void Awake()
        {
            controlPoints = GameObject.Find("SmallPteranodonControlPoints").GetComponent<CSkyControlPoint>().controlPoints;
            SetStartPosition();
        }
		private void Update()
		{
			StoopAndClimb();
            if (IsStateChanged())
                ReturnToPool();
        }
		private void OnEnable()
        {
            this.transform.Rotate(45, 0, 0);
            isSoundActivated = false;
        }
		private void OnDisable()
		{
			ResetObstacle();
		}
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "PlayerCollider" || other.tag == "PlayerCollider")
            {
				this.GetComponent<BoxCollider>().enabled = false;
				hitCount--;
            }
        }
        public void ResetObstacle()
        {
            SetStartPosition();
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
			this.GetComponent<BoxCollider>().enabled = true;

            hitCount = 1;
        }
		private void SetStartPosition()
		{
			if (controlPoints != null)
			{
				this.transform.position = new Vector3(transform.position.x, controlPoints[1].position.y, controlPoints[1].position.z);
			}
		}
		private void StoopAndClimb()
		{
			if (controlPoints.Length < 4) return;

			Transform p0 = controlPoints[currentSegment];
			Transform p1 = controlPoints[currentSegment + 1];
			Transform p2 = controlPoints[currentSegment + 2];
			Transform p3 = controlPoints[currentSegment + 3];

			// Catmull-Rom � ���� ���
			Vector3 newPosition = CatmullRom(p0.position, p1.position, p2.position, p3.position, t);
			newPosition.x = transform.position.x;
			transform.position = newPosition;

			// ��� t ���� ���������� ������Ʈ (speed�� � �̵� �ӵ� ����)
			t += Time.deltaTime * moveSpeed / Vector3.Distance(p1.position, p2.position);

			// t�� 1�� �����ϸ� ���� �������� ��ȯ
			if (t >= 1f)
			{
				t = 0f; // � �ð� �ʱ�ȭ
				currentSegment++;

				// ������ ������ ������ ��ũ��Ʈ ����
				if (currentSegment == 1)
				{
					this.GetComponentInChildren<Animator>().SetBool("isTouch", true);
					this.transform.Rotate(-45, 0, 0);

                    if (!isSoundActivated)
                    {
                        isSoundActivated = true;

                        SoundManager.instance.SFXPlay("Sounds/Pterosaur");
                    }
                }

				if (currentSegment >= controlPoints.Length - 3)
				{
					currentSegment = 0;

                    ReturnToPool(hitCount * 10);
				}
			}
		}
		private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			return 0.5f * (
				(2f * p1) +
				(-p0 + p2) * t +
				(2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
				(-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t);
		}
	}
}