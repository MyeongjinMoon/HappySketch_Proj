using HakSeung;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static HakSeung.UIManager;

namespace JongJin
{
    public class TutorialDescriptionState : MonoBehaviour, IGameState
    {
        [SerializeField] private GameObject[] players;
        [SerializeField] private Image image;

        private float time = 0.0f;
        private float fTime = 1.0f;

        private const int STARTTIME = 2; 
        private const int ENDTIME = 0;

        private CUITutorialPopup.TutorialState tutorialState = CUITutorialPopup.TutorialState.STORY;
        private bool isPopupTime;
        private bool isFinished;
        
        public void EnterState()
        {
            if (tutorialState == CUITutorialPopup.TutorialState.HEART)
                return;

            StartCoroutine(TutorialStart());
            
            switch(tutorialState)
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
                case CUITutorialPopup.TutorialState.HEART:
                    tutorialState = CUITutorialPopup.TutorialState.STORY;
                    break;
            }

            isPopupTime = false;
            isFinished = false;
        }
        public void UpdateState()
        {
            if (tutorialState == CUITutorialPopup.TutorialState.STORY)
                return;

            if (isPopupTime != true)
            {
                ShowTutorialPopup(tutorialState);
                if (UIManager.Instance.CurrentPopupUI != null)
                    StartCoroutine(PopupTimer(STARTTIME));
            }
        }

        public void ExitState()
        {
            isPopupTime = false;

            UIManager.Instance.CloseAllPopupUI();
        }

        IEnumerator TutorialStart()
        {
            yield return new WaitForSeconds(fTime);
            for (int playerNum = 0; playerNum < players.Length; playerNum++)
                players[playerNum].SetActive(true);

            while (image.color.a > 0.0f)
            {
                time += Time.deltaTime / fTime;
                image.color = new Color(0.0f, 0.0f, 0.0f, Mathf.Lerp(1.0f, 0.0f, time));
                yield return null;
            }
        }

        private IEnumerator PopupTimer(float setTime)
        {
            Debug.Log("타이머 작동중");
            yield return new WaitForSeconds(fTime * 0.5f);

            while (setTime > ENDTIME)
            {
                ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).TimerUpdate(setTime);
                setTime -= Time.deltaTime;
                yield return null;
            }

            ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).TimerUpdate(ENDTIME);

            yield return new WaitForSeconds(fTime);

            isFinished = true;
        }

        private void ShowTutorialPopup(CUITutorialPopup.TutorialState tutorialPopupState)
        {
            isPopupTime = true;
            switch (tutorialPopupState)
            {
                case CUITutorialPopup.TutorialState.RUNNING:
                    UIManager.Instance.ShowPopupUI(EPopupUIType.TutorialPopupPanel.ToString());
                    ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).ImageSwap(CUITutorialPopup.TutorialState.RUNNING);
                    break;
                case CUITutorialPopup.TutorialState.JUMP:
                    UIManager.Instance.ShowPopupUI(EPopupUIType.TutorialPopupPanel.ToString());
                    ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).ImageSwap(CUITutorialPopup.TutorialState.JUMP);
                    break;
                case CUITutorialPopup.TutorialState.HEART:
                    UIManager.Instance.ShowPopupUI(EPopupUIType.TutorialPopupPanel.ToString());
                    ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).ImageSwap(CUITutorialPopup.TutorialState.HEART);

                    break;
                default:

                    break;

            }
        }

        public bool IsFinishedTutorialPopup()
        {
            return isFinished;
        }


    }
}
