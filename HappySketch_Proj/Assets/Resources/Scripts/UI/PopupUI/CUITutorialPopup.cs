using HakSeung;
using JongJin;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace HakSeung
{
    public class CUITutorialPopup : CUIPopup
    {
        public enum TutorialState
        {
            STORY,
            RUNNING,
            JUMP,
            HEART,
            END
        }

        public enum EventState
        {
            TAIL,
            PTEROSAUR,
            INSECT,
            VOLCANICASH,
            END
        }

        private bool isPlayingEffect = false;

        [SerializeField] private Image guideImage;
        [SerializeField] private Image timerFillImage;
        [SerializeField] private TextMeshProUGUI timerCountText;
        //TODO <학승> - 상수 7 넣어놓은 것 나중에 state에 맞게 처리해 놔야됨
        [SerializeField] private Sprite[] guideSprites = new Sprite[7];
        [SerializeField] private float effectDuration;
        [SerializeField] private GameObject timerImage;

        protected override void InitUI()
        {
            effectDuration = 0.5f;
        }

        public void TimerUpdate(float curTime)
        {
            if (curTime < 0)
                curTime = 0;
            
            timerFillImage.fillAmount = Mathf.Clamp(curTime * 0.1f, 0f, 1f);
            timerCountText.text = curTime.ToString();
        }

        public override void Show()
        {
            base.Show();
            StartCoroutine(PlayPopupEffect());
            timerImage.SetActive(true);
        }

        public void TimerHide()
        {
            timerImage.SetActive(false);
        }


        protected override IEnumerator PlayPopupEffect()
        {
            if (isPlayingEffect)
                yield break;
            else
                isPlayingEffect = true;

            float elapsedTime = 0f;
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;

            if (baseRectTransform == null)
            {
                Debug.LogError("baseRectTransform이 존재하지 않음");
                yield break;
            }

            while (effectDuration > elapsedTime)
            {
                float timeProgress = elapsedTime / effectDuration ;
                baseRectTransform.localScale = Vector3.Lerp(startScale, endScale, timeProgress);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            baseRectTransform.localScale = endScale;

            isPlayingEffect = false;

        }

        public void ImageSwap(TutorialState tutorialAction )
        {
            switch(tutorialAction)
            {
                case TutorialState.STORY:
                    guideImage.sprite = guideSprites[(int)TutorialState.STORY];
                    break;
                case TutorialState.RUNNING:
                    guideImage.sprite = guideSprites[(int)TutorialState.RUNNING];
                    break;
                case TutorialState.JUMP:
                    guideImage.sprite = guideSprites[(int)TutorialState.JUMP];
                    break;
                case TutorialState.HEART:
                    guideImage.sprite = guideSprites[(int)TutorialState.HEART];
                    break;
                default:
                    guideImage.sprite = guideSprites[(int)TutorialState.RUNNING];
                    break;
            }

        }
        public void ImageSwap(EGameState gameSceneState)
        {
            switch(gameSceneState)
            {
                case EGameState.TAILMISSION: //꼬리
                    guideImage.sprite = guideSprites[(int)TutorialState.END + (int)EventState.TAIL ];
                    break;
                case EGameState.FIRSTMISSION: // 익룡
                    guideImage.sprite = guideSprites[(int)TutorialState.END + (int)EventState.PTEROSAUR];
                    break;
                case EGameState.SECONDMISSION: // 파리때
                    guideImage.sprite = guideSprites[(int)TutorialState.END + (int)EventState.INSECT];
                    break;
                case EGameState.THIRDMISSION: // 화산재
                    guideImage.sprite = guideSprites[(int)TutorialState.END + (int)EventState.VOLCANICASH];
                    break;
                default:
                    break;
            }
        }



    }
}
