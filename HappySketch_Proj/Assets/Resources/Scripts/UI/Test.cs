using HakSeung;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HakSeung
{
    public class Test : MonoBehaviour
    {

        private void Awake()
        {
            //사용할 UI 미리 Cashing 이 과정을 씬 초반에 무조건 해주어야 한다.
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.ETestType), (int)UIManager.ETestType.RunningCanvas);
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.ETestType), (int)UIManager.ETestType.EventScenePanel);
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.ETestType), (int)UIManager.ETestType.TutorialPopupPanel);

        }

        void Start()
        {
            //씬 UI 생성 사용할 씬들을 미리 준비해 놓기. Default로 0번은 켜진 상태로 유지됨
            UIManager.Instance.CreateSceneUI(UIManager.ETestType.EventScenePanel.ToString(), (int)UIManager.ETestType.RunningCanvas);
            UIManager.Instance.CreateSceneUI(UIManager.ETestType.RunningCanvas.ToString(), (int)UIManager.ETestType.EventScenePanel);
        }

        // Update is called once per frame
        void Update()
        {
            #region 팝업 UI 사용 예시
            //팝업 UI 보이기
            if (Input.GetKeyDown(KeyCode.P))
            {
                UIManager.Instance.ShowPopupUI(UIManager.ETestType.TutorialPopupPanel.ToString());
                ((CUITutorialPopup)(UIManager.Instance.CurrentPopupUI)).ImageSwap(CUITutorialPopup.TutorialState.STORY);
                ((CUITutorialPopup)(UIManager.Instance.CurrentPopupUI)).TimerHide();
            }

            //팝업 UI 닫기
            if (Input.GetKeyDown(KeyCode.O))
            {
                UIManager.Instance.ClosePopupUI();
            }
            #endregion

            #region 씬 스왑 예시
            //씬UI 스왑 앞에 CreateSceneUI가 된 UI를 인덱스를 통해 변경 가능
            if (Input.GetKeyDown(KeyCode.Q))
            {
                UIManager.Instance.SceneUISwap(1);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                UIManager.Instance.SceneUISwap(0);
            }
            #endregion

            #region 씬 넘어갈 시 무조건 호출해주어야 하는 함수들
            if (Input.GetKeyDown(KeyCode.I))
            {
                //팝업 UI 파괴 
                UIManager.Instance.ClearUIObj();
                //씬 UI 파괴
                UIManager.Instance.ClearSceneUI();
            }

            #endregion



            //노트 값 설정
            //((CUIEventPanel)UIManager.Instance.CurSceneUI).playerNotes[1].Show();
            //프로그래스 바의 맥스 값 설정
            ((CUIEventPanel)UIManager.Instance.CurSceneUI).progressBar.MaxProgress = 100f;
            //프로그래스 바 값 수정
            ((CUIEventPanel)UIManager.Instance.CurSceneUI).progressBar.FillProgressBar(10f);
        }
    }
}
