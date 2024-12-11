using HakSeung;
using Jaehoon;
using JongJin;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace MyeongJin
{
	public class CVolcanicAsh : MonoBehaviour
	{
		public IObjectPool<CVolcanicAsh> Pool { get; set; }

		private SpriteRenderer sprite;
		private Vector3 startPosition;

        private GameObject gameSceneController;
        private GameSceneController gamecSceneController;
        private EGameState curState = EGameState.THIRDMISSION;

        private void Awake()
		{
			sprite = GetComponent<SpriteRenderer>();

            gameSceneController = GameObject.Find("GameSceneController");
            gamecSceneController = gameSceneController.GetComponent<GameSceneController>();
        }
        private void Update()
        {
            if (IsStateChanged())
                ReturnToPool();
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
        protected bool IsStateChanged()
        {
            return curState != gamecSceneController.CurState;
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
				SoundManager.instance.SFXPlay("Sounds/RemoveVolcanoAsh");
                ReturnToPool(10);
            }
        }
    }
}