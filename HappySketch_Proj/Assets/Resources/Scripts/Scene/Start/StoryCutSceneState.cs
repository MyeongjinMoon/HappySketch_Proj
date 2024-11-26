using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace JongJin
{
    public class StoryCutSceneState : MonoBehaviour, IGameState
    {
        [SerializeField] private PlayableDirector timelineDirector;
        private bool isFinish = false;
        public void EnterState()
        {
            isFinish = false;
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
            isFinish = true;
        }
        public bool IsFinishedStoryCutScene()
        {
            return isFinish;
        }
    }
}
