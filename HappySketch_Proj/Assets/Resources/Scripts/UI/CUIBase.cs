using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace HakSeung
{
    public abstract class CUIBase : MonoBehaviour
    {
        [SerializeField] private string uiName = null;
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
            if (string.IsNullOrEmpty(uiName))
                uiName = gameObject.name;

            gameObject.SetActive(true);
        }

        /// <summary>
        /// UI�� ����� ���� ��Ȱ��ȭ �޼���
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// UI�ʱⰪ ������ ���� �޼���
        /// </summary>
        protected abstract void InitUI();

    }
}
