using HakSeung;
using MyeongJin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HakSeung.UIManager;

namespace JongJin
{
	public class FirstMissionState : MonoBehaviour, IGameState
	{
		private bool isSuccess = false;
		private bool isMissionFinished = false;
		private bool isWait = false;
		private float timer = 60f;

		private GameObject player1;
		private GameObject player2;
		private CSpawnController spawnController;
		private Vector3 player1Scale;
		private Vector3 player2Scale;

		private void Awake()
		{
			player1 = GameObject.FindWithTag("Player1");
			player2 = GameObject.FindWithTag("Player2");
			spawnController = GameObject.Find("SpawnController").GetComponent<CSpawnController>();
            player1Scale = player1.transform.localScale;
			player2Scale = player2.transform.localScale;
		}
		public void EnterState()
		{
			isSuccess = false;
			player1.transform.localScale = player1.transform.localScale * 1.5f;
			player2.transform.localScale = player2.transform.localScale * 1.5f;
			timer = 60f;
		}
		public void UpdateState()
		{
			DecreaseTime();

			SetTimer();
			CheckProgressBar();
		}

		public void ExitState()
		{
			((CUIEventPanel)UIManager.Instance.CurSceneUI).progressBar.Init();
			UIManager.Instance.SceneUISwap((int)ESceneUIType.RunningCanvas);
			player1.transform.localScale = player1Scale;
			player2.transform.localScale = player2Scale;
            spawnController.canSpawn = true;

            UIManager.Instance.ClosePopupUI();
		}
		public bool IsFinishMission(out bool success)
		{
			success = false;
			if (isSuccess)
			{
				if (!isWait)
					StartCoroutine(Stay(isSuccess));
				success = true;
				return isMissionFinished;
			}
			if (timer <= 0)
			{
				if (!isWait)
					StartCoroutine(Stay(isSuccess));
				return isMissionFinished;
			}
			return false;
		}
		private void DecreaseTime()
		{
			timer -= Time.deltaTime;
			if (timer <= 0)
				timer = 0;
		}
		private void CheckProgressBar()
		{
			if (((CUIEventPanel)UIManager.Instance.CurSceneUI).progressBar.isProgressBarFullFilled)
				isSuccess = true;
		}
		private IEnumerator Stay(bool isSuccess)
		{
			spawnController.canSpawn = false;
            isWait = true;

			if (isSuccess)
			{
				UIManager.Instance.ShowPopupUI(UIManager.ETestType.TutorialPopupPanel.ToString());
				((CUITutorialPopup)(UIManager.Instance.CurrentPopupUI)).ImageSwap(CUITutorialPopup.EventResult.SUCCESS);
				((CUITutorialPopup)(UIManager.Instance.CurrentPopupUI)).TimerHide();
			}
			else
			{
				UIManager.Instance.ShowPopupUI(UIManager.ETestType.TutorialPopupPanel.ToString());
				((CUITutorialPopup)(UIManager.Instance.CurrentPopupUI)).ImageSwap(CUITutorialPopup.EventResult.FAILED);
				((CUITutorialPopup)(UIManager.Instance.CurrentPopupUI)).TimerHide();
			}
			//½ÇÆÐ UI¶ç¿ì±â
			yield return new WaitForSeconds(3.0f);
			isMissionFinished = true;
		}
		private void SetTimer()
		{
			((CUIEventPanel)UIManager.Instance.CurSceneUI).SetTimer(timer);
		}
	}
}