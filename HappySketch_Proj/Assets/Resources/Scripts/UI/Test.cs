using HakSeung;
using JongJin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HakSeung
{
    public class Test : MonoBehaviour
    {
        private bool testBool;
        private int index = 3;
        private void Awake()
        {
            UIManager.Instance.MainCanvasSetting();
            //사용할 UI 미리 Cashing 이 과정을 씬 초반에 무조건 해주어야 한다.
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.ESceneUIType), (int)UIManager.ESceneUIType.RunningCanvas);
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.ESceneUIType), (int)UIManager.ESceneUIType.EventScenePanel);
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.ESceneUIType), (int)UIManager.ESceneUIType.TailMissionPanel);

            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.EPopupUIType), (int)UIManager.EPopupUIType.TutorialPopupPanel);
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.EPopupUIType), (int)UIManager.EPopupUIType.FadePopupCanvas);


            testBool = false;

        }

        void Start()
        {
            //씬 UI 생성 사용할 씬들을 미리 준비해 놓기. Default로 0번은 켜진 상태로 유지됨
            UIManager.Instance.CreateSceneUI(UIManager.ESceneUIType.EventScenePanel.ToString(), (int)UIManager.ESceneUIType.EventScenePanel);
            UIManager.Instance.CreateSceneUI(UIManager.ESceneUIType.RunningCanvas.ToString(), (int)UIManager.ESceneUIType.RunningCanvas);
            UIManager.Instance.CreateSceneUI(UIManager.ESceneUIType.TailMissionPanel.ToString(), (int)UIManager.ESceneUIType.TailMissionPanel);

        }

        // Update is called once per frame
        void Update()
        {
            #region 팝업 UI 사용 예시
            //팝업 UI 보이기
            if (Input.GetKeyDown(KeyCode.P))
            {
                //튜토리얼 이미지 스왑
                UIManager.Instance.ShowPopupUI(UIManager.EPopupUIType.TutorialPopupPanel.ToString());
                ((CUITutorialPopup)(UIManager.Instance.CurrentPopupUI)).ImageSwap(CUITutorialPopup.EventResult.SUCCESS);
                ((CUITutorialPopup)(UIManager.Instance.CurrentPopupUI)).TimerHide();

                //페이드인 페이드 아웃
                //UIManager.Instance.ShowPopupUI(UIManager.EPopupUIType.FadePopupCanvas.ToString());
                
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                UIManager.Instance.ShowPopupUI(UIManager.EPopupUIType.FadePopupCanvas.ToString());
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                UIManager.Instance.SceneUISwap(2);
            }

            if(Input.GetKeyDown(KeyCode.F))
                ((CUITailMissionPanel)UIManager.Instance.CurSceneUI).OnFailedEvent(3 - index--);
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

            #region 팝업 스왑 테스트
            if(Input.GetKeyDown(KeyCode.J))
            {
                if(testBool)
                    UIManager.Instance.SwapPopupUI(UIManager.EPopupUIType.FadePopupCanvas.ToString());
                else
                    UIManager.Instance.SwapPopupUI(UIManager.EPopupUIType.TutorialPopupPanel.ToString());
                testBool = !testBool;
            }
            #endregion




            /*//노트 값 설정
            //((CUIEventPanel)UIManager.Instance.CurSceneUI).playerNotes[1].Show();
            //프로그래스 바의 맥스 값 설정
            ((CUIEventPanel)UIManager.Instance.CurSceneUI).progressBar.MaxProgress = 100f;
            //프로그래스 바 값 수정
            ((CUIEventPanel)UIManager.Instance.CurSceneUI).progressBar.FillProgressBar(10f);*/
        }
    }
}
