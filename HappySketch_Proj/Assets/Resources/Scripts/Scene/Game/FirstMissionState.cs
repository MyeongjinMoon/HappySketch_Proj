using HakSeung;
using Jaehoon;
using MyeongJin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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

        private readonly float ENDTIME = 0.0f;
        private float waitTime = 1.0f;
        private bool isDelayStart = false;

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
            StartCoroutine(TutorialPopup(10.0f));

            isSuccess = false;
			player1.transform.localScale = player1.transform.localScale * 1.5f;
			player2.transform.localScale = player2.transform.localScale * 1.5f;
		}
		public void UpdateState()
		{
            if (!isDelayStart)
                return;

			if (spawnController.canSpawn)
				DecreaseTime();

			SetTimer();
			CheckProgressBar();
		}

		public void ExitState()
		{
			((CUIEventPanel)UIManager.Instance.CurSceneUI).progressBar.Init();
			//UIManager.Instance.SceneUISwap((int)ESceneUIType.RunningCanvas);
			player1.transform.localScale = player1Scale;
			player2.transform.localScale = player2Scale;
            spawnController.canSpawn = true;

            UIManager.Instance.ClosePopupUI();
            UIManager.Instance.SceneUISwap((int)ESceneUIType.RunningCanvas);
        }
		public bool IsFinishMission(out bool success)
		{
			success = false;
			if (isSuccess)
			{
				if (!isWait)
				{
					SoundManager.instance.SFXPlay("Sounds/MissionSuccess");
					StartCoroutine(Stay(isSuccess));
				}
				success = true;
				return isMissionFinished;
			}
			if (timer <= 0)
			{
				if (!isWait)
                {
                    SoundManager.instance.SFXPlay("Sounds/MissionFail");
                    StartCoroutine(Stay(isSuccess));
				}
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
        private IEnumerator TutorialPopup(float setTime)
        {
            isDelayStart = false;
            UIManager.Instance.SwapPopupUI(UIManager.EPopupUIType.TutorialPopupPanel.ToString());
            ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).ImageSwap(EGameState.FIRSTMISSION);

            while (setTime > ENDTIME)
            {
                ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).TimerUpdate(setTime);
                setTime -= Time.deltaTime;
                yield return null;
            }

            ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).TimerUpdate(ENDTIME);

            UIManager.Instance.CloseAllPopupUI();
            ((CUIEventPanel)UIManager.Instance.CurSceneUI).SetTimer(timer);

            yield return new WaitForSeconds(waitTime);

            isDelayStart = true;
        }
    }
}