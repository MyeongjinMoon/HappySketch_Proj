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
		[SerializeField] private GameObject[] endingResultPlayers;
		[SerializeField] private GameObject[] crown;
		[SerializeField] private RuntimeAnimatorController[] playerController;
		[SerializeField] private RuntimeAnimatorController[] playerResultController;
		[SerializeField] private GameObject successEndingObject;
		[SerializeField] private GameObject failedEndingObject;
		[SerializeField] private Fade fade;

		private Animator dinosaurAnimator;
		private Animator[] endingPlayersAnimators;
		private Animator[] endingResultPlayersAnimators;
		private bool isEnabledResultPlayers = false;
		private bool isFadeStart = false;

		public override void EnterState()
		{
			if (isGameSuccess)
			{
				endingPlayersAnimators = new Animator[endingPlayers.Length];
				endingResultPlayersAnimators = new Animator[endingResultPlayers.Length];

				for (int i = 0; i < endingPlayers.Length; i++)
				{
					if (i == topPlayerIndex)
					{
						endingPlayers[i].GetComponent<Animator>().runtimeAnimatorController = playerController[0];
						endingResultPlayers[i].GetComponent<Animator>().runtimeAnimatorController = playerResultController[0];
						crown[i].SetActive(true);
                    }
					else
					{
						endingPlayers[i].GetComponent<Animator>().runtimeAnimatorController = playerController[1];
						endingResultPlayers[i].GetComponent<Animator>().runtimeAnimatorController = playerResultController[1];
					}
					endingPlayersAnimators[i] = endingPlayers[i].GetComponent<Animator>();
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
				if (!endingPlayersAnimators[0].GetCurrentAnimatorStateInfo(0).IsName("HumanSlowSprint") && (endingPlayersAnimators[0].speed <= 0))
				{
					if (!isFadeStart)
					{
						isFadeStart = true;

                        fade.FadeInOut();
					}
                    if (fade.IsFinished && !endingResultPlayers[0].activeSelf)
                    {
						for (int i = 0; i < endingResultPlayers.Length; i++)
						{
							endingPlayers[i].SetActive(false);
                            endingResultPlayers[i].SetActive(true);
                        }

                        StartCoroutine(Stay(4f));
                    }
                }
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
		private IEnumerator Stay(float time)
		{
			while (time > 0)
			{
				time-=Time.deltaTime;
				yield return null;
            }
            isFinish = true;
        }
		public bool IsFinishedStoryCutScene()
		{
			return isFinish;
		}
	}
}