using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HakSeung
{
    public abstract class CUIPopup : CUIBase
    {
        public virtual void ClosePopupUI()
        {
            // 상속받는애들은 정보 초기화 처리 해주기
            UIManager.Instance.ClosePopupUI(this);
        }

        protected abstract IEnumerator PlayPopupEffect();
    }
}
