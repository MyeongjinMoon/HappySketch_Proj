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

		private string smallPteranodonName = "Prefabs/Obstacle/Team/FirstMission/SmallPteranodon";      // 프리팹이 존재하는 폴더 위치
		private string bigPteranodonName = "Prefabs/Obstacle/Team/FirstMission/BigPteranodon";      // 프리팹이 존재하는 폴더 위치
		private string crocodileName = "Prefabs/Obstacle/Team/FirstMission/Crocodile";      // 프리팹이 존재하는 폴더 위치
		private GameObject smallPteranodon;
		private GameObject bigPteranodon;
		private GameObject crocodile;

		// >>: 익룡 이동 점
		private Transform[] smallPteranodonControlPoints;
		private Transform[] bigPteranodonControlPoints;
		private Transform[] groundControlPoints;
		// <<

		private void Awake()
		{
			smallPteranodon = Resources.Load<GameObject>(smallPteranodonName);
			bigPteranodon = Resources.Load<GameObject>(bigPteranodonName);
			crocodile = Resources.Load<GameObject>(crocodileName);

			#region 프리팹 예외처리
			if (smallPteranodon != null)
			{
				Debug.Log($"프리팹 '{smallPteranodonName}'을(를) Load 하였습니다.");
			}
			else
			{
				Debug.LogError($"프리팹 '{smallPteranodonName}'을(를) 찾을 수 없습니다.");
				// 예외처리 코드 추가
			}
			if (bigPteranodon != null)
			{
				Debug.Log($"프리팹 '{bigPteranodonName}'을(를) Load 하였습니다.");
			}
			else
			{
				Debug.LogError($"프리팹 '{bigPteranodonName}'을(를) 찾을 수 없습니다.");
				// 예외처리 코드 추가
			}
			if (crocodile != null)
			{
				Debug.Log($"프리팹 '{crocodileName}'을(를) Load 하였습니다.");
			}
			else
			{
				Debug.LogError($"프리팹 '{crocodileName}'을(를) 찾을 수 없습니다.");
				// 예외처리 코드 추가
			}
			#endregion
		}
		private void Start()
		{
            smallPteranodonControlPoints = GameObject.Find("SmallPteranodonControlPoints").GetComponent<CSkyControlPoint>().controlPoints;
            bigPteranodonControlPoints = GameObject.Find("BigPteranodonControlPoints").GetComponent<CSkyControlPoint>().controlPoints;

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
					var go = Instantiate(smallPteranodon);
					go.name = "SmallPteranodon";

					obstacle = go.AddComponent<CSmallPteranodon>();
					break;
				case 1:
					go = Instantiate(bigPteranodon);
					go.name = "BigPteranodon";

					obstacle = go.AddComponent<CBigPteranodon>();
					break;
				case 2:
					go = Instantiate(crocodile);
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
			// TODO < 문명진 > - space를 Line의 x값을 받아서 사용해야 함. - 2024.11.11 17:30
			float space = 4f;

			CCreatureHerd obstacle = null;
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

			// TODO < 문명진 > - 생성 위치를 미션 지점으로 지정해줘야 함. - 2024.11.11 14:20
			// "30"과 "20"을 Line에 맞춰서 생성해야 함.
			if (obstacle is CSmallPteranodon)
			{
				obstacle.transform.position = new Vector3(lineNum * space + position.x - 2, smallPteranodonControlPoints[1].position.y, smallPteranodonControlPoints[1].position.z);

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