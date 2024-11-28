using JongJin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Rendering.InspectorCurveEditor;

namespace HakSeung
{
    public class UIManager : MonoBehaviour
    {
        public enum ESceneUIType
        {
            RunningCanvas,
            EventScenePanel,
            TailMissionPanel,
            END
        }

        public enum EPopupUIType
        {
            TutorialPopupPanel,
            FadePopupCanvas,
            EndingPopupPanel,

            END
        }

        public enum ETestType
        {
            RunningCanvas,
            EventScenePanel,
            TutorialPopupPanel,
            FadePopupCanvas,
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

        //TODO <���н�> RunningCanvas �̿��� ���� Get�� private ���� ���� ���� Running Canvas ���� �ʿ�
        public GameObject MainCanvas { get; set; }
        //public GameObject MainCanvas{ private get; set; }


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

        //Todo<���н�> �ӽ÷� �⺻ Find�� ���� �޾ƿ� ���� Tag�� �̸��̴� ���� �ؾߵ�
        private void Initialzie()
        {
            MainCanvas = GameObject.Find("MainCanvas");
            uiPrefabs = new Dictionary<string, UnityEngine.Object>();
            uiObjs = new Dictionary<string, CUIBase>();
            popupUIStack = new Stack<CUIPopup>();
            SceneUIList = new List<CUIScene>();
            for (int i = 0; i < MAXSCENEUICOUNT; i++)
                SceneUIList.Add(null);
        }

        public UnityEngine.Object UICashing<T>(System.Type type, int enumIndex) where T : UnityEngine.Object
        {
            if (!type.IsEnum)
                return null;

            string uiName = type.GetEnumName(enumIndex);

            if (uiPrefabs.ContainsKey(uiName))
                return uiPrefabs[uiName];

            T uiObj = Resources.Load<T>(PREFABSPATH + $"{uiName}");

            if (uiObj == null)
                Debug.LogError("�ε� ����: " + PREFABSPATH + $"�� {uiName}�� �������� �ʽ��ϴ�.");

            uiPrefabs.Add(uiName, uiObj);

            return uiPrefabs[uiName];
        }

        public void CreateSceneUI(string key, int sceneUIIndex = 0)
        {
            CUIScene sceneUI = null;

            if (!uiPrefabs.ContainsKey(key))
            {
                Debug.LogError($"SceneUI Key: {key}�� �������� �ʽ��ϴ�.");
                return;
            }

            if (SceneUIList[sceneUIIndex] != null)
            {
                SceneUIList[sceneUIIndex].Hide();
                Destroy(SceneUIList[sceneUIIndex].gameObject);
            }

            if (sceneUI = uiPrefabs[key].GetComponent<CUIScene>())
            {
                if (SceneUIList[sceneUIIndex] = Instantiate(sceneUI))
                {
                    SceneUIList[sceneUIIndex].transform.SetParent(MainCanvas.transform, false);
                    //SceneUIList[sceneUIIndex].Show();
                }
                else
                    Debug.LogError($"{key}���� ����");
            }
            else
                Debug.LogError($"{key}�� CUIScene�� ��ӹ��� �ʴ� Ÿ���Դϴ�.");

            if (sceneUIIndex == 0)
                SceneUISwap();
            else
                SceneUIList[sceneUIIndex].Hide();
        }

        public void SceneUISwap(int sceneUIIndex = 0)
        {
            Debug.Log("����ȯ");
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
                SceneUIList[i].Hide();
                Destroy(SceneUIList[i].gameObject);
            }
            SceneUIList.Clear();
        }


        //TODO<���н�> ShowPopupUI �����ϰ� ���־��̴� �˾� ���߿� �ѹ��� �� �˾����� �����ϴ� �޼��尡 �ʿ��ϴ�. 24/11/21 if�� ���� �������ϼ����ʿ�
        public bool ShowPopupUI(string key)
        {
            CUIPopup popUI = null;

            if (!uiPrefabs.ContainsKey(key))
            {
                Debug.LogError($"PopupUI Key: {key}�� �������� �ʽ��ϴ�.");
                return false;
            }

            if (uiObjs.ContainsKey(key))
            {
                if (popUI = uiObjs[key] as CUIPopup)
                {
                    popupUIStack.Push(popUI);
                    popupUIStack.Peek().Show();
                    ++popupIndex;
                }
                else
                {
                    Debug.LogError($"{key}�� CUIPopup�� ������ ���� �ʽ��ϴ�.");
                    return false;
                }
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
            {
                Debug.LogError($"{key}�� CUIPopup�� ������ ���� �ʽ��ϴ�.");
                return false;
            }

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
                Debug.LogError($"popupUi: {popupUI.UIName}�� ���� �� �����ϴ�.");
                return;
            }

            popupUIStack.Peek().Hide();
            popupUIStack.Pop();
            --popupIndex;
        }

        public void CloseAllPopupUI()
        {
            while (popupUIStack.Count > 0)
                ClosePopupUI();
        }

        public void ClearUIObj()
        {
            if (uiObjs.Count == 0)
                return;

            CloseAllPopupUI();

            foreach (CUIBase items in uiObjs.Values)
            {
                Destroy(items.gameObject);
                Debug.Log(items.name + "�ı�");
            }
            uiObjs.Clear();
            popupUIStack.Clear();
        }


        public void ClearAllUI()
        {
            ClearUIObj();
            ClearSceneUI();
        }


    }
}
