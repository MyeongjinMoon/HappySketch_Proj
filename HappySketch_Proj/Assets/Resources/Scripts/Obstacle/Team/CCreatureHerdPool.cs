using HakSeung;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace MyeongJin
{
	public class CCreatureHerdPool : MonoBehaviour
	{
		public int maxPoolSize = 15;
		public int stackDefaultCapacity = 15;

		private int creatureNum = 0;
		private int bigCreatureStack = 0;

		private readonly string smallPteranodonPath = "Prefabs/Obstacle/Team/FirstMission/SmallPteranodon";
        private readonly string bigPteranodonPath = "Prefabs/Obstacle/Team/FirstMission/BigPteranodon";
		private readonly string crocodilePath = "Prefabs/Obstacle/Team/FirstMission/Crocodile";
		private GameObject smallPteranodon;
		private GameObject bigPteranodon;
		private GameObject crocodile;
        private GameObject parent;

		private void Awake()
		{
			smallPteranodon = Resources.Load<GameObject>(smallPteranodonPath);
			bigPteranodon = Resources.Load<GameObject>(bigPteranodonPath);
			crocodile = Resources.Load<GameObject>(crocodilePath);

            parent = GameObject.Find("ObstacleBox");
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

			switch (creatureNum / 5)
			{
				case 0:
					var go = Instantiate(smallPteranodon, parent.transform);
					go.name = "SmallPteranodon";

					obstacle = go.AddComponent<CSmallPteranodon>();
					break;
				case 1:
					go = Instantiate(bigPteranodon, parent.transform);
					go.name = "BigPteranodon";

					obstacle = go.AddComponent<CBigPteranodon>();
					break;
				case 2:
					go = Instantiate(crocodile, parent.transform);
					go.name = "Crocodile";

					obstacle = go.AddComponent<CCrocodile>();
					break;
			}
			creatureNum++;

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
		public void SpawnCreatureHerd(int lineNum, Vector3 position)
		{
			float space = 4f;

			CCreatureHerd obstacle;
			List<CCreatureHerd> temp = new List<CCreatureHerd>();

			if (bigCreatureStack % 5 != 0)
			{
				if (UnityEngine.Random.Range(0, 2) == 0)
					while (true)
					{
						obstacle = Pool.Get();
						if (obstacle is CSmallPteranodon)
						{
							foreach (CCreatureHerd i in temp)
							{
								i.ReturnToPool();
							}
							break;
						}
						else
						{
							temp.Add(obstacle);
						}
					}
				else
					while (true)
					{
						obstacle = Pool.Get();
						if (obstacle is CCrocodile)
						{
							foreach (CCreatureHerd i in temp)
							{
								i.ReturnToPool();
							}
							break;
						}
						else
						{
							temp.Add(obstacle);
						}
					}
			}
			else
			{
				while (true)
				{
					obstacle = Pool.Get();
					if (obstacle is CBigPteranodon)
					{
						foreach (CCreatureHerd i in temp)
						{
							i.ReturnToPool();
						}
						break;
					}
					else
					{
						temp.Add(obstacle);
					}
				}
			}
			bigCreatureStack++;

			if (obstacle is CSmallPteranodon)
			{
				obstacle.transform.position = new Vector3(lineNum * space + position.x - 2, obstacle.transform.position.y, obstacle.transform.position.z);

                ((CUIEventPanel)UIManager.Instance.CurSceneUI).playerNotes[lineNum].Show(obstacle.gameObject, lineNum);
            }
            if (obstacle is CBigPteranodon)
            {
                ((CUIEventPanel)UIManager.Instance.CurSceneUI).playerNotes[0].Show(obstacle.gameObject, 0);
                ((CUIEventPanel)UIManager.Instance.CurSceneUI).playerNotes[1].Show(obstacle.gameObject, 1);
            }
			if(obstacle is CCrocodile)
			{
                CCrocodile crocodile = obstacle as CCrocodile;
				((CUIEventPanel)UIManager.Instance.CurSceneUI).playerNotes[lineNum].Show(obstacle.gameObject, crocodile.targetNum);
            }
		}
	}
}