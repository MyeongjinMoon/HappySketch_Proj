using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JongJin
{
    public class StoryDescriptionState : MonoBehaviour, IGameState
    {
        private bool isFinish = false;
        public void EnterState()
        {
            isFinish = false;
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
            yield return new WaitForSeconds(10.0f);
            isFinish = true;
        }
        public bool IsStroyDescriptionFinish()
        {
            return isFinish;
        }
    }
}
