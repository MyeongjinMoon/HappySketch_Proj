using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace HakSeung
{
	public class CUIProgressBar : MonoBehaviour
	{
		[Header("Porgress Bar")]
		//[SerializeField] private float curProgress;
		[SerializeField]private float maxProgress;
		[SerializeField]private Image PrograssBarFill;

		private const float maxFillAmount = 1f;
		private const float defaultMaxProgress = 100;
		public bool isProgressBarFullFilled = false;

		private void Awake()
		{
			Init();
		}
        private void OnDisable()
        {
            Init();
        }
        public float MaxProgress { 
			set 
			{
				if(value > defaultMaxProgress)
					maxProgress = value;
			} 
		}
		public void Init()
		{
			PrograssBarFill.fillAmount = 0;
            maxProgress = defaultMaxProgress;
			isProgressBarFullFilled = false;
        }

		public void FillProgressBar(float curProgress)
		{
			//if (maxProgress <= defaultMaxProgress) return;

			if(PrograssBarFill.fillAmount < maxFillAmount)
				PrograssBarFill.fillAmount += curProgress / maxProgress;
			else
				PrograssBarFill.fillAmount = maxFillAmount;

			if(PrograssBarFill.fillAmount == maxFillAmount)
                isProgressBarFullFilled = true;

        }
	}
}
