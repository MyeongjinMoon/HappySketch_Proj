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

		private readonly string swatterPath = "Prefabs/Obstacle/Team/SecondMission/Swatter";

        private readonly float obstacleTime = 8f;
        private readonly float creatureTime = 5.5f;
        private readonly float backgroundTime = 1.5f;
        private readonly float flyTime = 3f;
        private float obstacleTimer = 0;
		private float creatureTimer = 0;
		private float flyTimer = 0;
		private float backgroundTimer = 0;
		private readonly int playerCount = 2;
		private bool isThirdMissionGenerate = false;

		private RunningState runningState;
		private EGameState curState;
		private GameSceneController gamecSceneController;

		private Transform rayBox;
		private RaycastHit hit;
		private readonly float maxDistance = 10f;

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
						TimerIncrease(EGameState.RUNNING);

                        if (IsSpawnTime(obstacleTime, EGameState.RUNNING))
						{
							GenerateObstacles(EGameState.RUNNING);
                        }
						break;
					case EGameState.FIRSTMISSION:
                        TimerIncrease(EGameState.FIRSTMISSION);

                        if (IsBackgroundSpawnTime(backgroundTime))
							SpawnCreatureHerdBackground();

						if (IsSpawnTime(creatureTime, EGameState.FIRSTMISSION))
                            GenerateObstacles(EGameState.FIRSTMISSION);
                        break;
					case EGameState.SECONDMISSION:
                        TimerIncrease(EGameState.SECONDMISSION);

                        if (IsSpawnTime(flyTime, EGameState.SECONDMISSION))
                            GenerateObstacles(EGameState.SECONDMISSION);
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
			InitTimer();

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
		private void GenerateObstacles(EGameState curstate)
		{
			switch(curstate)
			{
				case EGameState.RUNNING:
                    {
						int playerindex = UnityEngine.Random.Range(0, playerCount);

						obstaclePool.SpawnObstacle(playerindex, runningState.GetPlayerDistance(playerindex) + 15);

						obstacleTimer = 0;
					}
                    break;
                case EGameState.FIRSTMISSION:
                    {
                        creatureHerdPool.SpawnCreatureHerd(UnityEngine.Random.Range(0, playerCount), MissionGroundPos);

						creatureTimer = 0;
					}
                    break;
                case EGameState.SECONDMISSION:
                    {
						flyPool.SpawnFly(MissionGroundPos);

						flyTimer = 0;
					}
                    break;
            }
		}
		private void GenerateVolcanicAsh()
		{
			volcanicAshPool.SpawnVolcanicAshes(MissionGroundPos);
		}
		private bool IsBackgroundSpawnTime(float time)
		{
			return backgroundTimer > time;
		}
		private bool IsSpawnTime(float time, EGameState curstate)
		{
			switch (curstate)
			{
				case EGameState.RUNNING:
                    return obstacleTimer > time;
                case EGameState.FIRSTMISSION:
                    return creatureTimer > time;
                case EGameState.SECONDMISSION:
					return flyTimer > time;
				default:
					return false;
			}
		}
        private void TimerIncrease(EGameState curstate)
        {
            switch (curstate)
            {
                case EGameState.RUNNING:
                    obstacleTimer += Time.deltaTime;
                    break;
                case EGameState.FIRSTMISSION:
                    creatureTimer += Time.deltaTime;
                    backgroundTimer += Time.deltaTime;
                    break;
                case EGameState.SECONDMISSION:
                    flyTimer += Time.deltaTime;
                    break;
            }
        }
        private void InitTimer()
        {
            obstacleTimer = 0;
            creatureTimer = 0;
            flyTimer = 0;
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