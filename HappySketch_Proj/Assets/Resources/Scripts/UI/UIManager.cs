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
            
            END
        }

        public enum EPopupUIType
        {
            TutorialPopupPanel,

            END
        }
        
        public enum ETestType
        {
            RunningCanvas,
            EventScenePanel,
            TutorialPopupPanel,

        }

        private static UIManager s_Instance;
        
        private Dictionary<string, UnityEngine.Object> uiPrefabs = new Dictionary<string, UnityEngine.Object>();
        private Dictionary<string, CUIBase> uiObjs;

        private const string UIMANGEROBJECTNAME = "_UIManager";
        private const string PREFABSPATH = "Prefabs/UI/";
        private const int MAXSCENEUICOUNT = 2;

        private GameSceneController gameSceneController;

        private int popupIndex = 0;

        private List<CUIScene> SceneUIList = null;
        private Stack<CUIPopup> popupUIStack;
        public CUIPopup CurrentPopupUI { get { return popupUIStack.Peek(); } }
        public CUIScene CurSceneUI { get; private set; } = null;
        
        //TODO <이학승> RunningCanvas 이용을 위해 Get을 private 에서 해제 추후 Running Canvas 수정 필요
        public GameObject MainCanvas{ get; set; }
        //public GameObject MainCanvas{ private get; set; }
        

        public static UIManager Instance
        {
            get
            {
                if(s_Instance == null)
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

        //Todo<이학승> 임시로 기본 Find를 통해 받아옴 추후 Tag던 이름이던 참조 해야됨
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

            string uiName = type.GetEnumName(enumIndex) ;

            if (uiPrefabs.ContainsKey(uiName))
                return uiPrefabs[uiName];

            T uiObj = Resources.Load<T>(PREFABSPATH + $"{uiName}");
             
            if(uiObj == null)
                 Debug.LogError("로드 실패: " + PREFABSPATH + $"에 {uiName}는 존재하지 않습니다.");

            uiPrefabs.Add(uiName, uiObj);

            return uiPrefabs[uiName];
        }

        public void CreateSceneUI(string key, int sceneUIIndex = 0)
        {
            CUIScene sceneUI = null;

            if (!uiPrefabs.ContainsKey(key))
            {
                Debug.LogError($"SceneUI Key: {key}가 존재하지 않습니다.");
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
                    Debug.LogError($"{key}생성 실패");
            }
            else
                Debug.LogError($"{key}는 CUIScene를 상속받지 않는 타입입니다.");

            if (sceneUIIndex == 0)
                SceneUISwap();
            else
                SceneUIList[sceneUIIndex].Hide();
        }

        public void SceneUISwap(int sceneUIIndex = 0)
        {
            Debug.Log("씬전환");
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

            for(int i = 0; i < SceneUIList.Count; i++)
            {
                SceneUIList[i].Hide();
                Destroy(SceneUIList[i].gameObject);
            }
            SceneUIList.Clear();
        }


        //TODO<이학승> ShowPopupUI 제외하고 자주쓰이는 팝업 나중에 한번만 뜰 팝업들을 정리하는 메서드가 필요하다. 24/11/21 if문 보기 안좋으니수정필요
        public bool ShowPopupUI(string key)
        {
            CUIPopup popUI = null;

            if (!uiPrefabs.ContainsKey(key))
            {
                Debug.LogError($"PopupUI Key: {key}가 존재하지 않습니다.");
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
                    Debug.LogError($"{key}는 CUIPopup을 가지고 있지 않습니다.");
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
                Debug.LogError($"{key}는 CUIPopup을 가지고 있지 않습니다.");
                return false;
            }

            popupUIStack.Peek().gameObject.transform.SetAsLastSibling();
            return true;

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
                Debug.LogError($"popupUi: {popupUI.UIName}은 닫을 수 없습니다.");
                return;
            }

            popupUIStack.Peek().Hide();
            popupUIStack.Pop();
            --popupIndex;
        }

        public void CloseAllPopupUI()
        {
            while(popupUIStack.Count > 0)
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
                Debug.Log(items.name + "파괴");
            }
            uiObjs.Clear();
            popupUIStack.Clear();
        }
        


    }
}
