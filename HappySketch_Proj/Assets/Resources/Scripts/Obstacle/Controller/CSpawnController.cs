using HakSeung;
using JongJin;
using System.Collections;
using UnityEngine;

namespace MyeongJin
{
	public class CSpawnController : MonoBehaviour
	{
		public Vector3 MissionGroundPos { get; private set; }
		public bool canSpawn = true;

		[SerializeField] private GameObject missionGround;

		private CObstacleObjectPool obstaclePool;
		private CCreatureHerdPool creatureHerdPool;
		private CCreatureBackgroundPool creatureBackgroundPool;
		private CFlyPool flyPool;
		private CVolcanicAshPool volcanicAshPool;
		private GameObject gameSceneController;
		private GameObject swatter;

		private string swatterPath = "Prefabs/Obstacle/Team/SecondMission/Swatter";

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

		private void Start()
        {
            rayBox = GameObject.Find("Main Camera").transform;
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

			if (canSpawn)
			{
				switch (curState)
				{
					case EGameState.RUNNING:
                        RunningTimerIncrease();
						if (IsSpawnTime(8f))
						{
							CheckCanSpawnObstacle();
							obstacleTimer = 0;
						}
						break;
					case EGameState.FIRSTMISSION:
                        FirstMissionTimerIncrease();

						if (IsBackgroundSpawnTime(1.5f))
							SpawnCreatureHerdBackground();

						if (IsCreatureSpawnTime(5.5f))
							CheckCanSpawnCreatureHerd();
						break;
					case EGameState.SECONDMISSION:
						SecondMissionTimerIncrease();

						if (IsFlySpawnTime(3))
							GenerateFly();
						break;
					case EGameState.THIRDMISSION:
						if (!isThirdMissionGenerate)
						{
							isThirdMissionGenerate = true;

							GenerateVolcanicAsh();
						}
						break;
				}
			}
        }
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
				if (hit.collider.gameObject.tag == "Ash")
					hit.transform.GetComponent<CVolcanicAsh>().FadeAway();
			}
		}
		private void RunningTimerIncrease()
		{
			obstacleTimer += Time.deltaTime;
        }
        private void FirstMissionTimerIncrease()
        {
            creatureTimer += Time.deltaTime;
            backgroundTimer += Time.deltaTime;
        }
        private void SecondMissionTimerIncrease()
        {
            flyTimer += Time.deltaTime;
        }
        private void SpawnCreatureHerdBackground()
		{
			creatureBackgroundPool.SpawnCreatureHerd(MissionGroundPos);
			backgroundTimer = 0;
		}
		private void UpdateCurState()
		{
			if (gamecSceneController.CurState != curState)
			{
				ChangedState(gamecSceneController.CurState);
				if (gamecSceneController.CurState != EGameState.RUNNING)
				{
					canSpawn = false;
					StartCoroutine(StayForTutorialPopUp(10f));
                }
                obstacleTimer = 0;
            }
			curState = gamecSceneController.CurState;
        }
		private void ChangedState(EGameState curState)
		{
			if (curState == EGameState.RUNNING)
				for (int i = 0; i < playerCount; i++)
				{
					((CUIRunningCanvas)UIManager.Instance.CurSceneUI).playerNotes[i].gameObject.SetActive(false);
				}
			else if (curState == EGameState.FIRSTMISSION)
                for (int i = 0; i < playerCount; i++)
                {
                    ((CUIEventPanel)UIManager.Instance.CurSceneUI).playerNotes[i].gameObject.SetActive(false);
                }
        }
        private void CheckCanSpawnObstacle()
		{
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
		private void IsSpawnHerd(int i)
		{
			creatureHerdPool.SpawnCreatureHerd(i, MissionGroundPos);
		}
		private bool IsSpawnTime(float time)
		{
			return obstacleTimer > time;
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
		private IEnumerator StayForTutorialPopUp(float time)
		{
			while(time > 0)
			{
				time -= Time.deltaTime;
				yield return null;
			}
			canSpawn = true;
		}
	}
}