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

        [HideInInspector] public bool isGameSuccess;
        [HideInInspector] public int topPlayerIndex;
        [HideInInspector] public float player1Time;
        [HideInInspector] public float player2Time;

        [SerializeField] private AudioClip successSound;
        [SerializeField] private AudioClip failSound;

        private void Awake()
		{
            //UI Ä³½Ì
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
                SoundManager.instance.BackgroundMusicPlay(successSound);
            else
                SoundManager.instance.BackgroundMusicPlay(failSound);
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