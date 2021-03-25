using DIKUArcade.EventBus;
using DIKUArcade.State;
namespace Galaga.GalagaStates {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }

        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);

            ActiveState = MainMenu.GetInstance();
        }

        public void SwitchState(GameStateType stateType) {
            switch(stateType) {
                case GameStateType.GameRunning:
                    ActiveState = new GameRunning();
                    break;
                default:
                    break;
            } 
        }

        public void ProcessEvent(GameEventType type, GameEvent<object> gameEvent) {
            if (type == GameEventType.GameStateEvent) {
                switch (gameEvent.Message) {
                    case "CHANGE_STATE":
                        switch (gameEvent.Parameter1) {
                            case "GAME_RUNNING":
                                SwitchState(GameStateType.GameRunning);
                                break;
                            case "GAME_QUIT":
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.InputEvent,
                                    this,
                                    "KEY_ESCAPE",
                                    "KEY_RELEASE", ""
                                );
                                break;
                        }
                        break;
                }
            } else if (type == GameEventType.InputEvent) {

            }
        }
    }
} 