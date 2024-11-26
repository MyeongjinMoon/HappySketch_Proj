using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace HakSeung
{
    public abstract class CUIBase : MonoBehaviour
    {
        [SerializeField]private string uiName = null;
        protected RectTransform baseRectTransform;
        public string UIName { get { return uiName; } }

        private void Awake()
        {
            InitUI();
            baseRectTransform = gameObject.GetComponent<RectTransform>();
        }

        /// <summary>
        /// UI�� ���̱� ���� Ȱ��ȭ �޼���
        /// </summary>
        public virtual void Show()
        {
            Debug.Log($"UI/<color=yellow>{uiName}</color> Ȱ��ȭ");
            gameObject.SetActive(true);
        }

        /// <summary>
        /// UI�� ����� ���� ��Ȱ��ȭ �޼���
        /// </summary>
        public virtual void Hide()
        {
            Debug.Log($"UI/<color=yellow>{uiName}</color> ��Ȱ��ȭ");
            gameObject.SetActive(false);
        }

        /// <summary>
        /// UI�ʱⰪ ������ ���� �޼���
        /// </summary>
        protected abstract void InitUI();

     /*   private void UIBind<T>(System.Type type) where T : UnityEngine.Object
        {
            //�̰� �Ŵ������� ������ �־�� �ϴ°��ݾ�
            if (!type.IsEnum)
                return;

            string[] uiNames = Enum.GetNames(type);

            UnityEngine.Object[] uiObjects = new UnityEngine.Object[uiNames.Length];

            for(int i = 0; i < uiNames.Length; i++)
            {
                //uiObjects[i] = 
            }

        }*/
    }
}
