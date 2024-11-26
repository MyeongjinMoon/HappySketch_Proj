using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyeongJin
{
	public class CCrocodile : CCreatureHerd
	{
		private Vector3 startPosition;

		// >>: Stoop
		private List<Transform[]> controlPoints = new List<Transform[]>();  // 제어점 (최소 4개 필요)

		private float moveSpeed = 12f;		// 이동 속도
		private float t = 0f;				// Catmull-Rom 곡선의 시간 변수
		private int currentSegment = 0;		// 현재 이동 중인 곡선 구간
		public int targetNum = 0;
		// <<
		private int hitCount = 1;
		private void Awake()
		{
			controlPoints.Add(GameObject.Find("GroundControlPoint1").GetComponent<CGroundControlPoint>().controlPoints);
			controlPoints.Add(GameObject.Find("GroundControlPoint2").GetComponent<CGroundControlPoint>().controlPoints);
		}
		private void Update()
		{
			SwimAndAttack();
		}
		private void OnEnable()
		{
			targetNum = UnityEngine.Random.Range(0, 2);
			SetStartPosition();
			if(targetNum == 1)
			{
				Vector3 rotation = new Vector3(5, 220, 0);
				this.transform.rotation = Quaternion.Euler(rotation);
			}
            else
            {
                Vector3 rotation = new Vector3(5, 140, 0);
                this.transform.rotation = Quaternion.Euler(rotation);
            }
        }
		private void OnDisable()
		{
			ResetObstacle();
		}
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "PlayerCollider" || other.tag == "PlayerCollider")
            {
                Debug.Log("Crocodile Attack");
                this.GetComponent<BoxCollider>().enabled = false;
                hitCount--;
            }
        }
        public new void ResetObstacle()
		{
			hitCount = 1;
            this.GetComponent<BoxCollider>().enabled = true;

            SetStartPosition();
		}
		private void SetStartPosition()
		{
			if (controlPoints.Count != 0)
			{
				startPosition = this.transform.position;
				startPosition.y = controlPoints[targetNum][1].position.x;
				startPosition.y = controlPoints[targetNum][1].position.y;
				startPosition.z = controlPoints[targetNum][1].position.z;
				this.transform.position = startPosition;
			}
		}
		private void SwimAndAttack()
		{
			if (controlPoints[targetNum].Length < 4) return;

			Transform p0 = controlPoints[targetNum][currentSegment];
			Transform p1 = controlPoints[targetNum][currentSegment + 1];
			Transform p2 = controlPoints[targetNum][currentSegment + 2];
			Transform p3 = controlPoints[targetNum][currentSegment + 3];

			// Catmull-Rom 곡선 공식 사용
			Vector3 newPosition = CatmullRom(p0.position, p1.position, p2.position, p3.position, t);
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
                    this.GetComponentInChildren<Animator>().SetBool("isTouch", true);

				if (currentSegment >= controlPoints[targetNum].Length - 3)
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