using System.IO;
using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Input;
using DIKUArcade.Events;
using Breakout.States;

namespace Breakout {
    public class Game : DIKUGame, IGameEventProcessor {
        private StateMachine stateMachine;

        public Game(WindowArgs winArgs) : base(winArgs) {
            window.SetKeyEventHandler(KeyHandler);
            //window.SetClearColor(System.Drawing.Color.DarkGray);

            BreakoutBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.WindowEvent, GameEventType.PlayerEvent, GameEventType.ControlEvent,
                GameEventType.GameStateEvent, GameEventType.MovementEvent } );
            BreakoutBus.GetBus().Subscribe(GameEventType.WindowEvent, this);  

            stateMachine = new StateMachine();
        }

        //KeyHandler sends singal on to ActiveState of StateMachine
        private void KeyHandler(KeyboardAction action, KeyboardKey key) { 
            if (action == KeyboardAction.KeyRelease && key == KeyboardKey.Escape) {
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.WindowEvent, Message = "CLOSE_WINDOW" });
            }
            else {
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