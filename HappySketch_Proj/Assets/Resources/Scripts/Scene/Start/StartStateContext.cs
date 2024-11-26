using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JongJin
{
    public enum EStartGameState
    {
        STORYDESCRIPTION,
        STORYCUTSCENE,
        TUTORIALDESCRIPTION,
        TUTORIALACTION,

        END
    }
    public class StartStateContext
    {
        public IGameState CurrentState { get; set; }
        private readonly StartSceneController controller;
        public StartStateContext(StartSceneController controller)
        {
            this.controller = controller;
        }

        public void Transition(IGameState gameState)
        {
            if (CurrentState != null)
                CurrentState.ExitState();
            CurrentState = gameState;

            CurrentState.EnterState();
        }
    }

}
