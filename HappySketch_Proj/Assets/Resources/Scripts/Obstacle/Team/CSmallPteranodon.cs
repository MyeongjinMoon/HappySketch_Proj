using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyeongJin
{
	public class CSmallPteranodon : CCreatureHerd
	{
		private Vector3 startPosition;
		// >>: Stoop
		private Transform[] controlPoints;  // 제어점 (최소 4개 필요)

		private float moveSpeed = 10f;       // 이동 속도
		private float t = 0f;               // Catmull-Rom 곡선의 시간 변수
		private int currentSegment = 0;     // 현재 이동 중인 곡선 구간
											// <<
		private int hitCount = 1;
		private void Start()
		{
			controlPoints = GameObject.Find("SmallPteranodonControlPoints").GetComponent< CSkyControlPoint>().controlPoints;
		}
		private void Update()
		{
			StoopAndClimb();
		}
		private void OnEnable()
		{
			this.transform.Rotate(45, 0, 0);
		}
		private void OnDisable()
		{
			ResetObstacle();
		}
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "PlayerCollider" || other.tag == "PlayerCollider")
            {
                Debug.Log("SmallPteranodon Attack");
				this.GetComponent<BoxCollider>().enabled = false;
				hitCount--;
            }
        }
        public new void ResetObstacle()
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
				startPosition = this.transform.position;
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

			// Catmull-Rom 곡선 공식 사용
			Vector3 newPosition = CatmullRom(p0.position, p1.position, p2.position, p3.position, t);
			newPosition.x = transform.position.x;
			transform.position = newPosition;

			// 곡선의 t 값을 점진적으로 업데이트 (speed로 곡선 이동 속도 조정)
			t += Time.deltaTime * moveSpeed / Vector3.Distance(p1.position, p2.position);

			// t가 1에 도달하면 다음 구간으로 전환
			if (t >= 1f)
			{
				t = 0f; // 곡선 시간 초기화
				currentSegment++;

				// 마지막 구간을 지나면 스크립트 중지
				if (currentSegment == 1)
				{
					this.GetComponentInChildren<Animator>().SetBool("isTouch", true);
					this.transform.Rotate(-45, 0, 0);
				}

				if (currentSegment >= controlPoints.Length - 3)
				{
					currentSegment = 0;

                    // TODO <문명진> : hitCount만큼 프로그래스바 늘리기
					
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