using HakSeung;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace JongJin
{
    public class GameSceneController : MonoBehaviour
    {
        [Header("GameScene States")]
        [SerializeField] private CutSceneState cutSceneState;
        [SerializeField] private RunningState runningState;
        [SerializeField] private TailMissionState tailMissionState;
        [SerializeField] private FirstMissionState firstMissionState;
        [SerializeField] private SecondMissionState secondMissionState;
        [SerializeField] private ThirdMissionState thirdMissionState;

        
        [Header("MissionCamera Set")]
        [SerializeField] private GameObject curLookAt;
        [SerializeField] private GameObject curFollow;
        [SerializeField] private GameObject []lookAt;
        [SerializeField] private GameObject []follow;

        [Header("BackGround Set")]
        [SerializeField] private GameObject missionGround;
        [SerializeField] private GameObject startForestGround;

        [SerializeField] Material skyboxNormal;
        [SerializeField] Material skyboxVolcano;

        [SerializeField] private GameObject missionRoomVolcano;

        private GameStateContext gameStateContext;
        private EGameState curState;
        public EGameState CurState { get { return curState; } }

        private void Awake()
        {
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.ESceneUIType), (int)UIManager.ESceneUIType.RunningCanvas);
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.ESceneUIType), (int)UIManager.ESceneUIType.EventScenePanel);

            cutSceneState = GetComponent<CutSceneState>();
            runningState = GetComponent<RunningState>();
            tailMissionState = GetComponent<TailMissionState>();
            firstMissionState = GetComponent<FirstMissionState>();
            secondMissionState = GetComponent<SecondMissionState>();
            thirdMissionState = GetComponent<ThirdMissionState>();

            missionGround.SetActive(false);           // missionGround�� �ʱ⿡�� ���� ����
            startForestGround.SetActive(true);          // startForestGround�� �ʱ⿡ �ѵ� ����

            RenderSettings.skybox = skyboxNormal;       // �ʱ� skybox�� skyboxNormal

            missionRoomVolcano.SetActive(false);
            
            gameStateContext = new GameStateContext(this);
            //gameStateContext.Transition(cutSceneState);
            //curState = EGameState.CUTSCENE;
            gameStateContext.Transition(runningState);
            curState = EGameState.RUNNING;

        }

        private void Start()
        {
            UIManager.Instance.CreateSceneUI(UIManager.ESceneUIType.RunningCanvas.ToString(), (int)UIManager.ESceneUIType.RunningCanvas);
            UIManager.Instance.CreateSceneUI(UIManager.ESceneUIType.EventScenePanel.ToString(), (int)UIManager.ESceneUIType.EventScenePanel);


        }

        private void Update()
        {
            switch (curState)
            {
                case EGameState.CUTSCENE:
                    if (cutSceneState.IsFinishedCutScene())
                        UpdateState(EGameState.RUNNING);
                    break;
                case EGameState.RUNNING:
                    if (runningState.IsFirstMissionTriggered())
                    {
                        UpdateState(EGameState.FIRSTMISSION);
                        missionGround.SetActive(true);                // FirstMission�� �����ϸ� missionGround�� ����
                        startForestGround.SetActive(false);             // FirstMission�� �����ϸ� startForestGround�� ����
                        missionRoomVolcano.SetActive(true);
                    }
                    else if (runningState.IsSecondMissionTriggered())
                    {
                        UpdateState(EGameState.SECONDMISSION);
                        missionGround.SetActive(true);
                        missionRoomVolcano.SetActive(true);
                        RenderSettings.skybox = skyboxVolcano;          // SecondMission ���� �� skybox�� �Ӱ� ����
                    }
                    else if (runningState.IsThirdMissionTriggered())
                    {
                        UpdateState(EGameState.THIRDMISSION);
                        //missionGround.SetActive(true);
                    }
                    else if (runningState.IsTailMissionTriggered())
                    {
                        UpdateState(EGameState.TAILMISSION);
                        missionGround.SetActive(true);
                        missionRoomVolcano.SetActive(true);
                        startForestGround.SetActive(false);
                    }
                    break;
                case EGameState.TAILMISSION:
                    if(tailMissionState.IsFinishMission(out runningState.isMissionSuccess))
                        UpdateState(EGameState.RUNNING);
                    break;
                case EGameState.FIRSTMISSION:           // ù ��° ���� �̼� ���¿���
                    if (firstMissionState.IsFinishMission(out runningState.isMissionSuccess))         // ù ��° ���� �̼� ������
                    { 
                        UpdateState(EGameState.RUNNING);            // ���� �������� �ٽ� �޸�
                        missionGround.SetActive(false);             // missinoGround �ٽ� ����
                        missionRoomVolcano.SetActive(false);
                    }
                    break;
                case EGameState.SECONDMISSION:          // missionGround.SetActive(false) �־����
                    if (secondMissionState.IsFinishMission(out runningState.isMissionSuccess))         // ù ��° ���� �̼� ������
                    {
                        UpdateState(EGameState.RUNNING);            // ���� �������� �ٽ� �޸�
                        missionGround.SetActive(false);             // missinoGround �ٽ� ����
                        missionRoomVolcano.SetActive(false);
                    }
                    break;
                case EGameState.THIRDMISSION:           // missionGround.SetActive(false) �־����
                    if (thirdMissionState.IsFinishMission(out runningState.isMissionSuccess))         // ù ��° ���� �̼� ������
                    {
                        UpdateState(EGameState.RUNNING);            // ���� �������� �ٽ� �޸�
                        missionGround.SetActive(false);             // missinoGround �ٽ� ����
                        missionRoomVolcano.SetActive(false);
                    }
                    break;
            }
            gameStateContext.CurrentState.UpdateState();
        }

        private void UpdateState(EGameState nextState)
        {
            if (curState == nextState)
                return;
            curState = nextState;

            UpdateCamera(curState);

            switch (curState)
            {
                case EGameState.RUNNING:
                    gameStateContext.Transition(runningState);
                    break;
                case EGameState.TAILMISSION:
                    gameStateContext.Transition(tailMissionState);
                    break;
                case EGameState.FIRSTMISSION:
                    gameStateContext.Transition(firstMissionState);
                    break;
                case EGameState.SECONDMISSION:
                    gameStateContext.Transition(secondMissionState);
                    break;
                case EGameState.THIRDMISSION:
                    gameStateContext.Transition(thirdMissionState);
                    break;
            }
        }

        private void UpdateCamera(EGameState curState)
        {
            if (curState == EGameState.CUTSCENE || curState == EGameState.RUNNING)
                return;

            curLookAt.transform.position = lookAt[(int)curState].transform.position;
            curFollow.transform.position = follow[(int)curState].transform.position;
        }

        //TODO <이학승> 씬 전환시 작동 될 코드
        //private void DestroyUI;
        
    }
}