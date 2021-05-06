using DIKUArcade.State;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Input;
using System.IO;

namespace Breakout.States {
    public class MainMenu : IGameState {
        private static MainMenu instance; // = null
        private Entity backGroundImage;
        private Text[] menuButtons;
        private int activeMenuButton;
        private const int maxMenuButtons = 2;

        public MainMenu() { InitializeGameState(); }

        /// <summary>
        /// Colors all non-active buttons red while active button is green
        /// </summary>
        private void colorButtons() {
            for ( int i = 0; i < 2; i++ ) {
                menuButtons[i].SetColor(new Vec3I(255,0,0));
            }
            menuButtons[activeMenuButton].SetColor(new Vec3I(0,255,0));
        }

        public static MainMenu GetInstance() {
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
        }

        public void InitializeGameState() {
            //Initialize backGroundImage
            Vec2F imagePos = new Vec2F(0f,0f);
            Vec2F imageExtent = new Vec2F(1f, 1f);
            StationaryShape shape = new StationaryShape(imagePos, imageExtent);
            IBaseImage image = new Image(Path.Combine( ProjectPath.getPath(),
                "Assets", "Images", "shipit_titlescreen.png"));
            backGroundImage = new Entity(shape, image);

            //Initialize Buttons
            Text newGameButton = new Text("New Game", new Vec2F(0.3f, 0.5f),
                new Vec2F(0.3f, 0.2f));
            Text quitGameButton = new Text("Quit", new Vec2F(0.3f, 0.45f),
                new Vec2F(0.3f, 0.2f));


            menuButtons = new Text[maxMenuButtons] { newGameButton, quitGameButton };
        }

        public void ResetState() { 
            MainMenu.instance = new MainMenu();
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
        /*
        private void KeyRelease(KeyboardKey key) {
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
                    if (activeMenuButton == 0) { // New Game pressed
                        BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                            EventType = GameEventType.GameStateEvent, Message = "CHANGE_STATE",
                            StringArg1 = "GAME_RUNNING" });

                    } else if (activeMenuButton == 1) { // Quit pressed
                        BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                            EventType = GameEventType.GameStateEvent, Message = "CHANGE_STATE",
                            StringArg1 = "GAME_QUIT" });
                    }
                    break;
                }
                default:
                    break;
            }
        } */
        
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
                            if (activeMenuButton == 0) { // New Game pressed
                                BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                                    EventType = GameEventType.GameStateEvent, 
                                    Message = "CHANGE_STATE",
                                    StringArg1 = "GAME_RUNNING" });
                                    System.Console.WriteLine("Pressed enter on new game");

                            } else if (activeMenuButton == 1) { // Quit pressed
                                BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                                    EventType = GameEventType.GameStateEvent, 
                                    Message = "CHANGE_STATE",
                                    StringArg1 = "GAME_QUIT" });
                                    System.Console.WriteLine("Pressed enter on quit");
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