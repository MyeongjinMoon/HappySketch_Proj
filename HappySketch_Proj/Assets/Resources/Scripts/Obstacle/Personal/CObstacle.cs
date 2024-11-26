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
		private EGameState curState;
		private EGameState oldState;

		private float timeToCheckPosition = 0.1f;
		private int maxRotateValue = 1;
		private int minRotateSpeed = 1;
		private int curRotateSpeed;

		//TODO < 문명진 > - destructPosition Value를 Dinosaur의 끝 부분으로 변경해야함. - 2024.11.07 4:10
		private float destructPosition;
		// <<

		private void Start()
		{
			dinasaur = GameObject.Find("Dinosaur");

			//TODO < 문명진 > - CGenerator 클래스가 아닌 공룡의 위치를 가지고 있는 녀석의 클래스에서 가져와야함. - 2024.11.07 4:10
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
            if (other.tag == "Player1" || other.tag == "Player2")
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

			if (this.transform.position.z < destructPosition)
				ReturnToPool();
		}
		private void StateCheck()
		{
			curState = gamecSceneController.CurState;
		}
		private bool IsStateChanged()
		{
			bool isChanged = false;

			if (oldState != curState)
				isChanged = true;

			oldState = curState;

			return isChanged;
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
            //TODO < 문명진 > - 돌의 삭제 위치를 공룡 위치로 초기화 해줘야함. - 2024.11.07 4:20
            this.GetComponent<BoxCollider>().enabled = true;
        }
		private void RotateObstacle()
		{
			this.transform.Rotate(-curRotateSpeed, 0, 0);
		}
	}
}