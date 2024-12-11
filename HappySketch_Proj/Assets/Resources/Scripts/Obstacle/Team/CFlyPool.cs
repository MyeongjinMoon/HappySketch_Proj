using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace MyeongJin
{
	public class CFlyPool : MonoBehaviour
	{
		public int maxPoolSize = 5;
		public int stackDefaultCapacity = 5;

		private string flyName = "Prefabs/Obstacle/Team/SecondMission/Fly";      // �������� �����ϴ� ���� ��ġ
		private GameObject fly;
        private GameObject parent;

        // TODO <������> : ���� ��ֹ� ��ġ�� �����ϰ� �ִ� ������ ������ ��
        private Vector3 mainCameraPosition;
		private Vector3 missionGroundPosition;

		private Vector3[] flyArr;
		private Vector3[] flyOffset;
        private Vector3 basePosition = new Vector3(0, 2.5f, 6f);

        private bool[] isFlyExist;

        private void Awake()
		{
			fly = Resources.Load<GameObject>(flyName);

            parent = GameObject.Find("ObstacleBox");
        }
		private void Start()
        {
            flyOffset = new Vector3[maxPoolSize];
            flyOffset[0] = new Vector3(-4, -1.5f, 0);
            flyOffset[1] = new Vector3(-2, 1.5f, 0);
            flyOffset[2] = new Vector3(0, -1.5f, 0);
            flyOffset[3] = new Vector3(2, 1.5f, 0);
            flyOffset[4] = new Vector3(4, -1.5f, 0);

            isFlyExist = new bool[maxPoolSize];
            flyArr = new Vector3[maxPoolSize];

            for (int i = 0; i < maxPoolSize; i++)
            {
                flyArr[i] = basePosition + flyOffset[i];
                isFlyExist[i] = false;
            }

            for (int i = 0; i < maxPoolSize; i++)
            {
                CreatedPooledItem().ReturnToPool();
            }
        }
		public IObjectPool<CFly> Pool
		{
			get
			{
				if (pool == null)
					pool = new ObjectPool<CFly>(
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
		private IObjectPool<CFly> pool;

		private CFly CreatedPooledItem()
		{
			CFly obstacle = null;

				var go = Instantiate(fly, parent.transform);
				go.name = "Fly";

				obstacle = go.AddComponent<CFly>();

			obstacle.Pool = Pool;

			return obstacle;
		}
		private void OnReturnedToPool(CFly obstacle)
		{
			obstacle.gameObject.SetActive(false);

			for (int i = 0; i < maxPoolSize; i++)
			{
				if(obstacle.transform.position == (missionGroundPosition + flyArr[i]))
				{
					isFlyExist[i] = false;
                }
			}
		}
		private void OnTakeFromPool(CFly obstacle)
		{
			obstacle.gameObject.SetActive(true);
		}
		private void OnDestroyPoolObject(CFly obstacle)
		{
			Destroy(obstacle.gameObject);
		}
		public void SpawnFly(Vector3 groundPosition)
		{
            missionGroundPosition = groundPosition;
			missionGroundPosition.y = 2;

            for (int i = 0; i < maxPoolSize; i++)
			{
				if (isFlyExist[i] || UnityEngine.Random.Range(0, 2) == 0)
					continue;

				CFly obstacle = Pool.Get();
                isFlyExist[i] = true;

                obstacle.transform.position = missionGroundPosition + flyArr[i];
			}
		}
	}
}