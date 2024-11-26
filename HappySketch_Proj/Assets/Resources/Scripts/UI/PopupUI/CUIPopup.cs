using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HakSeung
{
    public abstract class CUIPopup : CUIBase
    {
        public virtual void ClosePopupUI()
        {
            // ��ӹ޴¾ֵ��� ���� �ʱ�ȭ ó�� ���ֱ�
            UIManager.Instance.ClosePopupUI(this);
        }

        protected abstract IEnumerator PlayPopupEffect();
    }
}
