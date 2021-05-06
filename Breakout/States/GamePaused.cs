using DIKUArcade.State;
using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Input;

namespace Breakout.States {
    public class GamePaused : IGameState {
        private Text[] menuButtons;
        private int activeMenuButton;
        private const int maxMenuButtons = 2;
        public GamePaused() {
            InitializeGameState();
        }

        public void GameLoop() {}

        public void InitializeGameState() {
            // Initialie Buttons
            Text ContinueButton = new Text("Continue", new Vec2F(0.3f, 0.5f),
                new Vec2F(0.3f, 0.2f));
            Text quitGameButton = new Text("Main Menu", new Vec2F(0.3f, 0.45f),
                new Vec2F(0.3f, 0.2f));

            menuButtons = new Text[maxMenuButtons] { ContinueButton, quitGameButton };
        }

        private void colorButtons() {
            for ( int i = 0; i < 2; i++ ) {
                menuButtons[i].SetColor(new Vec3I(255,0,0));
            }
            
            menuButtons[activeMenuButton].SetColor(new Vec3I(0,255,0));
        }

        public void ResetState() {}

        public void UpdateState() {
            colorButtons();
        }

        public void RenderState() {
            foreach (Text button in menuButtons) {
                button.RenderText();
            }
        }
        /*
        private void KeyRelease(string key) {
            switch (key) {
                case "KEY_UP":
                    if (!(activeMenuButton == 0)) {
                        activeMenuButton--;
                    }
                    break;
                case "KEY_DOWN":
                    if (activeMenuButton < maxMenuButtons-1) {
                        activeMenuButton++;
                    }
                    break;
                case "KEY_ENTER": {
                    if (activeMenuButton == 0) { // Continue pressed
                        GalagaBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "GAME_CONTINUE", ""));
                    } else if (activeMenuButton == 1) { // Main Menu pressed
                        GalagaBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "GAME_MAINMENU", ""));
                    }
                    break;
                }
                default:
                    break;
            }
        }
        */

        public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
            switch (action) {
                case KeyboardAction.KeyRelease:
                    switch (key) {
                        case KeyboardKey.Up:
                            if (!(activeMenuButton == 0)) {
                                activeMenuButton--;
                            }
                            break;
                        case KeyboardKey.Down:
                            if (activeMenuButton < maxMenuButtons-1) {
                                activeMenuButton++;
                            }
                            break;
                        case KeyboardKey.Enter: {
                            if (activeMenuButton == 0) { // Continue pressed
                                BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                                    EventType = GameEventType.GameStateEvent,
                                    Message = "CHANGE_STATE",
                                    StringArg1 = "GAME_CONTINUE" });
                            } 
                            else if (activeMenuButton == 1) { // Main Menu pressed
                                BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                                    EventType = GameEventType.GameStateEvent,
                                    Message = "CHANGE_STATE",
                                    StringArg1 = "GAME_MAINMENU" });
                            }
                            break;
                        }
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}