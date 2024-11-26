using HakSeung;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HakSeung.UIManager;

namespace JongJin
{
    public class SecondMissionState : MonoBehaviour, IGameState
    {
        private bool isSuccess = false;
        private bool isMissionFinished = false;
        private bool isWait = false;
        private float timer = 60f;

        public void EnterState()
        {
            isSuccess = false;
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
        }
        private void CheckProgressBar()
        {
            if (((CUIEventPanel)UIManager.Instance.CurSceneUI).progressBar.isProgressBarFullFilled)
                isSuccess = true;
        }
        private IEnumerator Stay()
        {
            isWait = true;
            yield return new WaitForSeconds(3.0f);
            isMissionFinished = true;
        }
        private void SetTimer()
        {
            ((CUIEventPanel)UIManager.Instance.CurSceneUI).SetTimer(timer);
        }
    }
}