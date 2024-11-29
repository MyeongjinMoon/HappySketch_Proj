using HakSeung;
using MyeongJin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static HakSeung.UIManager;

namespace JongJin
{
    public class SecondMissionState : MonoBehaviour, IGameState
    {
        private CSpawnController spawnController;
        private bool isSuccess = false;
        private bool isMissionFinished = false;
        private bool isWait = false;
        private float timer = 60f;

        private readonly float ENDTIME = 0.0f;
        private float waitTime = 1.0f;
        private bool isDelayStart = false;

        private void Awake()
        {
            spawnController = GameObject.Find("SpawnController").GetComponent<CSpawnController>();
        }
        public void EnterState()
        {
            StartCoroutine(TutorialPopup(10.0f));

            isSuccess = false;
            timer = 1f;
        }
        public void UpdateState()
        {
            if (!isDelayStart)
                return;

            DecreaseTime();

            SetTimer();
            CheckProgressBar();
        }

        public void ExitState()
        {
            ((CUIEventPanel)UIManager.Instance.CurSceneUI).progressBar.Init();
            UIManager.Instance.SceneUISwap((int)ESceneUIType.RunningCanvas);

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
                    StartCoroutine("Stay");
                success = true;
                return isMissionFinished;
            }
            if (timer <= 0)
            {
                if (!isWait)
                    StartCoroutine("Stay");
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
        private IEnumerator Stay()
        {
            isWait = true;
            spawnController.canSpawn = false;
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
            ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).ImageSwap(EGameState.SECONDMISSION);

            while (setTime > ENDTIME)
            {
                ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).TimerUpdate(setTime);
                setTime -= Time.deltaTime;
                yield return null;
            }

            ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).TimerUpdate(ENDTIME);

            UIManager.Instance.CloseAllPopupUI();
            yield return new WaitForSeconds(waitTime);

            isDelayStart = true;
        }
    }
}