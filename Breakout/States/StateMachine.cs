using DIKUArcade.Events;
using DIKUArcade.State;

namespace Breakout.States {
    public class StateMachine : IGameEventProcessor {
        public IGameState ActiveState { get; private set; }
        private IGameState tempState;

        public StateMachine() {
            BreakoutBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);

            ActiveState = MainMenu.GetInstance();
        }

        private void SwitchState(GameStateType stateType) {
            switch(stateType) {
                case GameStateType.GameRunning:
                    ActiveState = GameRunning.GetInstance();
                    break;
                case GameStateType.GamePause:
                    ActiveState = GamePaused.GetInstance();
                    break;
                case GameStateType.MainMenu:
                    ActiveState = MainMenu.GetInstance();
                    break;
                default:
                    break;
            }
        }

        //Process Event method
        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.GameStateEvent) {
                switch (gameEvent.Message) {
                    case "CHANGE_STATE":
                        switch (gameEvent.StringArg1) {
                            case "GAME_RUNNING":
                                SwitchState(GameStateType.GameRunning);
                                break;
                            case "GAME_QUIT":
                                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                                    EventType = GameEventType.WindowEvent, 
                                    Message = "CLOSE_WINDOW" });
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