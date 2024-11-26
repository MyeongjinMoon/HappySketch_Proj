using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JongJin
{
    public class TutorialActionState : MonoBehaviour, IGameState
    {
        private const int STARTTIME = 10;
        private const int ENDTIME = 0;
        private int timerTime;

        public void EnterState()
        {
            timerTime = STARTTIME;
        }
        public void UpdateState()
        {

        }

        public void ExitState()
        {
        }

        private IEnumerator Timer()
        {
            while (timerTime > ENDTIME)
            {

            }

            timerTime = ENDTIME;

            yield return null;
        }
    }
}
