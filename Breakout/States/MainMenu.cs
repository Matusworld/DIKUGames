using System.IO;
using System.Collections.Generic;
using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Input;
using Breakout.States.Buttons;


namespace Breakout.States {
    public class MainMenu : IGameState {
        private static MainMenu instance;
        private Entity backGroundImage;
        private LinkedList<Button> buttons;
        public LinkedListNode<Button> activeButton { get; private set; }

        public MainMenu() { 
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
            NewGameButton newGameButton = new NewGameButton("New Game", new Vec2F(0.2f, 0.4f),
                new Vec2F(0.3f, 0.3f), activeColor, passiveColor);
            //Quit Game
            QuitGameButton quitGameButton = new QuitGameButton("Quit", new Vec2F(0.2f, 0.3f),
                new Vec2F(0.3f, 0.3f), activeColor, passiveColor);

            newGameButton.SetActive();
            quitGameButton.SetPassive();

            buttons = new LinkedList<Button>();
            buttons.AddLast(newGameButton);
            buttons.AddLast(quitGameButton);
            activeButton = buttons.First;
        }

        public static MainMenu GetInstance() {
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
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