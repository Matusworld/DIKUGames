using System.IO;
using System.Collections.Generic;
using DIKUArcade.State;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Input;
using Breakout.States.Buttons;

namespace Breakout.States {
    public class GamePaused : IGameState {
        private static GamePaused instance;
        //private Text[] menuButtons;
        private Entity backGroundImage;
        //private int activeMenuButton;
        //private const int maxMenuButtons = 2;

        private LinkedList<Button> buttons;
        private LinkedListNode<Button> activeButton;


        public GamePaused() {
            Init();
        }

        public void Init() {
            //Initialize backGroundImage
            Vec2F imagePos = new Vec2F(0f,0f);
            Vec2F imageExtent = new Vec2F(1f, 1f);
            StationaryShape shape = new StationaryShape(imagePos, imageExtent);
            IBaseImage image = new Image(Path.Combine( ProjectPath.getPath(),
                "Breakout","Assets", "Images", "shipit_titlescreen.png"));
            backGroundImage = new Entity(shape, image);

            // Initialie Buttons
            Vec3I passiveColor = new Vec3I(192,192,192);
            Vec3I activeColor = new Vec3I(255,160,0);
            //New Game
            ContinueButton continueButton = new ContinueButton("Continue", new Vec2F(0.2f, 0.4f),
                new Vec2F(0.3f, 0.3f), activeColor, passiveColor);
            //Quit Game
            MainMenuButton mainMenuButton = new MainMenuButton("Quit", new Vec2F(0.2f, 0.3f),
                new Vec2F(0.3f, 0.3f), activeColor, passiveColor);

            continueButton.SetActive();
            mainMenuButton.SetPassive();
            
            buttons = new LinkedList<Button>();
            buttons.AddLast(continueButton);
            buttons.AddLast(mainMenuButton);
            activeButton = buttons.First;
        }

        public static GamePaused GetInstance() {
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
        }
        /*
        private void colorButtons() {
            for ( int i = 0; i < 2; i++ ) {
                menuButtons[i].SetColor(new Vec3I(192,192,192));
            }
            
            menuButtons[activeMenuButton].SetColor(new Vec3I(255,160,0));
        } */

        public void ResetState() {
            Init();
        }

        public void UpdateState() {
            //colorButtons();
        }

        public void RenderState() {
            backGroundImage.RenderEntity();

            foreach (Button button in buttons) {
                button.Render();
            }
        }

        public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
            switch (action) {
                case KeyboardAction.KeyRelease:
                    switch (key) {
                        case KeyboardKey.Up:
                            /*
                            if (!(activeMenuButton == 0)) {
                                activeMenuButton--;
                            } */
                            activeButton.Value.SetPassive();
                            activeButton = activeButton.Previous;
                            activeButton.Value.SetActive();
                            break;
                        case KeyboardKey.Down:
                            /*
                            if (activeMenuButton < maxMenuButtons-1) {
                                activeMenuButton++;
                            } */
                            activeButton.Value.SetPassive();
                            activeButton = activeButton.Next;
                            activeButton.Value.SetActive();
                            break;
                        case KeyboardKey.Enter: {
                            /*
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
                            }*/
                            activeButton.Value.Action();
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