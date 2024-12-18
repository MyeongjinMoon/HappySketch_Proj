using HakSeung;
using Jaehoon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static HakSeung.UIManager;

namespace JongJin
{
    public class TailMissionState : MonoBehaviour, IGameState
    {
        private readonly float ENDTIME = 0.0f;
        private readonly float startWarningBarX = -29.0f;
        private readonly float endWarningBarX = 29.0f;
        private readonly int ATTACKCOUNT = 3;

        [SerializeField] private GameObject dinosaur;
        [SerializeField] private TailContoller tailController;
        [SerializeField] private GameObject []warningEffect;
        [SerializeField] private GameObject []warningExclamation;
        [SerializeField] private RectTransform timingBar;

        [SerializeField] private float warningTime = 3.0f;
        [SerializeField] private float attackDelayTime = 3.0f;

        private float[] dinosaurPosY = { 0.4f, -3.8f };
        private float[] dinosaurRotX = { -16.0f, 12.0f  };

        private int attackCount;
        private float attackTime = 3f;

        private float waitTime = 1.0f;
        private float flowTime = 0.0f;
        private float warningFlowTime = 0.0f;
        private float attackFlowTime = 0.0f;

        private int randomAttackPos = -1;

        private bool isFinish = false;
        private bool isDelayStart = false;
        private bool isDelayFinish = false;
        public void EnterState()
        {
            if (!isDelayStart)
            {
                StartCoroutine(TutorialPopup(10.0f));
            }
            isFinish = false;
            isDelayFinish = false;

            attackCount = ATTACKCOUNT;

            flowTime = 0.0f;
            warningFlowTime = 0.0f;
            attackFlowTime = 0.0f;

            randomAttackPos = -1;
        }
        public void UpdateState()
        {
            if (isDelayFinish || !isDelayStart)
                return;

            flowTime += Time.deltaTime;
            if (flowTime < attackDelayTime)
                return;

            if (randomAttackPos == -1)
            {
                randomAttackPos = UnityEngine.Random.Range(0, 2);
                warningEffect[randomAttackPos].SetActive(true);
                warningExclamation[randomAttackPos].SetActive(true);
            }
            warningFlowTime += Time.deltaTime;
            if (warningFlowTime < warningTime)
                return;
            if (attackFlowTime <= 0.0f)
            {
                warningExclamation[randomAttackPos].SetActive(false);
                dinosaur.SetActive(true);
                dinosaur.transform.eulerAngles = new Vector3(dinosaurRotX[randomAttackPos], dinosaur.transform.eulerAngles.y, dinosaur.transform.eulerAngles.z);
                SoundManager.instance.SFXPlay("Sounds/Tail_Attack");

                timingBar.anchoredPosition
                    = new Vector2(-29.0f, warningEffect[randomAttackPos].GetComponent<RectTransform>().anchoredPosition.y);
                timingBar.gameObject.SetActive(true);
            }
            dinosaur.transform.position = new Vector3(Mathf.Lerp(147.0f, 157.0f + randomAttackPos, attackFlowTime / attackTime), dinosaurPosY[randomAttackPos], dinosaur.transform.position.z);
            timingBar.anchoredPosition = new Vector2(Mathf.Lerp(startWarningBarX, endWarningBarX, attackFlowTime / attackTime * 1.3f), timingBar.anchoredPosition.y);

            attackFlowTime += Time.deltaTime;
            if (attackFlowTime < attackTime)
                return;

            timingBar.gameObject.SetActive(false);
            warningEffect[randomAttackPos].SetActive(false);
            dinosaur.SetActive(false);
            attackCount--;

            flowTime = 0.0f;
            warningFlowTime = 0.0f;
            attackFlowTime = 0.0f;
            randomAttackPos = -1;

            if (tailController.CollisionCount != 0)
            {
                ((CUITailMissionPanel)UIManager.Instance.CurSceneUI).OnFailedEvent(2 - attackCount);
                SoundManager.instance.SFXPlay("Sounds/Tail_Fail");
            }

            if (tailController.CollisionCount == 0 || attackCount == 0)
            {
                isDelayFinish = true;
                StartCoroutine(DelayFinish());
            }
        }

        public void ExitState()
        { 
            UIManager.Instance.SwapSceneUI((int)ESceneUIType.RunningCanvas);

            dinosaur.SetActive(false);

            flowTime = 0.0f;
            warningFlowTime = 0.0f;
            attackFlowTime = 0.0f;
            randomAttackPos = -1;
        }

        private IEnumerator TutorialPopup(float setTime)
        {
            isDelayStart = false;
            UIManager.Instance.SwapPopupUI(UIManager.EPopupUIType.TutorialPopupPanel.ToString());
            ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).ImageSwap(EGameState.TAILMISSION);

            yield return new WaitForSeconds(waitTime);

            while (setTime > ENDTIME)
            {
                ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).TimerUpdate(setTime);
                setTime -= Time.deltaTime;
                yield return null;
            }

            ((CUITutorialPopup)UIManager.Instance.CurrentPopupUI).TimerUpdate(ENDTIME);

            UIManager.Instance.CloseAllPopupUI();
            yield return new WaitForSeconds(waitTime);

            isDelayStart = true;
        }
        public bool IsFinishMission(out bool success)
        {
            success = false;
            if (isFinish)
            {
                success = true;
                if (attackCount <= 0)
                    success = false;
                return true;
            }
            return false;
        }
        IEnumerator DelayFinish()
        {
            yield return new WaitForSeconds(3.0f);
            isFinish = true;
        }
    }
}
