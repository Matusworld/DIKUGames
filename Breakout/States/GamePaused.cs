using System.IO;
using DIKUArcade.State;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Input;

namespace Breakout.States {
    public class GamePaused : IGameState {
        private static GamePaused instance;
        private Text[] menuButtons;
        private Entity backGroundImage;
        private int activeMenuButton;
        private const int maxMenuButtons = 2;

        public GamePaused() {
            Init();
        }

        public void Init() {
            //Initialize backGroundImage
            Vec2F imagePos = new Vec2F(0f,0f);
            Vec2F imageExtent = new Vec2F(1f, 1f);
            StationaryShape shape = new StationaryShape(imagePos, imageExtent);
            IBaseImage image = new Image(Path.Combine( ProjectPath.getPath(),
                "Assets", "Images", "shipit_titlescreen.png"));
            backGroundImage = new Entity(shape, image);

            // Initialie Buttons
            Text ContinueButton = new Text("Continue", new Vec2F(0.2f, 0.4f),
                new Vec2F(0.3f, 0.3f));
            Text quitGameButton = new Text("Main Menu", new Vec2F(0.2f, 0.3f),
                new Vec2F(0.3f, 0.3f));

            menuButtons = new Text[maxMenuButtons] { ContinueButton, quitGameButton };
        }

        public static GamePaused GetInstance() {
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
        }

        private void colorButtons() {
            for ( int i = 0; i < 2; i++ ) {
                menuButtons[i].SetColor(new Vec3I(192,192,192));
            }
            
            menuButtons[activeMenuButton].SetColor(new Vec3I(255,160,0));
        }

        public void ResetState() {
            Init();
        }

        public void UpdateState() {
            colorButtons();
        }

        public void RenderState() {
            backGroundImage.RenderEntity();

            foreach (Text button in menuButtons) {
                button.RenderText();
            }
        }

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