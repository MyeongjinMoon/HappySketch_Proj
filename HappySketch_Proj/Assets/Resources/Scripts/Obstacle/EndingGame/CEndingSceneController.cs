using HakSeung;
using JongJin;
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

        // GameScene���� isGameSuccess�� �Ǵ��� �� �ִ� ���� �¸� ������ ������� ��.
        [HideInInspector] public bool isGameSuccess;

        // GameScene���� topPlayerIndex �Ǵ��� �� �ִ� 1�� �÷��̾� ������ ������� ��.
        [HideInInspector] public int topPlayerIndex = 0;

		private void Awake()
		{
            //UI ĳ�� 
			UIManager.Instance.UICashing<GameObject>(typeof(UIManager.EPopupUIType), (int)UIManager.EPopupUIType.FadePopupCanvas);
			UIManager.Instance.UICashing<GameObject>(typeof(UIManager.EPopupUIType), (int)UIManager.EPopupUIType.EndingPopupPanel);

            isGameSuccess = true;

            cEndingEnterState = GetComponent<CEndingEnterState>();
            cEndingEnterState.isGameSuccess = isGameSuccess;
            cEndingAnimationState = GetComponent<CEndingAnimationState>();
            cEndingAnimationState.isGameSuccess = isGameSuccess;
            cEndingResultState = GetComponent<CEndingResultState>();
            cEndingResultState.isGameSuccess = isGameSuccess;

            cEndingStateContext = new CEndingStateContext(this);
			cEndingStateContext.Transition(cEndingEnterState);
			curState = EEndingGameState.ENTERSCENE;
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