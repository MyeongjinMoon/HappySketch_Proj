using JongJin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyeongJin
{
    public enum EEndingGameState
    {
        ENTERSCENE,
        ANIMATION,
        RESULT,

        END
    }
    public class CEndingStateContext
    {
        public IGameState CurrentState { get; set; }
        private readonly CEndingSceneController controller;
        public CEndingStateContext(CEndingSceneController controller)
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