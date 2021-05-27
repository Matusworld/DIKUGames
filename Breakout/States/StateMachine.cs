using DIKUArcade.Events;
using DIKUArcade.State;
using DIKUArcade.Timers;

namespace Breakout.States {
    public class StateMachine : IGameEventProcessor {
        public IGameState ActiveState { get; private set; }

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
                case GameStateType.GameLost:
                    ActiveState = GameLost.GetInstance();
                    break;
                case GameStateType.GameWon:
                    ActiveState = GameWon.GetInstance();
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
                            case "GAME_NEWGAME":
                                SwitchState(GameStateType.GameRunning);
                                //reset timer before reset state
                                StaticTimer.RestartTimer();
                                //Reset state
                                ActiveState.ResetState();
                                //To properly render entities
                                ActiveState.RenderState();
                                break;
                            case "GAME_QUIT":
                                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                                    EventType = GameEventType.WindowEvent, 
                                    Message = "CLOSE_WINDOW" });
                                break;
                            case "GAME_PAUSED":
                                SwitchState(GameStateType.GamePause);
                                ActiveState.ResetState();
                                StaticTimer.PauseTimer();
                                break;
                            case "GAME_CONTINUE":
                                SwitchState(GameStateType.GameRunning);
                                StaticTimer.ResumeTimer();
                                break;
                            case "GAME_MAINMENU":
                                SwitchState(GameStateType.MainMenu);
                                ActiveState.ResetState();
                                break;
                            case "GAME_LOST":
                                SwitchState(GameStateType.GameLost);
                                ActiveState.ResetState();
                                break;
                            case "GAME_WON":
                                SwitchState(GameStateType.GameWon);
                                ActiveState.ResetState();
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