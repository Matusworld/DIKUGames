using DIKUArcade.EventBus;
using DIKUArcade.State;

namespace Galaga.GalagaStates {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }
        private IGameState tempState;

        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);

            ActiveState = MainMenu.GetInstance();
        }

        public void SwitchState(GameStateType stateType) {
            switch(stateType) {
                case GameStateType.GameRunning:
                    ActiveState = new GameRunning();
                    break;
                case GameStateType.GamePause:
                    ActiveState = new GamePaused();
                    break;
                case GameStateType.MainMenu:
                    ActiveState = MainMenu.GetInstance();
                    break;
                default:
                    break;
            }
        }

        //Process Event method
        public void ProcessEvent(GameEventType type, GameEvent<object> gameEvent) {
            if (type == GameEventType.InputEvent) {
                switch (gameEvent.Parameter1) {
                    case "KEY_PRESS":
                        ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);
                        break;
                    case "KEY_RELEASE":
                        ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);
                        break;
                    default:
                        break;
                }
            }
            if (type == GameEventType.GameStateEvent) {
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
                                        "KEY_RELEASE", ""
                                    )
                                );
                                break;
                            case "GAME_PAUSED":
                                tempState = ActiveState;
                                SwitchState(GameStateType.GamePause);
                                break;
                            case "GAME_CONTINUE":
                                ActiveState = tempState;
                                break;
                            case "GAME_MAINMENU":
                                SwitchState(GameStateType.MainMenu);
                                break;
                            default:
                                break;
                        } break;
                    default:
                        break;
                }
            }
        }
    }
}