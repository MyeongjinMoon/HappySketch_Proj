using HakSeung;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HakSeung.UIManager;

namespace JongJin
{
    public class TutorialActionState : MonoBehaviour, IGameState
    {
        private const float RUNNINGDURATIONTIME = 5f;
        private float currentRunningTime;

        private const int MAXACTIONCOUNT = 5;
        private int currentActionCount;

        private CUITutorialPopup.TutorialState tutorialState = CUITutorialPopup.TutorialState.STORY;

        private bool isActionConditionClear;

        public void EnterState()
        {
            currentRunningTime = 0;
            currentActionCount = 0;

            switch (tutorialState)
            {
                case CUITutorialPopup.TutorialState.STORY:
                    tutorialState = CUITutorialPopup.TutorialState.RUNNING;
                    break;
                case CUITutorialPopup.TutorialState.RUNNING:
                    tutorialState = CUITutorialPopup.TutorialState.JUMP;
                    break;
                case CUITutorialPopup.TutorialState.JUMP:
                    tutorialState = CUITutorialPopup.TutorialState.HEART;
                    break;
            }

            isActionConditionClear = false;
        }
        public void UpdateState()
        {
                
        }

        public void ExitState()
        {

        }

        // юс╫ц
        private bool CurrentTutorialActionCheck()
        {
            return true;
        }

        public bool IsFinishedAction()
        {
            return isActionConditionClear;
        }

  



    }
}
