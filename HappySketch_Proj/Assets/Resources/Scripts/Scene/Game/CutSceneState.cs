using HakSeung;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace JongJin
{
    public class CutSceneState : MonoBehaviour, IGameState
    {
        [SerializeField] private PlayableDirector timelineDirector;
        [SerializeField] private Fade fade;

        public void EnterState()
        {
            fade.FadeIn();
            UIManager.Instance.CurSceneUI.Hide();
            StartCoroutine(SetFinish());
        }
        public void UpdateState()
        {
        }

        public void ExitState()
        {
            UIManager.Instance.CurSceneUI.Show();
        }

        IEnumerator SetFinish()
        {
            yield return new WaitForSeconds((float)timelineDirector.duration - 1.0f);
            fade.FadeInOut();
            yield return new WaitForSeconds(1.0f);
        }
        public bool IsFinishedCutScene()
        {
            return fade.IsFinished;
        }
    }
}
