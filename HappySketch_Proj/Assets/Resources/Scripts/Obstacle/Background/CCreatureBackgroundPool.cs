using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace MyeongJin
{
	public class CCreatureBackgroundPool : MonoBehaviour
	{
		public int maxPoolSize = 30;
		public int stackDefaultCapacity = 30;

        private Vector3 missionGroundPos;

		private string backgroundPteranodonName = "Prefabs/Obstacle/Team/Background/BackgroundPteranodon";      // �������� �����ϴ� ���� ��ġ
		private GameObject backgroundPteranodon;

		private void Awake()
		{
            backgroundPteranodon = Resources.Load<GameObject>(backgroundPteranodonName);
		}
		private void Start()
		{
			for (int i = 0; i < maxPoolSize; i++)
			{
				CreatedPooledItem().ReturnToPool();
			}
		}
		public IObjectPool<CCreatureHerd> Pool
		{
			get
			{
				if (pool == null)
					pool = new ObjectPool<CCreatureHerd>(
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
		private IObjectPool<CCreatureHerd> pool;

		private CCreatureHerd CreatedPooledItem()
		{
			CCreatureHerd obstacle = null;

			var go = Instantiate(backgroundPteranodon);
			go.name = "BackgroundPteranodon";

			obstacle = go.AddComponent<CCreatureBackground>();

			obstacle.Pool = Pool;

			return obstacle;
		}
		private void OnReturnedToPool(CCreatureHerd obstacle)
		{
			obstacle.gameObject.SetActive(false);
		}
		private void OnTakeFromPool(CCreatureHerd obstacle)
		{
			obstacle.gameObject.SetActive(true);
		}
		private void OnDestroyPoolObject(CCreatureHerd obstacle)
		{
			Destroy(obstacle.gameObject);
		}
		public bool SpawnCreatureHerd(Vector3 position)
		{
            CCreatureHerd obstacle = Pool.Get();

			obstacle.transform.position = new Vector3(position.x + UnityEngine.Random.Range(-20, 21), position.y + 15, position.z + 50);

			return true;
		}
	}
}