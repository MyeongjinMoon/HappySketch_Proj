using HakSeung;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace JongJin
{
    public class Fade : CUIPopup
    {
        [SerializeField] private Image panel;
        private float time = 0.0f;
        private float fTime = 1.0f;

        public bool IsFinished { get; private set; } = false;
        
        public void FadeInOut()
        {
            IsFinished = false;
            StartCoroutine(FadeFlow());
        }
        public void FadeIn()
        {
            StartCoroutine(FadeInFlow());
        }
        public void FadeOut()
        {
            StartCoroutine(FadeOutFlow());
        }

        protected override void InitUI()
        {
            time = 0.0f;
            fTime = 1.0f;
        }

        public override void Show()
        {
            base.Show();

            IsFinished = false;
            StartCoroutine(PlayPopupEffect());
           
        }

        public override void Hide()
        {
            base.Hide();
            panel.color = new Color(0,0,0,0);
        }

        /// <summary>
        /// Popup이 활성화 되면 작동하는 효과 FadeFlow와 같은 기능을 함
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator PlayPopupEffect()
        {
            time = 0.0f;

            Color alpha = panel.color;
            while (alpha.a < 1.0f && isPlayingPopup)
            {
                time += Time.deltaTime / fTime;
                alpha.a = Mathf.Lerp(0.0f, 1.0f, time);
                panel.color = alpha;
                yield return null;
            }

            time = 0.0f;
            yield return new WaitForSeconds(1.0f);
            IsFinished = true;

            while (alpha.a > 0.0f && isPlayingPopup)
            {
                time += Time.deltaTime / fTime;
                alpha.a = Mathf.Lerp(1.0f, 0.0f, time);
                panel.color = alpha;
                yield return null;
            }

            UIManager.Instance.ClosePopupUI(this);
            yield return null;
        }

        IEnumerator FadeFlow()
        {
            time = 0.0f;

            Color alpha = panel.color;
            while (alpha.a < 1.0f)
            {
                time += Time.deltaTime / fTime;
                alpha.a = Mathf.Lerp(0.0f, 1.0f, time);
                panel.color = alpha;
                yield return null;
            }
            
            time = 0.0f;
            yield return new WaitForSeconds(1.0f);
            IsFinished = true;

            while (alpha.a > 0.0f)
            {
                time += Time.deltaTime / fTime;
                alpha.a = Mathf.Lerp(1.0f, 0.0f, time);
                panel.color = alpha;
                yield return null;
            }
            yield return null;
        }
        IEnumerator FadeInFlow()
        {
            time = 0.0f;
            Color alpha = panel.color;

            while (alpha.a > 0.0f)
            {
                time += Time.deltaTime / fTime;
                alpha.a = Mathf.Lerp(1.0f, 0.0f, time);
                panel.color = alpha;
                yield return null;
            }
            yield return null;
        }
        IEnumerator FadeOutFlow()
        {
            time = 0.0f;

            Color alpha = panel.color;
            while (alpha.a < 1.0f)
            {
                time += Time.deltaTime / fTime;
                alpha.a = Mathf.Lerp(0.0f, 1.0f, time);
                panel.color = alpha;
                yield return null;
            }
        }
    }
}