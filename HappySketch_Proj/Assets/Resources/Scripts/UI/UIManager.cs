using JongJin;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HakSeung
{
    public class UIManager : MonoBehaviour
    {
        public enum ESceneUIType
        {
            RunningCanvas,
            EventScenePanel,
            TailMissionPanel,
            TutorialCheckUIPanel,
            END
        }

        public enum EPopupUIType
        {
            TutorialPopupPanel,
            FadePopupCanvas,
            EndingPopupPanel,

            END
        }


        private static UIManager s_Instance;

        private Dictionary<string, UnityEngine.Object> uiPrefabs = new Dictionary<string, UnityEngine.Object>();
        private Dictionary<string, CUIBase> uiObjs;

        private const string UIMANGEROBJECTNAME = "_UIManager";
        private const string PREFABSPATH = "Prefabs/UI/";
        private const int MAXSCENEUICOUNT = 3;

        private GameSceneController gameSceneController;

        private int popupIndex = 0;

        private List<CUIScene> SceneUIList = null;
        private Stack<CUIPopup> popupUIStack;
        public CUIPopup CurrentPopupUI 
        { 
            get 
            {
                if (popupUIStack.Count == 0)
                    return null;
                return popupUIStack.Peek(); 
            } 
        }
        public CUIScene CurSceneUI { get; private set; } = null;

        public GameObject MainCanvas { get; set; }
        public static UIManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    GameObject newUIManagerObject = new GameObject(UIMANGEROBJECTNAME);
                    s_Instance = newUIManagerObject.AddComponent<UIManager>();
                }
                return s_Instance;

            }
        }

        private void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            s_Instance = this;

            DontDestroyOnLoad(this.gameObject);

            Initialzie();
        }

        private void Initialzie()
        {
            uiPrefabs = new Dictionary<string, UnityEngine.Object>();
            uiObjs = new Dictionary<string, CUIBase>();
            popupUIStack = new Stack<CUIPopup>();
            SceneUIList = new List<CUIScene>();
            
        }

        public void MainCanvasSetting()
        {
            MainCanvas = GameObject.Find("MainCanvas");
            for (int i = 0; i < MAXSCENEUICOUNT; i++)
                SceneUIList.Add(null);
        }

        public UnityEngine.Object UICashing<T>(System.Type type, int enumIndex) where T : UnityEngine.Object
        {
            if (!type.IsEnum)
                return null;

            string uiName = type.GetEnumName(enumIndex);

            if (uiPrefabs.ContainsKey(uiName))
            {
                return uiPrefabs[uiName];
            }

            T uiObj = Resources.Load<T>(PREFABSPATH + $"{uiName}");

            uiPrefabs.Add(uiName, uiObj);

            return uiPrefabs[uiName];
        }

        public void CreateSceneUI(string key, int sceneUIIndex = 0)
        {
            CUIScene sceneUI = null;

            if (!uiPrefabs.ContainsKey(key))
            {
                return;
            }

            if (SceneUIList[sceneUIIndex] != null && SceneUIList[sceneUIIndex].gameObject != null)
            {
                SceneUIList[sceneUIIndex].Hide();
                Destroy(SceneUIList[sceneUIIndex].gameObject);
                SceneUIList[sceneUIIndex] = null;
            }


            if (sceneUI = uiPrefabs[key].GetComponent<CUIScene>())
            {
                if (SceneUIList[sceneUIIndex] = Instantiate(sceneUI))
                {
                    SceneUIList[sceneUIIndex].transform.SetParent(MainCanvas.transform, false);
                }
            }

            if (sceneUIIndex == 0)
                SceneUISwap();
            else
                SceneUIList[sceneUIIndex].Hide();
        }

        public void SceneUISwap(int sceneUIIndex = 0)
        {
            if (sceneUIIndex >= SceneUIList.Count)
                return;

            if (CurSceneUI != null)
                CurSceneUI.Hide();

            CurSceneUI = SceneUIList[sceneUIIndex];
            CurSceneUI.Show();

        }

        public void ClearSceneUI()
        {
            if (SceneUIList.Count == 0)
                return;

            for (int i = 0; i < SceneUIList.Count; i++)
            {
                if (SceneUIList[i] == null)
                    continue;

                SceneUIList[i].Hide();
                Destroy(SceneUIList[i].gameObject);


            }
            SceneUIList.Clear();

        }

        public bool ShowPopupUI(string key)
        {
            CUIPopup popUI = null;

            if (!uiPrefabs.ContainsKey(key))
                return false;

            if (uiObjs.ContainsKey(key))
            {
                if (popUI = uiObjs[key] as CUIPopup)
                {
                    popupUIStack.Push(popUI);
                    popupUIStack.Peek().Show();
                    ++popupIndex;
                }
                else
                    return false;
            }
            else if (popUI = Instantiate(uiPrefabs[key]).GetComponent<CUIPopup>())
            {
                popUI.transform.SetParent(MainCanvas.transform, false);

                uiObjs.Add(key, popUI);
                popupUIStack.Push(popUI);
                popupUIStack.Peek().Show();
                ++popupIndex;
            }
            else
                return false;

            popupUIStack.Peek().gameObject.transform.SetAsLastSibling();
            return true;

        }

        public void SwapPopupUI(String key)
        {
            if (popupUIStack.Count == 0)
                ShowPopupUI(key);
            else 
            {
                ClosePopupUI(CurrentPopupUI);
                ShowPopupUI(key);
            }
        }


        public void ClosePopupUI()
        {
            if (popupUIStack.Count == 0)
                return;

            popupUIStack.Peek().Hide();
            popupUIStack.Pop();
            --popupIndex;
        }

        public void ClosePopupUI(CUIPopup popupUI)
        {
            if (popupUIStack.Count == 0)
                return;

            if (popupUIStack.Peek() != popupUI)
            {
                return;
            }

            popupUIStack.Peek().Hide();
            popupUIStack.Pop();
            --popupIndex;
        }

        public void CloseAllPopupUI()
        {
            while (popupUIStack.Count > 0)
            {
                ClosePopupUI();
            }
        }

        public void ClearUIObj()
        {
            if (uiObjs.Count == 0)
                return;

            CloseAllPopupUI();

            foreach (CUIBase items in uiObjs.Values)
            {
                Destroy(items.gameObject);
            }
            uiObjs.Clear();
            popupUIStack.Clear();
        }


        public void ClearAllUI()
        {
            ClearSceneUI();
            ClearUIObj();
        }


    }
}
