using HakSeung;
using Jaehoon;
using JongJin;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
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

		public enum EventResult
		{
			SUCCESS,
			FAILED,

            END
        }


        [SerializeField] private Image guideImage;
        [SerializeField] private Image timerFillImage;
        [SerializeField] private TextMeshProUGUI timerCountText;
        [SerializeField] private Sprite[] guideSprites = new Sprite[10];
        [SerializeField] private float effectDuration;
        [SerializeField] private GameObject timerImage;
		[SerializeField] private Image panelImage;
		[SerializeField] private ParticleSystem successParticle;
		[SerializeField] private ParticleSystem failParticle;
		enum SoundType
		{
			DEFAULT,
			SUCCESS,
			FAIL,
			END
		}

        private Vector3 startScale = Vector3.zero;
        private Vector3 endScale = Vector3.one;
		private UnityEngine.Color defaultColor;
		private UnityEngine.Color alphaClearColor;
		private bool isOnSound;
		private SoundType soundType;
        protected override void InitUI()
        {
            effectDuration = 0.5f;
            isOnSound = false;
            soundType = SoundType.DEFAULT;
			defaultColor = panelImage.color;
            alphaClearColor = new UnityEngine.Color(1.0f, 1.0f, 1.0f, 0f);
        }

        public void TimerUpdate(float curTime)
        {
            if (curTime < 0)
                curTime = 0;
            
            timerFillImage.fillAmount = Mathf.Clamp(curTime * 0.1f, 0f, 1f);
            timerCountText.text = (Mathf.CeilToInt(curTime)).ToString();

        }

        public override void Show()
        {
			base.Show();
			isOnSound = true;
            StartCoroutine(PlayPopupEffect());
			if (panelImage.color.a != defaultColor.a)
				panelImage.color = defaultColor;
            timerImage.SetActive(true);
		}

        public override void Hide()
        {
            base.Hide();
			successParticle.Stop();
			failParticle.Stop();
        }



        public void TimerHide()
		{
			timerImage.SetActive(false);
		}


		protected override IEnumerator PlayPopupEffect()
		{
			float elapsedTime = 0f;

			if (baseRectTransform == null)
			{
				yield break;
			}


            while (effectDuration > elapsedTime && isPlayingPopup)
			{
				float timeProgress = elapsedTime / effectDuration;
				baseRectTransform.localScale = Vector3.Lerp(startScale, endScale, timeProgress);
				elapsedTime += Time.deltaTime;
				yield return null;
			}

			baseRectTransform.localScale = endScale;
			
            if (isOnSound)
            {
                switch (soundType)
                {
                    case SoundType.FAIL:
                        SoundManager.instance.SFXPlay("Sounds/MissionFail");
                        break;
                    case SoundType.SUCCESS:
                        SoundManager.instance.SFXPlay("Sounds/MissionSuccess");
                        break;
					default:
                        SoundManager.instance.SFXPlay("Sounds/PopupUI");
                        break;
                }
               soundType = SoundType.DEFAULT;

                isOnSound = false;
            }
		}

		public void ImageSwap(TutorialState tutorialAction)
		{
			switch (tutorialAction)
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
			

            switch (gameSceneState)
			{
				case EGameState.TAILMISSION:
					guideImage.sprite = guideSprites[(int)TutorialState.END + (int)EventState.TAIL];
					break;
				case EGameState.FIRSTMISSION:
					guideImage.sprite = guideSprites[(int)TutorialState.END + (int)EventState.PTEROSAUR];
					break;
				case EGameState.SECONDMISSION:
					guideImage.sprite = guideSprites[(int)TutorialState.END + (int)EventState.INSECT];
					break;
				case EGameState.THIRDMISSION:
					guideImage.sprite = guideSprites[(int)TutorialState.END + (int)EventState.VOLCANICASH];
					break;
				default:
					break;
			}
		}

		public void ImageSwap(EventResult eventResult)
		{

            switch (eventResult)
			{
				case EventResult.SUCCESS: 
					guideImage.sprite = guideSprites[(int)TutorialState.END + (int)EventState.END + (int)EventResult.SUCCESS];
                    panelImage.color = alphaClearColor;
					successParticle.Play();
                    soundType = SoundType.SUCCESS;
                    break;
				case EventResult.FAILED:
					guideImage.sprite = guideSprites[(int)TutorialState.END + (int)EventState.END + (int)EventResult.FAILED];
                    panelImage.color = alphaClearColor;
					failParticle.Play();
                    soundType = SoundType.FAIL;
                    break;
			
				default:
					break;
			}
		}



	}
}
