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
        protected bool isFinish = false;
        protected bool isGameSuccess;

        private void Awake()
        {
            cEndingSceneController = GameObject.Find("EndingSceneController").GetComponent<CEndingSceneController>();
            isGameSuccess = cEndingSceneController.isGameSuccess;
            topPlayerIndex = cEndingSceneController.topPlayerIndex;
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