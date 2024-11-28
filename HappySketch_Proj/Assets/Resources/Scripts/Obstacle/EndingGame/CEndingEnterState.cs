using HakSeung;
using JongJin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MyeongJin
{
    public class CEndingEnterState : MonoBehaviour, IGameState
    {
        [SerializeField] private Fade fade;

        private CEndingSceneController cEndingSceneController;

        private int topPlayerIndex;
        private float topPlayerTime;
        private float restPlayerTime;
        private bool isFinish = false;
        public bool isGameSuccess;

        private void Awake()
        {
            cEndingSceneController = GameObject.Find("EndingSceneController").GetComponent<CEndingSceneController>();
            isGameSuccess = cEndingSceneController.isGameSuccess;
            topPlayerIndex = cEndingSceneController.topPlayerIndex;

            if (topPlayerIndex == 0)
            {
                topPlayerTime = cEndingSceneController.player1Time;
                restPlayerTime = cEndingSceneController.player2Time;
            }
            else
            {
                topPlayerTime = cEndingSceneController.player2Time;
                restPlayerTime = cEndingSceneController.player1Time;
            }
        }
        public void EnterState()
        {
            UIManager.Instance.ShowPopupUI(UIManager.EPopupUIType.FadePopupCanvas.ToString());

            FadeInOut();
        }
        public virtual void UpdateState()
        {
        }

        public virtual void ExitState()
        {

        }
        private void FadeInOut()
        {
            fade.FadeInOut();
        }
        public bool IsFinishCurState()
        {
            return fade.IsFinished;
        }
    }
}