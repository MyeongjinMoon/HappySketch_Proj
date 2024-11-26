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
            StartCoroutine(SetFinish());
        }
        public void UpdateState()
        {
        }

        public void ExitState()
        {
        }

        IEnumerator SetFinish()
        {
            yield return new WaitForSeconds((float)timelineDirector.duration - 1f);
            fade.FadeInOut();
        }
        public bool IsFinishedCutScene()
        {
            return fade.IsFinished;
        }
    }
}
