using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Input;
using DIKUArcade.Events;
using System.Collections.Generic;

namespace Breakout {
    public class Game : DIKUGame, IGameEventProcessor {
        
        public Game(WindowArgs winArgs) : base(winArgs) {
            window.SetKeyEventHandler(KeyHandler);
            window.SetClearColor(System.Drawing.Color.Navy);

            BreakoutBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.WindowEvent} );
            BreakoutBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
        }

        private void KeyHandler(KeyboardAction action, KeyboardKey key) {
            if (action == KeyboardAction.KeyPress) {
                switch (key) {

                }
            }
            else if (action == KeyboardAction.KeyRelease) {
                switch (key) {
                    case KeyboardKey.Escape:
                        BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                            EventType = GameEventType.WindowEvent, Message = "CLOSE_WINDOW"
                        } );
                        break;
                }
            }
        }

        public override void Render() {

        }

        public override void Update() {
            BreakoutBus.GetBus().ProcessEvents();
        }

        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.Message == "CLOSE_WINDOW") {
                window.CloseWindow();
            }
        }
    }
}