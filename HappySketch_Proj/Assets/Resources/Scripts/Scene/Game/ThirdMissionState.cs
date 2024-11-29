using HakSeung;
using Jaehoon;
using MyeongJin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HakSeung.UIManager;

namespace JongJin
{
    public class ThirdMissionState : MonoBehaviour, IGameState
    {
        private CSpawnController spawnController;
        private bool isSuccess = false;
        private bool isMissionFinished = false;
        private bool isWait = false;
        private float timer = 60f;

        private void Awake()
        {
            spawnController = GameObject.Find("SpawnController").GetComponent<CSpawnController>();
        }
        public void EnterState()
        {
            RenderSettings.fog = true;
            isSuccess = false;
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
           // UIManager.Instance.SceneUISwap((int)ESceneUIType.RunningCanvas);
            RenderSettings.fog = false;

            spawnController.canSpawn = true;
            UIManager.Instance.ClosePopupUI();
        }
        public bool IsFinishMission(out bool success)
        {
            success = false;
            if (isSuccess)
            {
                if (!isWait)
                {
                    SoundManager.instance.SFXPlay("Sounds/MissionSuccess");
                    StartCoroutine("Stay");
                }
                success = true;
                return isMissionFinished;
            }
            if (timer <= 0)
            {
                if (!isWait)
                {
                    SoundManager.instance.SFXPlay("Sounds/MissionFail");
                    StartCoroutine("Stay");
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
    }
}
