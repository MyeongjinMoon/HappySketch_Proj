using HakSeung;
using JongJin;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static HakSeung.UIManager;

namespace MyeongJin
{
	public class CEndingResultState : CBaseEndingState
	{
		[SerializeField] private Image image;
		[SerializeField] private Button nextStageBtn;
		[SerializeField] private Button restartBtn;
		[SerializeField] private Button endGameBtn;
		[SerializeField] private GameObject resultTable;

		private TextMeshProUGUI[] tableText;

		private float time = 0.0f;
		private const float fTime = 1.0f;
		private const int ENDTIME = 0;
		private const float SETTABLETIME = 0.6f;
		private const int SETBUTTONTIME = 3;
		private bool canSetButton = false;
		private bool isSetButton = false;

		private void Awake()
		{
			Transform[] children = resultTable.GetComponentsInChildren<Transform>();
			tableText = new TextMeshProUGUI[children.Length - 1];

			int i = 0;

			foreach (Transform child in children)
			{
				if (child.gameObject == resultTable)
				{
					continue;
				}
				tableText[i] = child.GetComponent<TextMeshProUGUI>();
				i++;
			}
		}

		public override void EnterState()
		{
            ShowResultPopup();

			StartCoroutine(SetTable(SETTABLETIME));
			StartCoroutine(SetButton(SETBUTTONTIME));
		}
		public override void ExitState()
		{
			// TODO <문명진> : 버튼 동작에 의한 이벤트 생성
		}
		private void ShowResultPopup()
		{
			// TODO <문명진> : 판넬 변경 조건 만들기
			UIManager.Instance.ShowPopupUI(EPopupUIType.EndingPopupPanel.ToString());
			((CUIEndingPopup)UIManager.Instance.CurrentPopupUI).ImageSwap(CUIEndingPopup.EndingState.SUCCESS);
		}
		private IEnumerator SetTable(float setTime)
		{
			while (setTime > ENDTIME)
			{
				setTime -= Time.deltaTime;
				yield return null;
			}

			resultTable.SetActive(true);
		}
		private IEnumerator SetButton(float setTime)
		{
			while (setTime > ENDTIME)
			{
				setTime -= Time.deltaTime;
				yield return null;
			}

			if (isGameSuccess)
			{
				nextStageBtn.gameObject.SetActive(true);
				endGameBtn.gameObject.SetActive(true);
			}
			else
			{
				restartBtn.gameObject.SetActive(true);
				endGameBtn.gameObject.SetActive(true);
			}
		}
	}
}