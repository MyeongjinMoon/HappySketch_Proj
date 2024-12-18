using HakSeung;
using Jaehoon;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

		[Header("Etc..")]
		[SerializeField] private Fade fade;

		private GameStateContext gameStateContext;
		private EGameState curState = EGameState.CUTSCENE;

		public EGameState CurState { get { return curState; } }

        [SerializeField] private AudioClip missionroomBackgroundMusic;
        [SerializeField] private AudioClip runningStateBackgroundMusic;
        private void Awake()
		{
			UIManager.Instance.MainCanvasSetting();
            
            UIManager.Instance.UICashing(typeof(UIManager.ESceneUIType), (int)UIManager.ESceneUIType.RunningCanvas);
			UIManager.Instance.UICashing(typeof(UIManager.ESceneUIType), (int)UIManager.ESceneUIType.EventScenePanel);
            UIManager.Instance.UICashing(typeof(UIManager.ESceneUIType), (int)UIManager.ESceneUIType.TailMissionPanel);
            UIManager.Instance.UICashing(typeof(UIManager.EPopupUIType), (int)UIManager.EPopupUIType.TutorialPopupPanel);
			UIManager.Instance.UICashing(typeof(UIManager.EPopupUIType), (int)UIManager.EPopupUIType.FadePopupCanvas);
            UIManager.Instance.UICashing(typeof(UIManager.ESceneUIType), (int)UIManager.ESceneUIType.TailMissionPanel);

            UIManager.Instance.CreateSceneUI(UIManager.ESceneUIType.RunningCanvas.ToString(), (int)UIManager.ESceneUIType.RunningCanvas);
            UIManager.Instance.CreateSceneUI(UIManager.ESceneUIType.EventScenePanel.ToString(), (int)UIManager.ESceneUIType.EventScenePanel);
            UIManager.Instance.CreateSceneUI(UIManager.ESceneUIType.TailMissionPanel.ToString(), (int)UIManager.ESceneUIType.TailMissionPanel);

            cutSceneState = GetComponent<CutSceneState>();
			runningState = GetComponent<RunningState>();
			tailMissionState = GetComponent<TailMissionState>();
			firstMissionState = GetComponent<FirstMissionState>();
			secondMissionState = GetComponent<SecondMissionState>();
			thirdMissionState = GetComponent<ThirdMissionState>();

			missionGround.SetActive(false);           
			startForestGround.SetActive(true);          

			RenderSettings.skybox = skyboxNormal;      

			missionRoomVolcano.SetActive(false);
			
			gameStateContext = new GameStateContext(this);
			gameStateContext.Transition(cutSceneState);
			curState = EGameState.CUTSCENE;
		}

		private void Start()
		{
            SoundManager.instance.BackgroundMusicPlay(runningStateBackgroundMusic);
			SoundManager.instance.SFXPlay("Sounds/JungleAmbience");
        }

		private void Update()
		{
			if (!fade.IsFinished && curState != EGameState.CUTSCENE)
				return;
			switch (curState)
			{
				case EGameState.CUTSCENE:
					if (cutSceneState.IsFinishedCutScene())
						UpdateState(EGameState.RUNNING);
					break;
				case EGameState.RUNNING:
					if (runningState.IsFirstMissionTriggered())
						UpdateState(EGameState.FIRSTMISSION);
					else if (runningState.IsSecondMissionTriggered())
						UpdateState(EGameState.SECONDMISSION);
					else if (runningState.IsThirdMissionTriggered())
						UpdateState(EGameState.THIRDMISSION);
					else if (runningState.IsTailMissionTriggered())
						UpdateState(EGameState.TAILMISSION);
                    break;
				case EGameState.TAILMISSION:
					if (tailMissionState.IsFinishMission(out runningState.isMissionSuccess))
					{
						runningState.isPrevStateTail = true;
						DecreaseLife(runningState.isMissionSuccess);
						UpdateState(EGameState.RUNNING);
					}
					break;
				case EGameState.FIRSTMISSION:        
					if (firstMissionState.IsFinishMission(out runningState.isMissionSuccess))
						UpdateState(EGameState.RUNNING);
					break;
				case EGameState.SECONDMISSION:        
					if (secondMissionState.IsFinishMission(out runningState.isMissionSuccess))
						UpdateState(EGameState.RUNNING);
					break;
				case EGameState.THIRDMISSION:         
					if (thirdMissionState.IsFinishMission(out runningState.isMissionSuccess))
						UpdateState(EGameState.RUNNING);
					break;
			}
			gameStateContext.CurrentState.UpdateState();
		}
		private void UpdateState(EGameState nextState)
		{
			if (curState == nextState)
				return;
			if (curState != EGameState.CUTSCENE)
                fade.FadeInOut();
			

            StartCoroutine(WaitUpdate(nextState));
		}
		IEnumerator WaitUpdate(EGameState nextState)
		{
            if (curState != EGameState.CUTSCENE)
                yield return new WaitForSeconds(2.0f);

            curState = nextState;

            UpdateCamera(curState);
            UpdateMap(curState);
            UpdateBackgroundMusic(curState);

            switch (curState)
            {
                case EGameState.RUNNING:
                    gameStateContext.Transition(runningState);
                    UIManager.Instance.SwapSceneUI((int)UIManager.ESceneUIType.RunningCanvas);
                    break;
                case EGameState.TAILMISSION:
                    gameStateContext.Transition(tailMissionState);
                    UIManager.Instance.SwapSceneUI((int)UIManager.ESceneUIType.TailMissionPanel);
                    SoundManager.instance.SFXPlay("Sounds/TailMissionDinosaurRoar");
                    break;
                case EGameState.FIRSTMISSION:
                    gameStateContext.Transition(firstMissionState);
                    UIManager.Instance.SwapSceneUI((int)UIManager.ESceneUIType.EventScenePanel);
                    break;
                case EGameState.SECONDMISSION:
                    gameStateContext.Transition(secondMissionState);
                    UIManager.Instance.SwapSceneUI((int)UIManager.ESceneUIType.EventScenePanel);
                    break;
                case EGameState.THIRDMISSION:
                    gameStateContext.Transition(thirdMissionState);
                    UIManager.Instance.SwapSceneUI((int)UIManager.ESceneUIType.EventScenePanel);
                    break;
            }
			yield return null;
        }

		private void UpdateCamera(EGameState curState)
		{
			if (curState == EGameState.CUTSCENE || curState == EGameState.RUNNING)
				return;

			curLookAt.transform.position = lookAt[(int)curState].transform.position;
			curFollow.transform.position = follow[(int)curState].transform.position;
		}
		 
		private void DecreaseLife(bool isSuccessMission)
		{
            if (isSuccessMission)
				return;
            runningState.Life--;

			if (runningState.Life <= 0)
			{
                PlayerPrefs.SetInt("ClearStage", 0);
				fade.FadeOut();
                StartCoroutine(SceneManagerExtended.Instance.GoToEndingScene());
            }
        }

        private void UpdateMap(EGameState curState)
        {
            switch (curState)
            {
                case EGameState.CUTSCENE:
                case EGameState.RUNNING:
                    missionGround.SetActive(false);
                    missionRoomVolcano.SetActive(false);
                    break;
                case EGameState.TAILMISSION:
                case EGameState.FIRSTMISSION:
                    missionGround.SetActive(true);
                    missionRoomVolcano.SetActive(true);
                    startForestGround.SetActive(false);
                    break;
                case EGameState.SECONDMISSION:
                    missionGround.SetActive(true);
                    missionRoomVolcano.SetActive(true);
                    break;
                case EGameState.THIRDMISSION:
                    missionGround.SetActive(true);
                    missionRoomVolcano.SetActive(true);
                    RenderSettings.skybox = skyboxVolcano;
                    break;
            }
        }

        private void UpdateBackgroundMusic(EGameState curState)
        {
            switch (curState)
            {
                case EGameState.CUTSCENE:
                case EGameState.RUNNING:
                    SoundManager.instance.BackgroundMusicPlay(runningStateBackgroundMusic);
                    break;
                case EGameState.TAILMISSION:
                case EGameState.FIRSTMISSION:
                case EGameState.SECONDMISSION:
                case EGameState.THIRDMISSION:
                    SoundManager.instance.BackgroundMusicPlay(missionroomBackgroundMusic);
                    break;
            }
        }


    }
}