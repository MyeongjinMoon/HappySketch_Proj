using HakSeung;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HakSeung.UIManager;

namespace JongJin
{
    public class TutorialActionState : MonoBehaviour, IGameState
    {
        public CUITutorialPopup.TutorialState CurrentTutorialState { get { return tutorialState; } }
        [SerializeField]private TutorialPlayerController[] playerController = new TutorialPlayerController[2];

        private CUITutorialPopup.TutorialState tutorialState = CUITutorialPopup.TutorialState.STORY;

        private bool isActionConditionClear;

        private float successWaitTime = 2f;

        private const int MAXACTIONCOUNT = 3;
      
        private int[] actionSuccessCounts;
        private float[] currentRunningTimes;

        private float runningStartTime = 5;

        private float runningSuccessSpeed = 5f;

        private bool prevPlayerActionTrigger;

        public void EnterState()
        {
            actionSuccessCounts = new int[playerController.Length];

            for(int playerIndex = 0; playerIndex < playerController.Length; playerIndex++)
                actionSuccessCounts[playerIndex] = 0;

            switch (tutorialState)
            {
                case CUITutorialPopup.TutorialState.STORY:
                    tutorialState = CUITutorialPopup.TutorialState.RUNNING;

                    currentRunningTimes = new float[playerController.Length];
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
            prevPlayerActionTrigger = false;

            for (int playerIndex = 0; playerIndex < playerController.Length; playerIndex++)
                playerController[playerIndex].PlayerReset(tutorialState);

            
            Debug.Log(tutorialState.ToString() + "튜토리얼");
        }
        public void UpdateState()
        {
            if(PlayerActionCheak(0) && PlayerActionCheak(1))
                StartCoroutine(ActionSuccessTimer());
        }

        public void ExitState()
        {
            if (tutorialState == CUITutorialPopup.TutorialState.HEART)
            {
                for (int playerIndex = 0; playerIndex < playerController.Length; playerIndex++)
                    SceneManagerExtended.Instance.SetReady(playerIndex, true);
                if (SceneManagerExtended.Instance.CheckReady())
                    StartCoroutine(SceneManagerExtended.Instance.GoToGameScene());
            } 

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

                    if (playerController[playerNum].ActionTrigger && !prevPlayerActionTrigger)
                    {
                        ++actionSuccessCounts[playerNum];
                        Debug.Log("���� �Ǵ� ��");
                    }

                    if (prevPlayerActionTrigger != playerController[playerNum].ActionTrigger)
                        prevPlayerActionTrigger = playerController[playerNum].ActionTrigger;

                    break;
                case CUITutorialPopup.TutorialState.HEART:
                    if (playerController[playerNum].ActionTrigger && !prevPlayerActionTrigger)
                        return true;

                    break;
            }

            return false;
        }
        private IEnumerator ActionSuccessTimer()
        {
            //Success ǥ�� ���� 
            if (tutorialState != CUITutorialPopup.TutorialState.HEART)
            {
                yield return new WaitForSeconds(successWaitTime);
                isActionConditionClear = true;
            }
            else 
            {

                for (int playerIndex = 0; playerIndex < playerController.Length; playerIndex++)
                {
                    yield return null;
                    SceneManagerExtended.Instance.SetReady(playerIndex, true);
                }
               if (SceneManagerExtended.Instance.CheckReady())
                    StartCoroutine(SceneManagerExtended.Instance.GoToGameScene());
             
              }


            yield return null;
        }


    }
}
