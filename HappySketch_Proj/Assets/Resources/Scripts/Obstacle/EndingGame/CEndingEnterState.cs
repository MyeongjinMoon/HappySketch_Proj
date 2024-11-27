using HakSeung;
using JongJin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MyeongJin
{
    public class CEndingEnterState : CBaseEndingState
    {
        [SerializeField] private Fade fade;

        public override void EnterState()
        {
            UIManager.Instance.ShowPopupUI(UIManager.EPopupUIType.FadePopupCanvas.ToString());

            FadeInOut();
        }
        private void FadeInOut()
        {
            fade.FadeInOut();
        }
        public new bool IsFinishCurState()
        {
            return fade.IsFinished;
        }
    }
}