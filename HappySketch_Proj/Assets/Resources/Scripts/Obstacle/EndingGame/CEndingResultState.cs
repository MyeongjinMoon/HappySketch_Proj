using HakSeung;
using JongJin;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static HakSeung.UIManager;

namespace MyeongJin
{
	public class CEndingResultState : MonoBehaviour, IGameState
    {
		[SerializeField] private Image image;
		[SerializeField] private Button nextStageBtn;
		[SerializeField] private Button restartBtn;
		[SerializeField] private Button endGameBtn;
		[SerializeField] private GameObject resultTable;

		private TextMeshProUGUI[] tableText;

		private const float fTime = 1.0f;
		private const int ENDTIME = 0;
		private const float SETTABLETIME = 0.6f;
		private const int SETBUTTONTIME = 3;

        private CEndingSceneController cEndingSceneController;

        protected int topPlayerIndex;
        protected float topPlayerTime;
        protected float restPlayerTime;
        protected bool isFinish = false;
        public bool isGameSuccess;

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

            cEndingSceneController = GameObject.Find("EndingSceneController").GetComponent<CEndingSceneController>();
            isGameSuccess = cEndingSceneController.isGameSuccess;
            topPlayerIndex = cEndingSceneController.topPlayerIndex;

            if (topPlayerIndex == 0)
            {
                topPlayerTime = cEndingSceneController.player1Time;
                restPlayerTime = cEndingSceneController.player2Time;
            }
            else
            {
                topPlayerTime = cEndingSceneController.player2Time;
                restPlayerTime = cEndingSceneController.player1Time;
            }

			SetButton();
        }

		public void EnterState()
		{
			ShowResultPopup();

			if (isGameSuccess)
				StartCoroutine(SetTable(SETTABLETIME));
			StartCoroutine(SetButton(SETBUTTONTIME));
        }
        public void UpdateState()
        {

        }
        public void ExitState()
		{
			// TODO <문명진> : 버튼 동작에 의한 이벤트 생성
		}
		private void ShowResultPopup()
		{
			// TODO <문명진> : 판넬 변경 조건 만들기
			UIManager.Instance.ShowPopupUI(EPopupUIType.EndingPopupPanel.ToString());
			if (isGameSuccess)
				((CUIEndingPopup)UIManager.Instance.CurrentPopupUI).ImageSwap(CUIEndingPopup.EndingState.SUCCESS);
			else
				((CUIEndingPopup)UIManager.Instance.CurrentPopupUI).ImageSwap(CUIEndingPopup.EndingState.FAILED);
		}
		private IEnumerator SetTable(float setTime)
		{
			if (topPlayerIndex == 0)
			{
				int topIndex = 1;
				int restIndex = 2;
				tableText[2].text = topIndex.ToString() + "P";
				tableText[3].text = restIndex.ToString() + "P";
				SetTime(tableText[4], topPlayerTime);
				SetTime(tableText[5], restPlayerTime);
            }
			else
			{
				int topIndex = 2;
				int restIndex = 1;
				tableText[2].text = topIndex.ToString() + "P";
				tableText[3].text = restIndex.ToString() + "P";
                SetTime(tableText[4], topPlayerTime);
                SetTime(tableText[5], restPlayerTime);
            }

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
        private void SetTime(TextMeshProUGUI tmpUGI, float time)
		{
            int minute = (int)(time / 60.0f);
            int second = (int)(time % 60.0f);
			int millisecond = (int)((time * 100) % 100.0f);

			tmpUGI.text = string.Format("{0:D2} : {1:D2} : {2:D2}", minute, second, millisecond);
        }
		private void SetButton()
		{
			restartBtn.onClick.AddListener(OnClickRestart);
            endGameBtn.onClick.AddListener(OnClickEndGame);

        }
        private void OnClickRestart()
		{
			UIManager.Instance.ClearAllUI();
			SceneManager.LoadScene("StartScene");
		}
        private void OnClickEndGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
        }
    }
}