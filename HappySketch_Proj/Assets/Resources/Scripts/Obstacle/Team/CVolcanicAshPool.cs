using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace MyeongJin
{
	public class CVolcanicAshPool : MonoBehaviour
	{
		public int maxPoolSize = 10;
		public int stackDefaultCapacity = 10;

		private string volcanicAshesPath = "Prefabs/Obstacle/Team/ThirdMission";
		private GameObject[] volcanicAshes;
		private GameObject parent;
		private Vector3[] volcanicAshOffset;
		private int ashNum = 0;

		private Vector3 mainCameraPosition;
		private Vector3 missionGroundPosition;

		private void Awake()
		{
			volcanicAshes = LoadPrefabsFromFolder(volcanicAshesPath);
            parent = GameObject.Find("ObstacleBox");
        }
		private void Start()
		{
			volcanicAshOffset = new Vector3[maxPoolSize];

			for (int i = 0; i < maxPoolSize; i++)
			{
				volcanicAshOffset[i] = new Vector3(Random.Range(-1, 2), 2.4f, i * 0.5f - 5);
            }

			for (int i = 0; i < maxPoolSize; i++)
			{
				CreatedPooledItem().ReturnToPool();
			}
		}
		public GameObject[] LoadPrefabsFromFolder(string folderPath)
		{
			GameObject[] prefabs = Resources.LoadAll<GameObject>(folderPath);

			if (prefabs.Length == 0)
			{
				Debug.LogError($"No prefabs found in folder: {folderPath}");
			}

			return prefabs;
		}
		public IObjectPool<CVolcanicAsh> Pool
		{
			get
			{
				if (pool == null)
					pool = new ObjectPool<CVolcanicAsh>(
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
		private IObjectPool<CVolcanicAsh> pool;

		private CVolcanicAsh CreatedPooledItem()
		{
			CVolcanicAsh obstacle = null;

			var go = Instantiate(volcanicAshes[ashNum], parent.transform);
			go.name = "Ash" + ashNum++;

			obstacle = go.AddComponent<CVolcanicAsh>();

			obstacle.Pool = Pool;

			return obstacle;
		}
		private void OnReturnedToPool(CVolcanicAsh obstacle)
		{
			obstacle.gameObject.SetActive(false);
		}
		private void OnTakeFromPool(CVolcanicAsh obstacle)
		{
			obstacle.gameObject.SetActive(true);
		}
		private void OnDestroyPoolObject(CVolcanicAsh obstacle)
		{
			Destroy(obstacle.gameObject);
		}
		public void SpawnVolcanicAshes(Vector3 groundPosition)
		{
			missionGroundPosition = groundPosition;
			missionGroundPosition.y = 0;

            for (int i = 0; i < maxPoolSize; i++)
			{
				CVolcanicAsh obstacle = Pool.Get();

				obstacle.transform.position = missionGroundPosition + volcanicAshOffset[i];
			}
		}
	}
}