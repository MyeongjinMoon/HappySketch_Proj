using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace JongJin
{
    public class Fade : MonoBehaviour
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

        IEnumerator FadeFlow()
        {
            panel.gameObject.SetActive(true);
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
            panel.gameObject.SetActive(false);
            yield return null;
        }
    }
}