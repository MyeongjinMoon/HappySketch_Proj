using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyeongJin
{
	public class CBigPteranodon : CCreatureHerd
	{
		private Vector3 startPosition;
		// >>: Stoop
		private Transform[] controlPoints;  // ������ (�ּ� 4�� �ʿ�)

		private float moveSpeed = 20f;       // �̵� �ӵ�
		private float t = 0f;               // Catmull-Rom ��� �ð� ����
		private int currentSegment = 0;     // ���� �̵� ���� � ����
											// <<
		private int rotateSpeed = 15;
		private int hitCount = 2;
		private void Awake()
		{
			controlPoints = GameObject.Find("BigPteranodonControlPoints").GetComponent<CSkyControlPoint>().controlPoints;
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
			this.transform.Rotate(-45, 0, 0);
        }
		private void OnDisable()
		{
			ResetObstacle();
		}
		private void OnTriggerEnter(Collider other)
		{
            if (other.tag == "PlayerCollider" || other.tag == "PlayerCollider")
            {
				Debug.Log("BigPterandon Attack");
                this.GetComponent<BoxCollider>().enabled = false;
                hitCount--;
            }
		}
		public new void ResetObstacle()
		{
			SetStartPosition();
			this.transform.rotation = Quaternion.Euler(0, 0, 0);
            this.GetComponent<BoxCollider>().enabled = true;
            rotateSpeed = 15;
			hitCount = 2;
        }
		private void SetStartPosition()
		{
			if(controlPoints != null)
			{
				startPosition = this.transform.position;
				startPosition.x = controlPoints[1].position.x;
				startPosition.y = controlPoints[1].position.y;
				startPosition.z = controlPoints[1].position.z;
				this.transform.position = startPosition;
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
			this.transform.Rotate(rotateSpeed * Time.deltaTime, 0, 0);

			// t�� 1�� �����ϸ� ���� �������� ��ȯ
			if (t >= 1f)
			{
				t = 0f; // � �ð� �ʱ�ȭ
				currentSegment++;

				if(currentSegment == 1)
				{
					this.GetComponentInChildren<Animator>().SetBool("isTouch", true);
					rotateSpeed *= 8;
				}

				// ������ ������ ������ ��ũ��Ʈ ����
				if (currentSegment >= controlPoints.Length - 3)
				{
					currentSegment = 0;

                    // TODO <������> : hitCount��ŭ ���α׷����� �ø���

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