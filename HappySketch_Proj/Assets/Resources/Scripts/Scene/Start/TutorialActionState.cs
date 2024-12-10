using HakSeung;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static HakSeung.UIManager;

namespace JongJin
{
    public class TutorialActionState : MonoBehaviour, IGameState
    {
        private const int TOTALPLAYERNUM = 2;
        public CUITutorialPopup.TutorialState CurrentTutorialState { get { return tutorialState; } }
        [SerializeField]private TutorialPlayerController[] playerController = new TutorialPlayerController[TOTALPLAYERNUM];

        private CUITutorialPopup.TutorialState tutorialState = CUITutorialPopup.TutorialState.STORY;

        private bool isActionConditionClear;

        private float successWaitTime = 3f;

        private const int MAXACTIONCOUNT = 3;
      
        private int[] actionSuccessCounts = new int[TOTALPLAYERNUM];
        private float[] currentRunningTimes = new float[TOTALPLAYERNUM];

        private float runningStartTime = 0;

        private float runningSuccessSpeed = 5f;

        private bool[] prevPlayerActionTrigger = new bool[TOTALPLAYERNUM];

        private bool isOnSuccess;

        public void EnterState()
        {
            isOnSuccess = false;

            for (int playerIndex = 0; playerIndex < TOTALPLAYERNUM; playerIndex++)
                actionSuccessCounts[playerIndex] = 0;

            switch (tutorialState)
            {
                case CUITutorialPopup.TutorialState.STORY:
                    tutorialState = CUITutorialPopup.TutorialState.RUNNING;
                    
                    for (int playerIndex = 0; playerIndex < playerController.Length; playerIndex++ )
                        currentRunningTimes[playerIndex] = runningStartTime;

                    break;
                case CUITutorialPopup.TutorialState.RUNNING:
                    tutorialState = CUITutorialPopup.TutorialState.JUMP;
                    break;
                case CUITutorialPopup.TutorialState.JUMP:
                    tutorialState = CUITutorialPopup.TutorialState.HEART;
                    break;
            }

            isActionConditionClear = false;

            for (int playerIndex = 0; playerIndex < playerController.Length; playerIndex++)
            {
                playerController[playerIndex].PlayerReset(tutorialState);
                prevPlayerActionTrigger[playerIndex] = false;
            }
            
            Debug.Log(tutorialState.ToString() + "튜토리얼");
        }
        public void UpdateState()
        {
            if (isOnSuccess)
                return;

            if (CurrentTutorialState != CUITutorialPopup.TutorialState.HEART)
            {
                bool isP1ActionTrue = PlayerActionCheak(0);
                bool isP2ActionTrue = PlayerActionCheak(1);

                if ((isP1ActionTrue && isP2ActionTrue))
                    StartCoroutine(ActionSuccessTimer());
            }
            else if ((CurrentTutorialState == CUITutorialPopup.TutorialState.HEART) && SceneManagerExtended.Instance.CheckReady())
                StartCoroutine(ActionSuccessTimer());
        }

        public void ExitState()
        {
     

        }

        // �ӽ�
        private bool CurrentTutorialActionCheck()
        {
            return true;
        }

        public bool IsFinishedAction()
        {

            return isActionConditionClear;
        }

        private bool PlayerActionCheak(int playerNum)
        {
            
            switch (tutorialState)
            {
               case CUITutorialPopup.TutorialState.RUNNING:
                    if (actionSuccessCounts[playerNum] >= MAXACTIONCOUNT)
                        return true;

                    if (playerController[playerNum].Speed >= runningSuccessSpeed)
                        currentRunningTimes[playerNum] += Time.deltaTime;
                    else
                        currentRunningTimes[playerNum] = 0;

                    actionSuccessCounts[playerNum] = (int)currentRunningTimes[playerNum];

                    break;
               case CUITutorialPopup.TutorialState.JUMP:
                    if (actionSuccessCounts[playerNum] >= MAXACTIONCOUNT)
                        return true;

                    if (playerController[playerNum].ActionTrigger && (prevPlayerActionTrigger[playerNum] != playerController[playerNum].ActionTrigger))
                    {
                        ++actionSuccessCounts[playerNum];
                        Debug.Log(playerNum + "점프 중");
                    }


                    prevPlayerActionTrigger[playerNum] = playerController[playerNum].ActionTrigger;

                    

                    break;
                case CUITutorialPopup.TutorialState.HEART:
                    

                    break;
            }

            ((CUITutorialPanel)UIManager.Instance.CurSceneUI).ActionCountSet(playerNum, 3 - actionSuccessCounts[playerNum]);

            return false;
        }
        private IEnumerator ActionSuccessTimer()
        {
            isOnSuccess = true;
            UIManager.Instance.ShowPopupUI(UIManager.EPopupUIType.TutorialPopupPanel.ToString());
            ((CUITutorialPopup)(UIManager.Instance.CurrentPopupUI)).ImageSwap(CUITutorialPopup.EventResult.SUCCESS);
            ((CUITutorialPopup)(UIManager.Instance.CurrentPopupUI)).TimerHide();

            //Success ǥ�� ���� 
            yield return new WaitForSeconds(successWaitTime);
            isActionConditionClear = true;
            
           /* else 
            {
               if (SceneManagerExtended.Instance.CheckReady())
                    StartCoroutine(SceneManagerExtended.Instance.GoToGameScene());
            }*/


            yield return null;
        }


    }
}
