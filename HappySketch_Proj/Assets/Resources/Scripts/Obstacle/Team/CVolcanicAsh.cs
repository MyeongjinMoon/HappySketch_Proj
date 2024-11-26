using HakSeung;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;

namespace MyeongJin
{
	public class CVolcanicAsh : MonoBehaviour
	{
		public IObjectPool<CVolcanicAsh> Pool { get; set; }

		private SpriteRenderer sprite;
		private Vector3 startPosition;

		private void Awake()
		{
			sprite = GetComponent<SpriteRenderer>();
		}
		private void OnEnable()
		{
			StartCoroutine("CastAshes");
		}
		private void OnDisable()
		{
			ResetObstacle();
		}
		private IEnumerator CastAshes()
		{
			float duration = 2f;
			float elapsedTime = 0f;

			var color = sprite.color;

			while (elapsedTime < duration)
			{
				// 경과 시간에 따라 알파 값을 증가
				elapsedTime += Time.deltaTime;
				color.a = Mathf.Lerp(0, 1, elapsedTime / duration);
				sprite.color = color;

				yield return null;
			}

			color.a = 1f;
			sprite.color = color;
			RenderSettings.fogDensity += 0.005f;
        }
		public void ReturnToPool()
		{
			Pool.Release(this);
		}
        public void ReturnToPool(int fillValue)
        {
            if (((CUIEventPanel)UIManager.Instance.CurSceneUI) != null)
                ((CUIEventPanel)UIManager.Instance.CurSceneUI).progressBar.FillProgressBar(fillValue);
            Pool.Release(this);
        }
        public void ResetObstacle()
		{
			this.GetComponent<BoxCollider>().enabled = true;
		}
		public void FadeAway()
		{
            var color = sprite.color;

			color.a -= 0.5f;
            sprite.color = color;

			if (RenderSettings.fogDensity > 0)
				RenderSettings.fogDensity -= 0.0025f;

            if (sprite.color.a == 0)
			{
                ReturnToPool(10);
            }
        }
        //private void OnTriggerEnter(Collider other)
        //{
        //    this.GetComponent<BoxCollider>().enabled = false;

        //    Debug.Log("Clear Fog");

        //    StartCoroutine("Clear");
        //}
        //private IEnumerator Clear()
        //{
        //    yield return new WaitForSeconds(1.0f);

        //    ReturnToPool();
        //}
    }
}