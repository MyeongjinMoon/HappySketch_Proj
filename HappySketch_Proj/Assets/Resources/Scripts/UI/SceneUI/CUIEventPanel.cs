using HakSeung;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HakSeung
{
    public class CUIEventPanel : CUIScene
    {
        [SerializeField] public CUIProgressBar progressBar;
        public CUINote[] playerNotes = new CUINote[TOTALPLAYERS];
        [SerializeField] private TextMeshProUGUI timerText;
        public GameObject timer;

        protected override void InitUI()
        {

        }
        public void SetTimer(float roundTimeLimit)
        {
            int hour = (int)(roundTimeLimit / 60.0f);
            int minute = (int)(roundTimeLimit % 60.0f);

            timerText.text = string.Format("{0:D2} : {1:D2}", hour, minute);
        }
    }
}
