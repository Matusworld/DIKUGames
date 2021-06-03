using System.IO;

using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Input;

using Breakout.States.Buttons;


namespace Breakout.States {
    /// <summary>
    /// Singleton MainMenu state with background Image and custom Text.
    /// Has Buttons via the ButtonManager.
    /// </summary>
    public class MainMenu : IGameState {
        private static MainMenu instance;
        private Entity backGroundImage;
        private ButtonManager buttonManager;

        public MainMenu() { 
            buttonManager = new ButtonManager();

            //Initialize backGroundImage
            Vec2F imagePos = new Vec2F(0f,0f);
            Vec2F imageExtent = new Vec2F(1f, 1f);
            StationaryShape shape = new StationaryShape(imagePos, imageExtent);
            IBaseImage image = new Image(Path.Combine( ProjectPath.getPath(),
                "Breakout","Assets", "Images", "shipit_titlescreen.png"));
            backGroundImage = new Entity(shape, image);

            //Add buttons
            //New Game
            buttonManager.AddButtonLast(ButtonTypes.NewGame, new Vec2F(0.2f, 0.4f),
                new Vec2F(0.3f, 0.3f));
            //Quit Game
            buttonManager.AddButtonLast(ButtonTypes.QuitGame, new Vec2F(0.2f, 0.3f),
                new Vec2F(0.3f, 0.3f));

            buttonManager.SetFirstButtonActive();
        }

        public static MainMenu GetInstance() {
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
        }

        public void ResetState() { 
            buttonManager.Reset();
         }

        public void UpdateState() {}

        public void RenderState() {
            backGroundImage.RenderEntity();

            buttonManager.RenderButtons();
        }

        public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
            switch (action) {
                case KeyboardAction.KeyRelease:
                    switch (key) {
                        case KeyboardKey.Down:
                        case KeyboardKey.Up:
                            buttonManager.ButtonMover(key);
                            break;
                        case KeyboardKey.Enter:
                            buttonManager.ActiveButtonAction();
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