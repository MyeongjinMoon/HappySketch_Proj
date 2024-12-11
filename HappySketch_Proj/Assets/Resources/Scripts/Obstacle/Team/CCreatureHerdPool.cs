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

		private string smallPteranodonName = "Prefabs/Obstacle/Team/FirstMission/SmallPteranodon";      // �������� �����ϴ� ���� ��ġ
		private string bigPteranodonName = "Prefabs/Obstacle/Team/FirstMission/BigPteranodon";      // �������� �����ϴ� ���� ��ġ
		private string crocodileName = "Prefabs/Obstacle/Team/FirstMission/Crocodile";      // �������� �����ϴ� ���� ��ġ
		private GameObject smallPteranodon;
		private GameObject bigPteranodon;
		private GameObject crocodile;

		// >>: �ͷ� �̵� ��
		private Transform[] smallPteranodonControlPoints;
		private Transform[] bigPteranodonControlPoints;
		private Transform[] groundControlPoints;
		// <<

		private void Awake()
		{
			smallPteranodon = Resources.Load<GameObject>(smallPteranodonName);
			bigPteranodon = Resources.Load<GameObject>(bigPteranodonName);
			crocodile = Resources.Load<GameObject>(crocodileName);
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
			// TODO < ������ > - space�� Line�� x���� �޾Ƽ� ����ؾ� ��. - 2024.11.11 17:30
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

			// TODO < ������ > - ���� ��ġ�� �̼� �������� ��������� ��. - 2024.11.11 14:20
			// "30"�� "20"�� Line�� ���缭 �����ؾ� ��.
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