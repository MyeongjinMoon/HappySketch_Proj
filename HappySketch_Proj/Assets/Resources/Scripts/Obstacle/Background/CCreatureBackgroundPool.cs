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

        private int creatureNum = 0;

		private string backgroundPteranodonName = "Prefabs/Obstacle/Team/Background/BackgroundPteranodon";      // 프리팹이 존재하는 폴더 위치
		private GameObject backgroundPteranodon;

		private void Awake()
		{
            backgroundPteranodon = Resources.Load<GameObject>(backgroundPteranodonName);

			#region 프리팹 예외처리
			if (backgroundPteranodon != null)
			{
				Debug.Log($"프리팹 '{backgroundPteranodonName}'을(를) Load 하였습니다.");
			}
			else
			{
				Debug.LogError($"프리팹 '{backgroundPteranodonName}'을(를) 찾을 수 없습니다.");
				// 예외처리 코드 추가
			}
			#endregion
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