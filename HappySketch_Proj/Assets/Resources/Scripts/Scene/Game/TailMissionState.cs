using HakSeung;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using static HakSeung.UIManager;

namespace JongJin
{
    public class TailMissionState : MonoBehaviour, IGameState
    {
        // TODO<이종진> - 나중에 레벨 디자인 끝나면 SerializeField 지우기 필요 - 20241121
        private readonly float startWarningBarX = -29.0f;
        private readonly float endWarningBarX = 29.0f;
        [SerializeField] private readonly float dinoPosDiff = 5.0f;
        [SerializeField] private readonly int ATTACKCOUNT = 3;

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

        private float flowTime = 0.0f;
        private float warningFlowTime = 0.0f;
        private float attackFlowTime = 0.0f;

        private int randomAttackPos = -1;

        private bool isSuccess = false;
        public void EnterState()
        {
            isSuccess = false;

            attackCount = ATTACKCOUNT;

            flowTime = 0.0f;
            warningFlowTime = 0.0f;
            attackFlowTime = 0.0f;

            randomAttackPos = -1;
        }
        public void UpdateState()
        {
            flowTime += Time.deltaTime;
            if (flowTime < attackDelayTime)
                return;

            if (randomAttackPos == -1)
            {
                randomAttackPos = Random.Range(0, 2);
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

                timingBar.anchoredPosition
                    = new Vector2(-29.0f, warningEffect[randomAttackPos].GetComponent<RectTransform>().anchoredPosition.y);
                timingBar.gameObject.SetActive(true);
            }
            dinosaur.transform.position = new Vector3(Mathf.Lerp(147.0f, 157.0f + randomAttackPos, attackFlowTime / attackTime), dinosaurPosY[randomAttackPos], dinosaur.transform.position.z);
            timingBar.anchoredPosition = new Vector2(Mathf.Lerp(startWarningBarX, endWarningBarX, attackFlowTime / attackTime * 1.3f), timingBar.anchoredPosition.y);

            attackFlowTime += Time.deltaTime;
            if (attackFlowTime < attackTime)
                return;

            isSuccess = tailController.CollisionCount == 0;

            timingBar.gameObject.SetActive(false);
            warningEffect[randomAttackPos].SetActive(false);
            dinosaur.SetActive(false);
            attackCount--;

            flowTime = 0.0f;
            warningFlowTime = 0.0f;
            attackFlowTime = 0.0f;
            randomAttackPos = -1;
        }

        public void ExitState()
        {
            UIManager.Instance.SceneUISwap((int)ESceneUIType.RunningCanvas);

            dinosaur.SetActive(false);

            flowTime = 0.0f;
            warningFlowTime = 0.0f;
            attackFlowTime = 0.0f;
            randomAttackPos = -1;
        }

        public bool IsFinishMission(out bool success)
        {
            success = false;
            if (isSuccess)
            {
                success = true;
                return true;
            }
            if (attackCount <= 0)
                return true;
            return false;
        }
    }
}
