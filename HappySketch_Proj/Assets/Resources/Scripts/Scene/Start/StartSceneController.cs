using HakSeung;
using Jaehoon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HakSeung.CUITutorialPopup;
using static HakSeung.UIManager;

namespace JongJin
{
    public class StartSceneController : MonoBehaviour
    {
        [Header("StartScene States")]
        [SerializeField] private StoryDescriptionState storyDescriptionState;
        [SerializeField] private StoryCutSceneState storyCutSceneState;
        [SerializeField] private TutorialDescriptionState tutorialDescriptionState;
        [SerializeField] private TutorialActionState tutorialActionState;
        [SerializeField] private AudioClip startSceneMusic;

        private StartStateContext startStateContext;
        private EStartGameState curState;
        public EStartGameState CurState { get { return curState; } }

        private void Awake()
        {
            UIManager.Instance.MainCanvasSetting();
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.EPopupUIType), (int)UIManager.EPopupUIType.TutorialPopupPanel);
            UIManager.Instance.UICashing<GameObject>(typeof(UIManager.ESceneUIType), (int)UIManager.ESceneUIType.TutorialCheckUIPanel);

            UIManager.Instance.CreateSceneUI(ESceneUIType.TutorialCheckUIPanel.ToString());
            UIManager.Instance.CurSceneUI.Hide();

            storyDescriptionState = GetComponent<StoryDescriptionState>();
            storyCutSceneState = GetComponent<StoryCutSceneState>();
            tutorialDescriptionState = GetComponent<TutorialDescriptionState>();
            tutorialActionState = GetComponent<TutorialActionState>();

            startStateContext = new StartStateContext(this);
            startStateContext.Transition(storyDescriptionState);
            curState = EStartGameState.STORYDESCRIPTION;
        }
        private void Start()
        {
            SoundManager.instance.BackgroundMusicPlay(startSceneMusic);

        }
        private void Update()
        {
            switch (curState)
            {
                case EStartGameState.STORYDESCRIPTION:
                    if (storyDescriptionState.IsStroyDescriptionFinish())
                        UpdateState(EStartGameState.STORYCUTSCENE);
                    break;
                case EStartGameState.STORYCUTSCENE:
                    if (storyCutSceneState.IsFinishedStoryCutScene())
                    {
                        UpdateState(EStartGameState.TUTORIALDESCRIPTION);
                        UIManager.Instance.CurSceneUI.Show();
                        
                    }
                    break;
                case EStartGameState.TUTORIALDESCRIPTION:
                    if (tutorialDescriptionState.IsFinishedTutorialPopup())
                        UpdateState(EStartGameState.TUTORIALACTION);
                    break;
                case EStartGameState.TUTORIALACTION:
                    if(tutorialActionState.IsFinishedAction())
                    {
                        if(tutorialActionState.CurrentTutorialState != CUITutorialPopup.TutorialState.HEART)
                            UpdateState(EStartGameState.TUTORIALDESCRIPTION);
                        else
                           StartCoroutine(SceneManagerExtended.Instance.GoToGameScene());
                    }

                    
                    
                    break;
            }
            startStateContext.CurrentState.UpdateState();
        }
        private void UpdateState(EStartGameState nextState)
        {
            if (curState == nextState)
                return;
            curState = nextState;

            switch (curState)
            {
                case EStartGameState.STORYDESCRIPTION:
                    startStateContext.Transition(storyDescriptionState);
                    break;
                case EStartGameState.STORYCUTSCENE:
                    startStateContext.Transition(storyCutSceneState);
                    break;
                case EStartGameState.TUTORIALDESCRIPTION:
                    startStateContext.Transition(tutorialDescriptionState);
                    break;
                case EStartGameState.TUTORIALACTION:
                    startStateContext.Transition(tutorialActionState);
                    break;
            }
        }
    }
}
