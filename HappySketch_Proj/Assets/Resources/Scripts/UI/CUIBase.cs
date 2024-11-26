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
        /// UI를 보이기 위한 활성화 메서드
        /// </summary>
        public virtual void Show()
        {
            Debug.Log($"UI/<color=yellow>{uiName}</color> 활성화");
            gameObject.SetActive(true);
        }

        /// <summary>
        /// UI를 숨기기 위한 비활성화 메서드
        /// </summary>
        public virtual void Hide()
        {
            Debug.Log($"UI/<color=yellow>{uiName}</color> 비활성화");
            gameObject.SetActive(false);
        }

        /// <summary>
        /// UI초기값 설정을 위한 메서드
        /// </summary>
        protected abstract void InitUI();

     /*   private void UIBind<T>(System.Type type) where T : UnityEngine.Object
        {
            //이거 매니저에서 가지고 있어야 하는거잖어
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
