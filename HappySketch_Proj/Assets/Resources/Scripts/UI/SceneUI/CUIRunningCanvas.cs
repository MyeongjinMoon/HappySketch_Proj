using HakSeung;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HakSeung
{
    public class CUIRunningCanvas : CUIScene
    {

        private readonly float progressBarStartX = -650.0f;
        private readonly float progressBarEndX = 670.0f;
        public CUINote[] playerNotes = new CUINote[TOTALPLAYERS];

        [SerializeField] private float imageOffset = -20.0f;

        [SerializeField] private Canvas runningCanvas;
        [SerializeField] private Image progressBarImage;
        [SerializeField] private RectTransform dinosaurImagePos;

        [SerializeField] private TextMeshProUGUI endDistanceText;
        [SerializeField] private TextMeshProUGUI dinosaurDistanceText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private Image[] heartImages;

        [SerializeField] private RectTransform[] playerImageposes = new RectTransform[TOTALPLAYERS];

        protected override void InitUI()
        {
            float playerPosX = progressBarEndX - progressBarStartX;
            runningCanvas = UIManager.Instance.MainCanvas.GetComponent<Canvas>();

            /*for (int i = 0; i < playerImagepos.Length; i++)
                    playerImagepos[i].anchoredPosition = new Vector2(playerPosX, playerImagepos[i].anchoredPosition.y);*/
        }

        public void SetProgressBar(float progressRate)
        {
            progressBarImage.fillAmount = progressRate / 100.0f;
        }
        public void SetPlayerImage(int playerNumber, float playerDistance, float totalRunningDistance)
        {
            float playerPosX = (progressBarEndX - progressBarStartX)
                * playerDistance / totalRunningDistance + progressBarStartX + imageOffset;

            playerImageposes[playerNumber].anchoredPosition = new Vector2(playerPosX, playerImageposes[playerNumber].anchoredPosition.y);
        }

        public void SetDinosaurImage(float dinosaurDistance, float totalRunningDistance)
        {
            float dinosaurX = (progressBarEndX - progressBarStartX)
                * dinosaurDistance / totalRunningDistance + progressBarStartX + imageOffset;

            dinosaurImagePos.anchoredPosition = new Vector2(dinosaurX, dinosaurImagePos.anchoredPosition.y);
        }

        public void SetDinosaurDistanceText(float lastRankerDistance, float dinosaurDistance)
        {
            int distance = (int)Mathf.Round(lastRankerDistance - dinosaurDistance);
            dinosaurDistanceText.text = string.Format("-{0}M", distance);
        }
        public void SetEndLineDistanceText(float lastRankerDistance, float totalRunningDistance)
        {
            int distance = (int)Mathf.Round(totalRunningDistance - lastRankerDistance);
            endDistanceText.text = string.Format("{0}M", distance);
        }
        public void SetTimer(float roundTimeLimit)
        {
            int hour = (int)(roundTimeLimit / 60.0f);
            int minute = (int)(roundTimeLimit % 60.0f);

            timerText.text = string.Format("{0:D2} : {1:D2}", hour, minute);
        }
    }
}