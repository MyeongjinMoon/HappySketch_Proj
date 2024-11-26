using Cinemachine;
using HakSeung;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static HakSeung.UIManager;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace JongJin
{
	public class RunningState : MonoBehaviour, IGameState
	{
		private readonly float progressBarStartX = -650.0f;
		private readonly float progressBarEndX = 670.0f;

		[Header("Object")]
		[SerializeField] private GameObject[] players;
		[SerializeField] private GameObject dinosaur;

		[Header("Time")]
		[SerializeField] private float roundTimeLimit;

		[Header("Distance")]
		[SerializeField] private float totalRunningDistance = 100.0f;
		[SerializeField] private float minDistance = 5.0f;
		[SerializeField] private float maxDistance = 15.0f;
		[SerializeField] private float playerSpacing = 2.0f;

		[Header("ProgressRate")]
		[SerializeField] private float tailMissionStartRate = 15.0f;
		// TODO<이종진> - 진행률에 따른 미션 이름 수정 필요 - 20241110
		[SerializeField] private float firstMissionRate = 35.0f;
		[SerializeField] private float secondMissionRate = 55.0f;
		[SerializeField] private float thirdMissionRate = 80.0f;

        [Header("Virtual Camera")] 
		[SerializeField] private GameObject runningViewCam;

		[HideInInspector] public bool isMissionSuccess = false;

		private int life = 3;

		private bool isRunning = true;
        private bool isPossibleTailMission = false;
		private bool isFirstMissionCompleted = false;
		private bool isSecondMissionCompleted = false;
		private bool isThirdMissionCompleted = false;

		private float[] playerDistance = { 0.0f, 0.0f, 0.0f, 0.0f };
		private float dinosaurDistance = 0.0f;
		private Vector3[] prevPlayerPosition = new Vector3[4];
		private Vector3 prevDinosaurPosition;
		private float firstRankerDistance = 0.0f;
		private float lastRankerDistance = 0.0f;

        public float FirstRankerDistance { get { return firstRankerDistance; } }

        public float ProgressRate { get { return firstRankerDistance / totalRunningDistance * 100.0f; } }

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
        }
        public void EnterState()
		{
            dinosaurSpeed = dinosaur.GetComponent<DinosaurController>().Speed;
            runningViewCam.GetComponent<CinemachineVirtualCamera>().Priority = 20;

			if (dinosaurDistance <= 1e-3)
				return;

			SetInfo();
			isRunning = true;
        }
		public void UpdateState()
		{
			roundTimeLimit -= Time.deltaTime;

			UpdateUI();

            if (!isPossibleTailMission && ProgressRate > tailMissionStartRate)
				isPossibleTailMission = true;
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
        }
		private void Move()
		{
			transform.position = Vector3.forward * firstRankerDistance;
		}

		#region 씬 전환시 플레이어, 공룡 정보 Setting
		private void InitPlayerPos()
		{
			float offset = 1.0f * (players.Length - 1) / 2.0f * playerSpacing;

			for (int playerNum = 0; playerNum < players.Length; playerNum++)
				prevPlayerPosition[playerNum] = new Vector3(offset + playerNum * -playerSpacing, 0.0f, 0.0f);
		}
		private void SetInfo()
		{
			for(int playerNum = 0; playerNum < players.Length; playerNum++)
				players[playerNum].transform.position 
					= new Vector3(prevPlayerPosition[playerNum].x, prevPlayerPosition[playerNum].y, prevPlayerPosition[playerNum].z);
			dinosaur.transform.position = prevDinosaurPosition;

		}
		private void SaveInfo()
		{
			for (int playerNum = 0; playerNum < players.Length; playerNum++)
				prevPlayerPosition[playerNum] =
					new Vector3(players[playerNum].transform.position.x, players[playerNum].transform.position.y, players[playerNum].transform.position.z + 5.0f);
			prevDinosaurPosition = dinosaur.transform.position;
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
			firstRankerDistance = playerDistance[0];
			lastRankerDistance = playerDistance[0];

			for (int playerNum = 1; playerNum < players.Length; playerNum++)
			{
				if (playerDistance[playerNum] > firstRankerDistance) firstRankerDistance = playerDistance[playerNum];
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
			if (minDistance <= lastRankerDistance - dinosaurDistance)
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
			((CUIRunningCanvas)UIManager.Instance.CurSceneUI).SetProgressBar(ProgressRate);
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

        #endregion
    }
}