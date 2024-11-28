using JongJin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyeongJin
{
    public class CBaseEndingState : MonoBehaviour, IGameState
    {
        private CEndingSceneController cEndingSceneController;

        protected int topPlayerIndex;
        protected float topPlayerTime;
        protected float restPlayerTime;
        protected bool isFinish = false;
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

        public virtual void EnterState()
        {

        }
        public virtual void UpdateState()
        {
        }

        public virtual void ExitState()
        {

        }
        public bool IsFinishCurState()
        {
            return isFinish;
        }
    }
}