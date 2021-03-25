using DIKUArcade.EventBus;
using DIKUArcade.State;

namespace Galaga.GameStates {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }

        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);

            ActiveState = MainMenu.GetInstance();
        }

        public void SwitchState(GameStateType stateType) {
            switch(stateType) {
                
            }
        }

        //Process Event method
    }
} 