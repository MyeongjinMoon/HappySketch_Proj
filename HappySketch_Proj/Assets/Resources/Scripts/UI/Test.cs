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
            //����� UI �̸� Cashing �� ������ �� �ʹݿ� ������ ���־�� �Ѵ�.
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.ETestType), (int)UIManager.ETestType.RunningCanvas);
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.ETestType), (int)UIManager.ETestType.EventScenePanel);
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.ETestType), (int)UIManager.ETestType.TutorialPopupPanel);

        }

        void Start()
        {
            //�� UI ���� ����� ������ �̸� �غ��� ����. Default�� 0���� ���� ���·� ������
            UIManager.Instance.CreateSceneUI(UIManager.ETestType.EventScenePanel.ToString(), (int)UIManager.ETestType.RunningCanvas);
            UIManager.Instance.CreateSceneUI(UIManager.ETestType.RunningCanvas.ToString(), (int)UIManager.ETestType.EventScenePanel);
        }

        // Update is called once per frame
        void Update()
        {
            #region �˾� UI ��� ����
            //�˾� UI ���̱�
            if (Input.GetKeyDown(KeyCode.P))
            {
                UIManager.Instance.ShowPopupUI(UIManager.ETestType.TutorialPopupPanel.ToString());
                ((CUITutorialPopup)(UIManager.Instance.CurrentPopupUI)).ImageSwap(CUITutorialPopup.TutorialState.STORY);
                ((CUITutorialPopup)(UIManager.Instance.CurrentPopupUI)).TimerHide();
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



            //��Ʈ �� ����
            //((CUIEventPanel)UIManager.Instance.CurSceneUI).playerNotes[1].Show();
            //���α׷��� ���� �ƽ� �� ����
            ((CUIEventPanel)UIManager.Instance.CurSceneUI).progressBar.MaxProgress = 100f;
            //���α׷��� �� �� ����
            ((CUIEventPanel)UIManager.Instance.CurSceneUI).progressBar.FillProgressBar(10f);
        }
    }
}
