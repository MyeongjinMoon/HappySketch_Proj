
namespace JongJin
{
    
    public enum EGameState
    {
        CUTSCENE,
        RUNNING,
        TAILMISSION,
        FIRSTMISSION,
        SECONDMISSION,
        THIRDMISSION,

        END
    }

    public class GameStateContext
    {
        public IGameState CurrentState { get; set; }
        private readonly GameSceneController controller;
        public GameStateContext(GameSceneController controller)
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