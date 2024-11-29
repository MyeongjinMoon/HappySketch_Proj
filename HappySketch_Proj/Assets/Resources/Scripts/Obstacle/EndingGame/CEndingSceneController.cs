using HakSeung;
using Jaehoon;
using JongJin;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MyeongJin
{
	public class CEndingSceneController : MonoBehaviour
	{
		[Header("EndingScene States")]
		[SerializeField] private CEndingEnterState cEndingEnterState;
		[SerializeField] private CEndingAnimationState cEndingAnimationState;
		[SerializeField] private CEndingResultState cEndingResultState;

		private CEndingStateContext cEndingStateContext;

		public EEndingGameState curState;

        // GameScene에서 isGameSuccess를 판단할 수 있는 게임 승리 변수를 정해줘야 함.
        [HideInInspector] public bool isGameSuccess;

        // GameScene에서 topPlayerIndex 판단할 수 있는 1등 플레이어 변수를 정해줘야 함.
        [HideInInspector] public int topPlayerIndex;
        [HideInInspector] public float player1Time;
        [HideInInspector] public float player2Time;

        private void Awake()
		{
            //UI 캐싱
            UIManager.Instance.MainCanvasSetting();

            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.EPopupUIType), (int)UIManager.EPopupUIType.FadePopupCanvas);
			UIManager.Instance.UICashing<GameObject>(typeof(UIManager.EPopupUIType), (int)UIManager.EPopupUIType.EndingPopupPanel);

            cEndingEnterState = GetComponent<CEndingEnterState>();
            cEndingEnterState.isGameSuccess = isGameSuccess;
            cEndingAnimationState = GetComponent<CEndingAnimationState>();
            cEndingAnimationState.isGameSuccess = isGameSuccess;
            cEndingResultState = GetComponent<CEndingResultState>();
            cEndingResultState.isGameSuccess = isGameSuccess;

            cEndingStateContext = new CEndingStateContext(this);
			cEndingStateContext.Transition(cEndingEnterState);
			curState = EEndingGameState.ENTERSCENE;

            isGameSuccess = Convert.ToBoolean(PlayerPrefs.GetInt("ClearStage"));
            player1Time = PlayerPrefs.GetFloat("Player1Time");
            player2Time = PlayerPrefs.GetFloat("Player2Time");

            topPlayerIndex = (player1Time < player2Time) ? 0 : 1;
        }
        private void Start()
        {
            if (isGameSuccess)
                SoundManager.instance.SFXPlay("Sounds/GameClear");
            else
                SoundManager.instance.SFXPlay("Sounds/GameFail");
        }
        private void Update()
		{
            switch (curState)
            {
                case EEndingGameState.ENTERSCENE:
                    if (cEndingEnterState.IsFinishCurState())
                        UpdateState(EEndingGameState.ANIMATION);
                    break;
                case EEndingGameState.ANIMATION:
                    if (cEndingAnimationState.IsFinishCurState())
                        UpdateState(EEndingGameState.RESULT);
                    break;
                case EEndingGameState.RESULT:
                    //if (cEndingEnterState.IsFinishedTutorialPopup())
                    //    UpdateState(EEndingGameState.TUTORIALACTION);
                    break;
            }
            cEndingStateContext.CurrentState.UpdateState();
        }
        private void UpdateState(EEndingGameState nextState)
        {
            if (curState == nextState)
                return;
            curState = nextState;

            switch (curState)
            {
                case EEndingGameState.ENTERSCENE:
                    cEndingStateContext.Transition(cEndingEnterState);
                    break;
                case EEndingGameState.ANIMATION:
                    cEndingStateContext.Transition(cEndingAnimationState);
                    break;
                case EEndingGameState.RESULT:
                    cEndingStateContext.Transition(cEndingResultState);
                    break;
            }
        }
    }
}