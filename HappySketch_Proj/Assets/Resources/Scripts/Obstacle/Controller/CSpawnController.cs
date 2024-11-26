using HakSeung;
using JongJin;
using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.InspectorCurveEditor;

namespace MyeongJin
{
	public class CSpawnController : MonoBehaviour
	{
		public Vector3 MissionGroundPos { get; private set; }

		[SerializeField] private GameObject missionGround;

		private CObstacleObjectPool obstaclePool;
		private CCreatureHerdPool creatureHerdPool;
		private CCreatureBackgroundPool creatureBackgroundPool;
		private CFlyPool flyPool;
		private CVolcanicAshPool volcanicAshPool;
		private GameObject gameSceneController;
		private GameObject swatter;

		private string swatterPath = "Prefabs/Obstacle/Team/SecondMission/Swatter";      // �������� �����ϴ� ���� ��ġ

		private float obstacleTimer = 0;
		private float creatureTimer = 0;
		private float flyTimer = 0;
		private float backgroundTimer = 0;
		private int playerCount = 2;
		private bool isThirdMissionGenerate = false;

		private int curGeneratePosition;
		private int oldGeneratePosition;

		private RunningState runningState;
		private EGameState curState;
		private GameSceneController gamecSceneController;

		private Transform rayBox;
		private RaycastHit hit;
		private float maxDistance = 10f;

		// TODO < ������ > - ���� �÷��̾��� ��ġ�� �޾ƾ� ��. - 2024.11.07 16:10
		public int FirstPlayerPosition { get; private set; }
		/// <summary>
		/// ���� �ͷ� ���� �ֱ�� Timer�� ���� �ð��� �� ������ Ȯ�������� �����ϰ� ����.
		/// </summary>
		public int Timer { get; private set; }

		private void Start()
		{
			swatter = Resources.Load<GameObject>(swatterPath);

			MissionGroundPos = missionGround.GetComponent<Transform>().position;

			obstaclePool = gameObject.AddComponent<CObstacleObjectPool>();
			creatureHerdPool = gameObject.AddComponent<CCreatureHerdPool>();
			creatureBackgroundPool = gameObject.AddComponent<CCreatureBackgroundPool>();
			flyPool = gameObject.AddComponent<CFlyPool>();
			volcanicAshPool = gameObject.AddComponent<CVolcanicAshPool>();

			gameSceneController = GameObject.Find("GameSceneController");
			gamecSceneController = gameSceneController.GetComponent<GameSceneController>();
			runningState = gameSceneController.GetComponent<RunningState>();

			curState = gamecSceneController.CurState;
		}
		private void Update()
		{
			UpdateCurState();

			switch (curState)
			{
				case EGameState.RUNNING:
                    TimerIncrease();
                    if (IsSpawnTime(6f))
					{
						CheckCanSpawnObstacle();
						obstacleTimer = 0;
                    }
					break;
				case EGameState.TAILMISSION:

					break;
				case EGameState.FIRSTMISSION:
					TimerIncrease();

					if (IsBackgroundSpawnTime(1.5f))
						SpawnCreatureHerdBackground();

					if (IsCreatureSpawnTime(5.5f))
						CheckCanSpawnCreatureHerd();
					break;
				case EGameState.SECONDMISSION:
					TimerIncrease();

					if (IsFlySpawnTime(3))
						GenerateFly();
					break;
				case EGameState.THIRDMISSION:
					if (!isThirdMissionGenerate)
					{
						rayBox = GameObject.Find("Main Camera").transform;
						isThirdMissionGenerate = true;

						GenerateVolcanicAsh();
					}
					break;
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (curState == EGameState.THIRDMISSION)
				{
					GenerateRay();
				}
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				if (curState == EGameState.SECONDMISSION)
				{
					GenerateSwatter(1, 0);
				}
			}
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (curState == EGameState.SECONDMISSION)
                {
                    GenerateSwatter(1, 1);
                }
            }
        }
		/// <summary>
		/// Player(����)�� �ش� �Լ��� ȣ�����ְ� playerIndex, vertical(��,��)�� �μ��� ��� ȣ�����ָ� ��
		/// </summary>
		public void GenerateSwatter(int playerIndex, int vertical)
		{
			var go = Instantiate(swatter);
			go.name = "Swatter" + playerIndex;

			go.AddComponent<CSwatter>();

			go.GetComponent<CSwatter>().Init(playerIndex, vertical);
		}
		public void GenerateRay()
		{
			if(Physics.Raycast(rayBox.position, rayBox.forward,out hit, maxDistance))
			{
				hit.transform.GetComponent<CVolcanicAsh>().FadeAway();
			}
		}
		private void TimerIncrease()
		{
			creatureTimer += Time.deltaTime;
			flyTimer += Time.deltaTime;
			backgroundTimer += Time.deltaTime;
			obstacleTimer += Time.deltaTime;

        }
		private void SpawnCreatureHerdBackground()
		{
			creatureBackgroundPool.SpawnCreatureHerd(MissionGroundPos);
			backgroundTimer = 0;
		}

		private void UpdateCurState()
		{
			curState = gamecSceneController.CurState;
		}
		private void CheckCanSpawnObstacle()
		{
			// TODO < ������ > - "10"�� RubberBand Size�� �ٲ���� ��. - 2024.11.11 18:55
			int playerindex = UnityEngine.Random.Range(0, playerCount);

			GameObject obstacle = obstaclePool.SpawnObstacle(playerindex, runningState.GetPlayerDistance(playerindex) + 15);

			((CUIRunningCanvas)UIManager.Instance.CurSceneUI).playerNotes[playerindex].Show(obstacle, playerindex);
		}
		private void CheckCanSpawnCreatureHerd()
		{
			IsSpawnHerd(UnityEngine.Random.Range(0, playerCount));

			creatureTimer = 0;
		}
		private void GenerateFly()
		{
			flyPool.SpawnFly(MissionGroundPos);
			flyTimer = 0;
		}
		private void GenerateVolcanicAsh()
		{
			volcanicAshPool.SpawnVolcanicAshes(MissionGroundPos);
		}
		/// <summary>
		/// ū �ͷ�/�Ǿ ���� �� true�� ��ȯ�Ͽ� �ش� Line���� �� �̻� ��ȯ���� ����. ��, ���� �� ������� �Ѹ����� ����
		/// </summary>
		private void IsSpawnHerd(int i)
		{
			creatureHerdPool.SpawnCreatureHerd(i, MissionGroundPos);
		}
		private bool IsSpawnTime(float time)
		{
			return obstacleTimer > time;
            //curGeneratePosition = (int)runningState.FirstRankerDistance;

            //return curGeneratePosition != oldGeneratePosition && !Convert.ToBoolean((curGeneratePosition % 24));
        }
		private bool IsCreatureSpawnTime(float time)
		{
			return creatureTimer > time;
		}
		private bool IsBackgroundSpawnTime(float time)
		{
			return backgroundTimer > time;
		}
		private bool IsFlySpawnTime(float time)
		{
			return flyTimer > time;
		}
	}
}