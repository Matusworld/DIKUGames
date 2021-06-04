using System.IO;
using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Input;
using DIKUArcade.Events;
using Breakout.States;

namespace Breakout {
    /// <summary>
    /// Game is the highest level class of the game.
    /// Holds the StateMachine and initialize the BreakoutBus.
    /// </summary>
    public class Game : DIKUGame, IGameEventProcessor {
        private StateMachine stateMachine;

        public Game(WindowArgs winArgs) : base(winArgs) {
            window.SetKeyEventHandler(KeyHandler);

            BreakoutBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.WindowEvent, GameEventType.ControlEvent,
                GameEventType.GameStateEvent } );
            BreakoutBus.GetBus().Subscribe(GameEventType.WindowEvent, this);  

            stateMachine = new StateMachine();
        }

        /// <summary>
        /// KeyHandler sends input from Keyboard on to the ActiveState of StateMachine
        /// </summary>
        /// <param name="action">Describes how the key is interacted with by the user</param>
        /// <param name="key">The signature of the key that has been pressed</param>
        private void KeyHandler(KeyboardAction action, KeyboardKey key) { 
            if (action == KeyboardAction.KeyRelease && key == KeyboardKey.Escape) {
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.WindowEvent, Message = "CLOSE_WINDOW" });
            } else {
                stateMachine.ActiveState.HandleKeyEvent(action, key);
            }
        }

        public override void Render() {
            stateMachine.ActiveState.RenderState();
        }

        public override void Update() {
            stateMachine.ActiveState.UpdateState();

            BreakoutBus.GetBus().ProcessEventsSequentially();
        }

        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.WindowEvent) {
                if (gameEvent.Message == "CLOSE_WINDOW") {
                    window.CloseWindow();
                }
            }
        }
    }
}