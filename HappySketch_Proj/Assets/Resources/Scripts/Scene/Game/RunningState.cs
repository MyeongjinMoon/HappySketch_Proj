using Cinemachine;
using HakSeung;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace JongJin
{
	public class RunningState : MonoBehaviour, IGameState
	{
		[Header("Object")]
		[SerializeField] private GameObject[] players;
		[SerializeField] private GameObject dinosaur;
		[SerializeField] private GameObject[] crown;

		[Header("Buff")]
		[SerializeField] private ParticleSystem mapBuffParticle;
		[SerializeField] private ParticleSystem[] buffParticles;
		[SerializeField] private ParticleSystem[] deBuffParticles;

		[Header("Time")]
		[SerializeField] private float roundTimeLimit = 300.0f;
		[SerializeField] private float buffTime = 10.0f;
		[SerializeField] private float debuffTime = 5.0f;
		[SerializeField] private float normalBuffTime = 3.0f;
		[SerializeField] private float normalDeBuffTime = 3.0f;

        [Header("Distance")]
		[SerializeField] private float totalRunningDistance = 500.0f;
		[SerializeField] private float minDistance = 13.0f;
		[SerializeField] private float maxDistance = 25.0f;
		[SerializeField] private float playerSpacing = 3.0f;
		[SerializeField] private float buffDistance = 10.0f;
		[SerializeField] private float deBuffDistance = 6.0f;

		[Header("ProgressRate")]
		[SerializeField] private float tailMissionStartRate = 15.0f;
		[SerializeField] private float tailMissionEndRate = 90.0f;
		[SerializeField] private float firstMissionRate = 35.0f;
		[SerializeField] private float secondMissionRate = 55.0f;
		[SerializeField] private float thirdMissionRate = 80.0f;

        [Header("Virtual Camera")] 
		[SerializeField] private GameObject runningViewCam;
		[SerializeField] private float plusCameraPosZ = 2.5f;

		[Header("UI")]
		[SerializeField] private Image fadeImage;

		[HideInInspector] public bool isMissionSuccess = false;
		[HideInInspector] public bool isPrevStateTail = false;
		[HideInInspector] public bool isDebuff = false;

		public int Life { get; set; } = 3;
		private float crownTimer = 0.0f;
		private float totalRoundTime = 0.0f;
		private float[] playersClearTime;
		private int isClearStage = 0;
		private bool isFinish = false;

		private bool isRunning = true;
        private bool isPossibleTailMission = false;
		private bool isFirstMissionCompleted = false;
		private bool isSecondMissionCompleted = false;
		private bool isThirdMissionCompleted = false;

		private int firstRankerId = 0;
		private float[] playerDistance = { 0.0f, 0.0f, 0.0f, 0.0f };
		private float dinosaurDistance = 0.0f;
		private Vector3[] prevPlayerPosition = new Vector3[4];
		private Vector3 prevDinosaurPosition;
		private float firstRankerDistance = 0.0f;
		private float lastRankerDistance = 0.0f;

        public float RoundTimeLimit { get { return roundTimeLimit; } }
		public float BuffTime { get { return buffTime; } }
		public float NormalBuffTime {  get { return normalBuffTime; } }
		public float NormalDeBuffTime { get { return normalDeBuffTime; } }
		

        public float FirstRankerDistance { get { return firstRankerDistance; } }

        public float ProgressRate { get { return firstRankerDistance / totalRunningDistance * 100.0f; } }
		public float ProgressEndRate { get { return lastRankerDistance / totalRunningDistance * 100.0f; } }

		public float DinosaurSpeed { get { return dinosaurSpeed; } }
		private float dinosaurSpeed = 2.0f;


		public float GetPlayerDistance(int playerNumber)
		{
			return playerDistance[playerNumber];
		}
		public Vector3 GetPlayerPrevPosition(int playerNumber)
		{
			return prevPlayerPosition[playerNumber];
		}
        private void Start()
        {
            InitPlayerPos();

			Life = 3;
			totalRoundTime = roundTimeLimit;
			playersClearTime = new float[players.Length];
        }
        public void EnterState()
		{
            dinosaurSpeed = dinosaur.GetComponent<DinosaurController>().Speed;
            runningViewCam.GetComponent<CinemachineVirtualCamera>().Priority = 20;

			if (dinosaurDistance <= 1e-3)
				return;

			SetInfo();
			isRunning = true;

			if (isMissionSuccess)
				StartCoroutine(OnBuff());
			else
			{
				if(isPrevStateTail)
					SetHeart();
				StartCoroutine(OnDeBuff());
            }

            crownTimer = 0.0f;
        }
		public void UpdateState()
		{
            if (isFinish)
                return;
			EndGame();

            roundTimeLimit -= Time.deltaTime;

			UpdateUI();
			UpdateCrown();

            if (!isPossibleTailMission && ProgressRate > tailMissionStartRate)
				isPossibleTailMission = true;
			if(isPossibleTailMission && ProgressRate > tailMissionEndRate)
				isPossibleTailMission = false;

			Move();
			CalculateObjectDistance();
			CalculateRank();
		}

		public void ExitState()
		{
			SaveInfo();
            runningViewCam.GetComponent<CinemachineVirtualCamera>().Priority = 16;

            UIManager.Instance.SceneUISwap((int)UIManager.ESceneUIType.EventScenePanel);

            isRunning = false;

			EndBuffEffect();
			EndDeBuffEffect();
        }
		private void Move()
		{
			float plusPos = 0.0f;
			if (isMissionSuccess) plusPos = plusCameraPosZ;
			transform.position = Vector3.forward * (Mathf.Lerp(transform.position.z, firstRankerDistance + plusPos, 0.5f));
		}

		#region 씬 전환시 플레이어, 공룡 정보 Setting
		private void InitPlayerPos()
		{
			float offset = 1.0f * (players.Length - 1) / 2.0f * playerSpacing;

			for (int playerNum = 0; playerNum < players.Length; playerNum++)
				prevPlayerPosition[playerNum] = new Vector3(offset + playerNum * -playerSpacing, 0.5f, 0.0f);
		}
		private void SetInfo()
		{
			for(int playerNum = 0; playerNum < players.Length; playerNum++)
				players[playerNum].transform.position 
					= new Vector3(prevPlayerPosition[playerNum].x, prevPlayerPosition[playerNum].y, prevPlayerPosition[playerNum].z);
			dinosaur.transform.position = prevDinosaurPosition;

			transform.position = new Vector3(transform.position.x, transform.position.y, firstRankerDistance);
        }
		private void SaveInfo()
		{
			for (int playerNum = 0; playerNum < players.Length; playerNum++)
				prevPlayerPosition[playerNum] =
					new Vector3(players[playerNum].transform.position.x, players[playerNum].transform.position.y + 0.5f, players[playerNum].transform.position.z + 8.0f);
			prevDinosaurPosition = dinosaur.transform.position;
			firstRankerDistance = players[firstRankerId].transform.position.z;
			lastRankerDistance = players[1 - firstRankerId].transform.position.z;

		}
		#endregion

		#region 플레이어, 공룡 거리 및 랭킹 계산
		private void CalculateObjectDistance()
		{
			for (int playerNum = 0; playerNum < players.Length; playerNum++)
				playerDistance[playerNum] = players[playerNum].transform.position.z;
			dinosaurDistance = dinosaur.transform.position.z;
		}
		private void CalculateRank()
		{
			if (ProgressRate >= 100.0f)
				return;

			firstRankerDistance = playerDistance[0];
			lastRankerDistance = playerDistance[0];
			firstRankerId = 0;

            for (int playerNum = 1; playerNum < players.Length; playerNum++)
			{
				if (playerDistance[playerNum] > firstRankerDistance) {firstRankerDistance = playerDistance[playerNum]; firstRankerId = playerNum; }
                if (playerDistance[playerNum] < lastRankerDistance) lastRankerDistance = playerDistance[playerNum];
			}
		}
		#endregion

		#region 러버 밴딩의 최대 최소 범위 제한
		public bool IsBeyondMaxDistance(Vector3 position)
		{
			if (position.z - dinosaur.transform.position.z > maxDistance)
				return false;
			return true;
		}

		public bool IsUnderMinDistance(Vector3 position, out Vector3 curPos)
		{
			curPos = new Vector3(position.x, position.y, dinosaur.transform.position.z + minDistance);
			if (position.z - dinosaur.transform.position.z < minDistance)
				return true;
			return false;
		}
		#endregion

		#region Running 상태일 때 전이 조건
		public bool IsTailMissionTriggered()
		{
			if (!isPossibleTailMission)
				return false;
			if (!isRunning)
				return false;
			if (minDistance + 0.001f <= lastRankerDistance - dinosaurDistance)
				return false;
			return true;
		}

		public bool IsFirstMissionTriggered()
		{
			if (isFirstMissionCompleted)
				return false;
			if (ProgressRate < firstMissionRate)
				return false;

			isFirstMissionCompleted = true;
			return true;
		}
		public bool IsSecondMissionTriggered()
		{
			if (isSecondMissionCompleted)
				return false;
			if (ProgressRate < secondMissionRate)
				return false;

			isSecondMissionCompleted = true;
			return true;
		}
		public bool IsThirdMissionTriggered()
		{
			if (isThirdMissionCompleted)
				return false;
			if (ProgressRate < thirdMissionRate)
				return false;

			isThirdMissionCompleted = true;
			return true;
		}

        #endregion

        #region Crown 관련 함수
        private void UpdateCrown()
		{
			if (crown[firstRankerId].gameObject.activeSelf)
				return;

			crownTimer += Time.deltaTime;
			if (crownTimer < 1.0f)
				return;
			crownTimer = 0.0f;

            crown[1 - firstRankerId].gameObject.SetActive(false);
			crown[firstRankerId].gameObject.SetActive(true);
		}
		#endregion

		#region 버프 관련 함수 (미션씬에서 성공, 실패시)
		IEnumerator OnBuff()
		{
			float curMaxDistance = maxDistance;
			maxDistance += buffDistance;

			StartBuffEffect();
			yield return new WaitForSeconds(buffTime);
			EndBuffEffect();

			while (maxDistance > curMaxDistance)
			{
				maxDistance -= Time.deltaTime;
				yield return null;
			}
			
			isMissionSuccess = false;
		}
		IEnumerator OnDeBuff()
		{
			float curMaxDistance = maxDistance;
			maxDistance -= deBuffDistance;
			isDebuff = true;

			StartDeBuffEffect();
			yield return new WaitForSeconds(debuffTime);
			EndDeBuffEffect();

            while (maxDistance < curMaxDistance)
            {
                maxDistance += Time.deltaTime;
                yield return null;
            }
            isDebuff = false;
		}

		private void StartBuffEffect()
		{
			for (int playerNum = 0; playerNum < players.Length; playerNum++)
				buffParticles[playerNum].Play();
			mapBuffParticle.Play();
		}
		private void EndBuffEffect()
		{
            for (int playerNum = 0; playerNum < players.Length; playerNum++)
                buffParticles[playerNum].Stop();
            mapBuffParticle.Stop();
        }
        private void StartDeBuffEffect()
        {
            for (int playerNum = 0; playerNum < players.Length; playerNum++)
                deBuffParticles[playerNum].Play();
        }
        private void EndDeBuffEffect()
        {
            for (int playerNum = 0; playerNum < players.Length; playerNum++)
                deBuffParticles[playerNum].Stop();
        }
        #endregion

        #region UI 관련 함수
        private void UpdateUI()
		{
			SetProgressBar();
			SetTimer();
			SetPlayerImage();
			SetDinosaurImage();
			SetDinosaurDistanceText();
            SetEndLineDistanceText();
        }
		private void SetProgressBar()
		{
			((CUIRunningCanvas)UIManager.Instance.CurSceneUI).SetProgressBar(ProgressEndRate);
		}
		private void SetPlayerImage()
		{
			for(int playerNum = 0; playerNum < 2; playerNum++)
				((CUIRunningCanvas)UIManager.Instance.CurSceneUI).SetPlayerImage(playerNum, playerDistance[playerNum], totalRunningDistance);
        }

		private void SetDinosaurImage()
		{
            ((CUIRunningCanvas)UIManager.Instance.CurSceneUI).SetDinosaurImage(dinosaurDistance, totalRunningDistance);
        }

		private void SetDinosaurDistanceText()
		{
			((CUIRunningCanvas)UIManager.Instance.CurSceneUI).SetDinosaurDistanceText(lastRankerDistance, dinosaurDistance);
        }
        private void SetEndLineDistanceText()
        {
			((CUIRunningCanvas)UIManager.Instance.CurSceneUI).SetEndLineDistanceText(lastRankerDistance, totalRunningDistance);
        }
        private void SetTimer()
		{
			((CUIRunningCanvas)UIManager.Instance.CurSceneUI).SetTimer(roundTimeLimit);
        }
		private void SetHeart()
		{
			isPrevStateTail = false;
			((CUIRunningCanvas)UIManager.Instance.CurSceneUI).SetHeart(Life);
        }

        #endregion

        #region 스테이지 종료
        private void EndGame()
		{
			if (ProgressRate >= 99.0f && playersClearTime[firstRankerId] == 0) playersClearTime[firstRankerId] = totalRoundTime - roundTimeLimit;
			if (ProgressEndRate >= 99.0f && playersClearTime[1 - firstRankerId] == 0) playersClearTime[1 - firstRankerId] = totalRoundTime - roundTimeLimit;

			if (Life > 0 && roundTimeLimit > 0.0f && ProgressEndRate < 99.0f)
				return;

			isFinish = true;

			isClearStage = 0;
			if (ProgressEndRate >= 99.0f)
				isClearStage = 1;

			SendEndingInfo();
			StartCoroutine(FadeIN());
            StartCoroutine(SceneManagerExtended.Instance.GoToEndingScene());
        }

		private void SendEndingInfo()
		{
			PlayerPrefs.SetInt("ClearStage", isClearStage);
			PlayerPrefs.SetFloat("Player1Time", (playersClearTime[0]));
			PlayerPrefs.SetFloat("Player2Time",(playersClearTime[1]));
        }
        IEnumerator FadeIN()
		{
            float time = 0.0f;

            Color alpha = fadeImage.color;
            while (alpha.a < 1.0f)
            {
                time += Time.deltaTime;
                alpha.a = Mathf.Lerp(0.0f, 1.0f, time);
                fadeImage.color = alpha;
                yield return null;
            }
        }
        #endregion
    }
}