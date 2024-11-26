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

        private const int STARTTIME = 10;
        private const int ENDTIME = 0;
        
        private int tutorialStateIndex = 0;
        private bool isPopupTime;
        private bool isFinished;
        
        public void EnterState()
        {
            StartCoroutine(TutorialStart());
            tutorialStateIndex = (int)CUITutorialPopup.TutorialState.RUNNING;
            isPopupTime = false;
            isFinished = false;
        }
        public void UpdateState()
        {
            if (isPopupTime != true)
            {
                ShowTutorialPopup(tutorialStateIndex);
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

        private void ShowTutorialPopup(int popupIndex)
        {
            isPopupTime = true;
            switch (popupIndex)
            {
                case (int)CUITutorialPopup.TutorialState.RUNNING:
                    UIManager.Instance.ShowPopupUI(EPopupUIType.TutorialPopupPanel.ToString());
                    ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).ImageSwap(CUITutorialPopup.TutorialState.RUNNING);
                    break;
                case (int)CUITutorialPopup.TutorialState.JUMP:
                    UIManager.Instance.ShowPopupUI(EPopupUIType.TutorialPopupPanel.ToString());
                    ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).ImageSwap(CUITutorialPopup.TutorialState.JUMP);

                    break;
                case (int)CUITutorialPopup.TutorialState.HEART:
                    UIManager.Instance.ShowPopupUI(EPopupUIType.TutorialPopupPanel.ToString());
                    ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).ImageSwap(CUITutorialPopup.TutorialState.HEART);

                    break;
            }
        }

        public bool IsFinishedTutorialPopup()
        {
            return isFinished;
        }


    }
}
