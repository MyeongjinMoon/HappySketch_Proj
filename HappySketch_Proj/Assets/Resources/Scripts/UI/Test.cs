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
            //����� UI �̸� Cashing �� ������ �� �ʹݿ� ������ ���־�� �Ѵ�.
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.ESceneUIType), (int)UIManager.ESceneUIType.RunningCanvas);
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.ESceneUIType), (int)UIManager.ESceneUIType.EventScenePanel);
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.ESceneUIType), (int)UIManager.ESceneUIType.TailMissionPanel);

            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.EPopupUIType), (int)UIManager.EPopupUIType.TutorialPopupPanel);
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.EPopupUIType), (int)UIManager.EPopupUIType.FadePopupCanvas);


            testBool = false;

        }

        void Start()
        {
            //�� UI ���� ����� ������ �̸� �غ��� ����. Default�� 0���� ���� ���·� ������
            UIManager.Instance.CreateSceneUI(UIManager.ESceneUIType.EventScenePanel.ToString(), (int)UIManager.ESceneUIType.EventScenePanel);
            UIManager.Instance.CreateSceneUI(UIManager.ESceneUIType.RunningCanvas.ToString(), (int)UIManager.ESceneUIType.RunningCanvas);
            UIManager.Instance.CreateSceneUI(UIManager.ESceneUIType.TailMissionPanel.ToString(), (int)UIManager.ESceneUIType.TailMissionPanel);

        }

        // Update is called once per frame
        void Update()
        {
            #region �˾� UI ��� ����
            //�˾� UI ���̱�
            if (Input.GetKeyDown(KeyCode.P))
            {
                //Ʃ�丮�� �̹��� ����
                UIManager.Instance.ShowPopupUI(UIManager.EPopupUIType.TutorialPopupPanel.ToString());
                ((CUITutorialPopup)(UIManager.Instance.CurrentPopupUI)).ImageSwap(CUITutorialPopup.EventResult.SUCCESS);
                ((CUITutorialPopup)(UIManager.Instance.CurrentPopupUI)).TimerHide();

                //���̵��� ���̵� �ƿ�
                //UIManager.Instance.ShowPopupUI(UIManager.EPopupUIType.FadePopupCanvas.ToString());
                
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                UIManager.Instance.ShowPopupUI(UIManager.EPopupUIType.FadePopupCanvas.ToString());
            }

            //�˾� UI �ݱ�
            if (Input.GetKeyDown(KeyCode.O))
            {
                UIManager.Instance.ClosePopupUI();
            }
            #endregion

            #region �� ���� ����
            //��UI ���� �տ� CreateSceneUI�� �� UI�� �ε����� ���� ���� ����
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

            #region �� �Ѿ �� ������ ȣ�����־�� �ϴ� �Լ���
            if (Input.GetKeyDown(KeyCode.I))
            {
                //�˾� UI �ı� 
                UIManager.Instance.ClearUIObj();
                //�� UI �ı�
                UIManager.Instance.ClearSceneUI();
            }

            #endregion

            #region �˾� ���� �׽�Ʈ
            if(Input.GetKeyDown(KeyCode.J))
            {
                if(testBool)
                    UIManager.Instance.SwapPopupUI(UIManager.EPopupUIType.FadePopupCanvas.ToString());
                else
                    UIManager.Instance.SwapPopupUI(UIManager.EPopupUIType.TutorialPopupPanel.ToString());
                testBool = !testBool;
            }
            #endregion




            /*//��Ʈ �� ����
            //((CUIEventPanel)UIManager.Instance.CurSceneUI).playerNotes[1].Show();
            //���α׷��� ���� �ƽ� �� ����
            ((CUIEventPanel)UIManager.Instance.CurSceneUI).progressBar.MaxProgress = 100f;
            //���α׷��� �� �� ����
            ((CUIEventPanel)UIManager.Instance.CurSceneUI).progressBar.FillProgressBar(10f);*/
        }
    }
}
