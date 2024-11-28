using JongJin;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace MyeongJin
{
    public class CEndingAnimationState : CBaseEndingState
    {
        [SerializeField] private GameObject dinosaur;
        [SerializeField] private GameObject[] endingPlayers;
        [SerializeField] private RuntimeAnimatorController[] playerController;
        [SerializeField] private GameObject successEndingObject;
        [SerializeField] private GameObject failedEndingObject;

        private Animator dinosaurAnimator;

        public override void EnterState()
        {
            if (isGameSuccess)
            {
                for (int i = 0; i < endingPlayers.Length; i++)
                {
                    if (i == topPlayerIndex)
                        endingPlayers[i].GetComponent<Animator>().runtimeAnimatorController = playerController[0];
                    else
                        endingPlayers[i].GetComponent<Animator>().runtimeAnimatorController = playerController[1];
                }

                successEndingObject.SetActive(true);
            }
            else
            {
                failedEndingObject.SetActive(true);

                dinosaurAnimator = dinosaur.GetComponent<Animator>();
            }

            isFinish = false;
        }
        public override void UpdateState()
        {
            if(isGameSuccess)
            {

            }
            else
            {
                if(dinosaurAnimator.GetCurrentAnimatorStateInfo(0).IsName("Roarning"))
                {
                    if (dinosaurAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                        isFinish = true;
                }
            }
        }
        public bool IsFinishedStoryCutScene()
        {
            return isFinish;
        }
    }
}