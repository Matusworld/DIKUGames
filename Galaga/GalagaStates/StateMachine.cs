using DIKUArcade.EventBus;
using DIKUArcade.State;
namespace Galaga.GalagaStates {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }

        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);

            ActiveState = MainMenu.GetInstance();
        }

        public void SwitchState(GameStateType stateType) {
            switch(stateType) {
                case GameStateType.GameRunning:
                    ActiveState = GameRunning.GetInstance();
                    break;
                default:
                    break;
            } 
        }

        public void ProcessEvent(GameEventType type, GameEvent<object> gameEvent) {
            
            if (type == GameEventType.GameStateEvent) {
                System.Console.WriteLine(gameEvent.Message);
                System.Console.WriteLine(gameEvent.Parameter1);
                switch (gameEvent.Message) {
                    case "CHANGE_STATE":
                        switch (gameEvent.Parameter1) {
                            case "GAME_RUNNING":
                                SwitchState(GameStateType.GameRunning);
                                break;
                            case "GAME_QUIT":
                                GalagaBus.GetBus().RegisterEvent(
                                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                                        GameEventType.InputEvent,
                                        this,
                                        "KEY_ESCAPE",
                                        "KEY_RELEASE", ""));
                                break;
                        }
                        break;
                }
            } 
            if (type == GameEventType.InputEvent) {
                switch (gameEvent.Parameter1) {
                    case "KEY_PRESS":
                        ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);
                        break;

                    case "KEY_RELEASE":
                        ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);
                        break;
                }
            }
        }
    }
} 