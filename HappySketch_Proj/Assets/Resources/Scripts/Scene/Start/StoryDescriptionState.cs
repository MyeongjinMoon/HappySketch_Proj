using HakSeung;
using Jaehoon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JongJin
{
    public class StoryDescriptionState : MonoBehaviour, IGameState
    {
        [SerializeField] private Fade fade;
        public void EnterState()
        {
            UIManager.Instance.ShowPopupUI(UIManager.ETestType.TutorialPopupPanel.ToString());
            ((CUITutorialPopup)(UIManager.Instance.CurrentPopupUI)).ImageSwap(CUITutorialPopup.TutorialState.STORY);
            ((CUITutorialPopup)(UIManager.Instance.CurrentPopupUI)).TimerHide();

            StartCoroutine(ProgressDescription());
        }
        public void UpdateState()
        {
        }

        public void ExitState()
        {

        }
        IEnumerator ProgressDescription()
        {
            yield return new WaitForSeconds(7.0f);
            UIManager.Instance.ClosePopupUI();
            fade.FadeInOut();
        }
        public bool IsStroyDescriptionFinish()
        {
            return fade.IsFinished;
        }
    }
}
