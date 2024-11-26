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

		private string volcanicAshesPath = "Prefabs/Obstacle/Team/ThirdMission";	// 프리팹이 존재하는 폴더 위치
		private GameObject[] volcanicAshes;
		private Vector3[] volcanicAshOffset;
		private int ashNum = 0;

		// TODO <문명진> : 추후 장애물 위치를 저장하고 있는 변수를 가져올 것
		private Vector3 mainCameraPosition;
		private Vector3 missionGroundPosition;

		private void Awake()
		{
			volcanicAshes = LoadPrefabsFromFolder(volcanicAshesPath);
		}
		private void Start()
		{
			volcanicAshOffset = new Vector3[maxPoolSize];

			for (int i = 0; i < maxPoolSize; i++)
			{
				volcanicAshOffset[i] = new Vector3(Random.Range(-1, 2), 2.5f, i * 0.5f - 5);
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

			var go = Instantiate(volcanicAshes[ashNum]);
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

			for (int i = 0; i < maxPoolSize; i++)
			{
				CVolcanicAsh obstacle = Pool.Get();

				obstacle.transform.position = groundPosition + volcanicAshOffset[i];
			}
		}
	}
}