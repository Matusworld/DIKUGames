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
    public class GameWon : IGameState {
        private static GameWon instance;
        private Entity backGroundImage;
        private LinkedList<Button> buttons;
        public LinkedListNode<Button> activeButton { get; private set; }

        private Text display;

        public GameWon() { 
            Init();
        }

        private void Init() {
            //Initialize backGroundImage
            Vec2F imagePos = new Vec2F(0f,0f);
            Vec2F imageExtent = new Vec2F(1f, 1f);
            StationaryShape shape = new StationaryShape(imagePos, imageExtent);
            IBaseImage image = new Image(Path.Combine( ProjectPath.getPath(),
                "Breakout","Assets", "Images", "shipit_titlescreen.png"));
            backGroundImage = new Entity(shape, image);

            //Initialize Buttons
            Vec3I passiveColor = new Vec3I(192,192,192);
            Vec3I activeColor = new Vec3I(255,160,0);
            //New Game
            MainMenuButton MainMenuButton = new MainMenuButton("Main Menu", new Vec2F(0.2f, 0.4f),
                new Vec2F(0.3f, 0.3f), activeColor, passiveColor);
            //Quit Game
            QuitGameButton quitGameButton = new QuitGameButton("Quit", new Vec2F(0.2f, 0.3f),
                new Vec2F(0.3f, 0.3f), activeColor, passiveColor);

            MainMenuButton.SetActive();
            quitGameButton.SetPassive();

            buttons = new LinkedList<Button>();
            buttons.AddLast(MainMenuButton);
            buttons.AddLast(quitGameButton);
            activeButton = buttons.First;

            display = new Text("GAME WON!", new Vec2F (0.38f, 0.2f), new Vec2F(0.3f, 0.3f));
            display.SetColor(System.Drawing.Color.Gold);
        }

        public static GameWon GetInstance() {
            return GameWon.instance ?? (GameWon.instance = new GameWon());
        }

        public void ResetState() { 
            Init();
         }

        public void UpdateState() {}

        public void RenderState() {
            backGroundImage.RenderEntity();

            foreach (Button button in buttons) {
                button.Render();
            }

            display.RenderText();
        }

        private void ButtonMover(KeyboardKey key) {
            activeButton.Value.SetPassive();

            if (key == KeyboardKey.Up && activeButton == buttons.First) {
                activeButton = buttons.Last;
            }
            else if (key == KeyboardKey.Down && activeButton == buttons.Last) {
                activeButton = buttons.First;
            }
            else if (key == KeyboardKey.Up) {
                activeButton = activeButton.Previous;
            }
            else if (key == KeyboardKey.Down) {
                activeButton = activeButton.Next;
            }

            activeButton.Value.SetActive();
        }
        
        public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
            switch (action) {
                case KeyboardAction.KeyRelease:
                    switch (key) {
                        case KeyboardKey.Down:
                        case KeyboardKey.Up:
                            ButtonMover(key);
                            break;
                        case KeyboardKey.Enter:
                            activeButton.Value.Action();
                            break;
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