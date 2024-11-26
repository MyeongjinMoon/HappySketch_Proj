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

        private string logName = "Prefabs/Obstacle/Personal/Log";		// �������� �����ϴ� ���� ��ġ
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

            #region ������ ����ó��
            if (log != null)
			{
				Debug.Log($"������ '{logName}'��(��) Load �Ͽ����ϴ�.");
			}
			else
			{
				Debug.LogError($"������ '{logName}'��(��) ã�� �� �����ϴ�.");
				// ����ó�� �ڵ� �߰�
			}
			if (rock != null)
			{
				Debug.Log($"������ '{rockName}'��(��) Load �Ͽ����ϴ�.");
			}
			else
			{
				Debug.LogError($"������ '{rockName}'��(��) ã�� �� �����ϴ�.");
				// ����ó�� �ڵ� �߰�
			}
            if (rock1 != null)
            {
                Debug.Log($"������ '{rock1Name}'��(��) Load �Ͽ����ϴ�.");
            }
            else
            {
                Debug.LogError($"������ '{rock1Name}'��(��) ã�� �� �����ϴ�.");
                // ����ó�� �ڵ� �߰�
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

			switch (UnityEngine.Random.Range(0,3))
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
            //TODO < ������ > - -1.5f�� �÷��̾� �� �������� ����. - 2024.11.11 19:45
            float space = 3f;

			var obstacle = Pool.Get();

			obstacle.transform.position = new Vector3(lineNum * -space + space / 2, 0.25f, zPosition);

			return obstacle.gameObject;
        }
    }
}