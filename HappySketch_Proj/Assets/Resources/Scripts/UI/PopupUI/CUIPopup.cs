using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HakSeung
{
    public abstract class CUIPopup : CUIBase
    {
        protected bool isPlayingPopup = false;

        public override void Show()
        {
            base.Show();
            isPlayingPopup = true;
        }

        public override void Hide()
        {
            base.Hide();
            isPlayingPopup = false;
        }

        public virtual void ClosePopupUI()
        {
            UIManager.Instance.ClosePopupUI(this);
        }

        protected abstract IEnumerator PlayPopupEffect();
    }
}
