using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JongJin
{
    public class TutorialDescriptionState : MonoBehaviour, IGameState
    {
        [SerializeField] private GameObject[] players;
        [SerializeField] private Image image;

        private float time = 0.0f;
        private float fTime = 1.0f;
        int curCount = 0;
        public void EnterState()
        {
            StartCoroutine(TutorialStart());
        }
        public void UpdateState()
        {
        }

        public void ExitState()
        {
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
    }
}
