using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace JongJin
{
    public class StoryCutSceneState : MonoBehaviour, IGameState
    {
        [SerializeField] private PlayableDirector timelineDirector;
        [SerializeField] private GameObject[] storyPlayers;
        [SerializeField] private GameObject storyCutScene;

        private bool isFinish = false;
        public void EnterState()
        {
            storyCutScene.SetActive(true);

            isFinish = false;
            StartCoroutine(SetFinish());
        }
        public void UpdateState()
        {
        }

        public void ExitState()
        {
            for (int playerNum = 0; playerNum < storyPlayers.Length; playerNum++)
                storyPlayers[playerNum].SetActive(false);
        }
        IEnumerator SetFinish()
        {
            yield return new WaitForSeconds((float)timelineDirector.duration);
            isFinish = true;
        }
        public bool IsFinishedStoryCutScene()
        {
            return isFinish;
        }
    }
}
