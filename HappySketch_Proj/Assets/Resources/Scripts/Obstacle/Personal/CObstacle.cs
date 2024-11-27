using JongJin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace MyeongJin
{
	public class CObstacle : MonoBehaviour
	{
		public IObjectPool<CObstacle> Pool { get; set; }

		private GameObject dinasaur;

		private GameObject gameSceneController;
		private GameSceneController gamecSceneController;
		private EGameState curState = EGameState.RUNNING;

		private float timeToCheckPosition = 0.1f;
		private int maxRotateValue = 1;
		private int minRotateSpeed = 1;
		private int curRotateSpeed;

		//TODO < ������ > - destructPosition Value�� Dinosaur�� �� �κ����� �����ؾ���. - 2024.11.07 4:10
		private float destructPosition;
		// <<

		private void Start()
		{
			dinasaur = GameObject.Find("Dinosaur");

			//TODO < ������ > - CGenerator Ŭ������ �ƴ� ������ ��ġ�� ������ �ִ� �༮�� Ŭ�������� �����;���. - 2024.11.07 4:10
			curRotateSpeed = Random.Range(minRotateSpeed, minRotateSpeed + maxRotateValue);

			gameSceneController = GameObject.Find("GameSceneController");
			gamecSceneController = gameSceneController.GetComponent<GameSceneController>();
		}

		private void Update()
		{
			RotateObstacle();
		}
		private void OnEnable()
		{
			StartCoroutine(CheckPosition());
		}
		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "PlayerCollider")
			{
				this.GetComponent<BoxCollider>().enabled = false;
			}
		}
		IEnumerator CheckPosition()
		{
			yield return new WaitForSeconds(timeToCheckPosition);

			//StateCheck();

			//if (IsStateChanged())
			//{
			//	ReturnToPool();
			//	yield return null;
			//}

			destructPosition = dinasaur.GetComponent<Transform>().position.z;

			StartCoroutine(CheckPosition());

			if (IsStateChanged())
			{
				ReturnToPool();
			}
			else if (this.transform.position.z < destructPosition)
				ReturnToPool();
		}
		private bool IsStateChanged()
		{
			return curState != gamecSceneController.CurState;
		}
		private void OnDisable()
		{
			ResetObstacle();
		}

		private void ReturnToPool()
		{
			Pool.Release(this);
		}
		private void ResetObstacle()
		{
			//TODO < ������ > - ���� ���� ��ġ�� ���� ��ġ�� �ʱ�ȭ �������. - 2024.11.07 4:20
			this.GetComponent<BoxCollider>().enabled = true;
		}
		private void RotateObstacle()
		{
			this.transform.Rotate(-curRotateSpeed, 0, 0);
		}
	}
}