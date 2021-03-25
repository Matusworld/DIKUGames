using DIKUArcade.State;
using DIKUArcade.EventBus;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;

namespace Galaga.GalagaStates {
    public class MainMenu : IGameState {
        private static MainMenu instance = null;

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

        public void GameLoop() {}

        public void InitializeGameState() {
            //Initialize backGroundImage
            Vec2F imagePos = new Vec2F(0f,0f);
            Vec2F imageExtent = new Vec2F(1f, 1f);
            StationaryShape shape = new StationaryShape(imagePos, imageExtent);
            IBaseImage image = new Image(Path.Combine(
                @"..\", "Assets", "Images", "TitleImage.png"));
            backGroundImage = new Entity(shape, image);

            //Initialize Buttons
            Text newGameButton = new Text("New Game", new Vec2F(0.3f, 0.5f),
                new Vec2F(0.2f, 0.1f));
            Text quitGameButton = new Text("Quit", new Vec2F(0.1f, 0.5f),
                new Vec2F(0.2f, 0.1f));


            menuButtons = new Text[maxMenuButtons] { newGameButton, quitGameButton };
        }

        public void UpdateGameLogic() {
            colorButtons();
        }

        public void RenderState() {
            backGroundImage.RenderEntity();

            foreach (Text button in menuButtons) {
                button.RenderText();
            }
        }

        //new game øverst, index 0
        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyAction) {
                case "KEY_RELEASE":
                    switch (keyValue) {
                        case "KEY_UP":
                            if (!(activeMenuButton == 0)) {
                                activeMenuButton--;
                            } break;
                        case "KEY_DOWN":
                            if (activeMenuButton < maxMenuButtons-1) {
                                activeMenuButton++;
                            } break;
                        case "KEY_ENTER":
                            if (activeMenuButton == 0) { //newGame
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.GameStateEvent,
                                    this,
                                    "CHANGE_STATE",
                                    "GAME_RUNNING", "");
                            }
                            else if (activeMenuButton == 1) { //quit
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.GameStateEvent,
                                    this,
                                    "CHANGE_STATE",
                                    "GAME_QUIT", "");
                            }
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