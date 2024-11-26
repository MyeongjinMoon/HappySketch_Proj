using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace MyeongJin
{
	public class CObstacleObjectPool : MonoBehaviour
	{
		public int maxPoolSize = 10;
		public int stackDefaultCapacity = 10;

        private string logName = "Prefabs/Obstacle/Personal/Log";		// 프리팹이 존재하는 폴더 위치
		private string rockName = "Prefabs/Obstacle/Personal/Rock";
		private string rock1Name = "Prefabs/Obstacle/Personal/Rock1";
		private GameObject log;
		private GameObject rock;
		private GameObject rock1;

		private void Awake()
		{
			log = Resources.Load<GameObject>(logName);
			rock = Resources.Load<GameObject>(rockName);
			rock1 = Resources.Load<GameObject>(rock1Name);

            #region 프리팹 예외처리
            if (log != null)
			{
				Debug.Log($"프리팹 '{logName}'을(를) Load 하였습니다.");
			}
			else
			{
				Debug.LogError($"프리팹 '{logName}'을(를) 찾을 수 없습니다.");
				// 예외처리 코드 추가
			}
			if (rock != null)
			{
				Debug.Log($"프리팹 '{rockName}'을(를) Load 하였습니다.");
			}
			else
			{
				Debug.LogError($"프리팹 '{rockName}'을(를) 찾을 수 없습니다.");
				// 예외처리 코드 추가
			}
            if (rock1 != null)
            {
                Debug.Log($"프리팹 '{rock1Name}'을(를) Load 하였습니다.");
            }
            else
            {
                Debug.LogError($"프리팹 '{rock1Name}'을(를) 찾을 수 없습니다.");
                // 예외처리 코드 추가
            }
            #endregion
        }

        public IObjectPool<CObstacle> Pool
		{
			get
			{
				if (pool == null)
					pool = new ObjectPool<CObstacle>(
								CreatedPooledItem,
								OnTakeFromPool,
								OnReturnedToPool,
								OnDestroyPoolObject,
								true,
								stackDefaultCapacity,
								maxPoolSize);

				return pool;
			}
		}
		private IObjectPool<CObstacle> pool;

		private CObstacle CreatedPooledItem()
		{
			CObstacle obstacle;

			switch (UnityEngine.Random.Range(0,2))
			{
				case 0:
					var go = Instantiate(log);
					go.name = "Log";

					obstacle = go.AddComponent<CObstacle>();
					break;
				case 1:
					go = Instantiate(rock);
					go.name = "Rock";

					obstacle = go.AddComponent<CObstacle>();
					break;
                case 2:
                    go = Instantiate(rock1);
                    go.name = "Rock1";

                    obstacle = go.AddComponent<CObstacle>();
                    break;
                default:
					obstacle = null;
					break;
			}

			obstacle.Pool = Pool;

			return obstacle;
		}
		private void OnReturnedToPool(CObstacle obstacle)
		{
			obstacle.gameObject.SetActive(false);
		}
		private void OnTakeFromPool(CObstacle obstacle)
		{
			obstacle.gameObject.SetActive(true);
		}
		private void OnDestroyPoolObject(CObstacle obstacle)
		{
			Destroy(obstacle.gameObject);
		}
		public GameObject SpawnObstacle(int lineNum, float zPosition)
		{
            //TODO < 문명진 > - -1.5f를 플레이어 간 간격으로 조정. - 2024.11.11 19:45
            float space = 3f;

			var obstacle = Pool.Get();

			obstacle.transform.position = new Vector3(lineNum * -space + space / 2, 0.25f, zPosition);

			return obstacle.gameObject;
        }
    }
}