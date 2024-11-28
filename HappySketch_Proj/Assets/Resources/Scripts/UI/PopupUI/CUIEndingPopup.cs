using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JongJin;
using HakSeung;

namespace MyeongJin
{
    public class CUIEndingPopup : CUIPopup
    {
        public enum EndingState
        {
            SUCCESS,
            FAILED,

            END
        }

        private bool isPlayingEffect = false;

        [SerializeField] private Image endingImage;

        [SerializeField] private Sprite successSprite;
        [SerializeField] private Sprite failedSprite;
        [SerializeField] private float effectDuration;

        [SerializeField] private Image panelImage;
        protected override void InitUI()
        {
            effectDuration = 0.5f;
        }

        public override void Show()
        {
            base.Show();
            StartCoroutine(PlayPopupEffect());
            if (panelImage.color.a != 0f)
                panelImage.color = new UnityEngine.Color(1.0f, 1.0f, 1.0f, 0f);
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
                Debug.LogError("baseRectTransform");
                yield break;
            }

            while (effectDuration > elapsedTime)
            {
                float timeProgress = elapsedTime / effectDuration;
                baseRectTransform.localScale = Vector3.Lerp(startScale, endScale, timeProgress);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            baseRectTransform.localScale = endScale;

            isPlayingEffect = false;

        }

        public void ImageSwap(EndingState tutorialAction)
        {
            switch (tutorialAction)
            {
                case EndingState.SUCCESS:
                    endingImage.sprite = successSprite;
                    break;
                case EndingState.FAILED:
                    endingImage.sprite = failedSprite;
                    break;
            }
        }
    }
}