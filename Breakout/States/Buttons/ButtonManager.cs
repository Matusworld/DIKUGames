using System.Collections.Generic;

using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Input;

namespace Breakout.States.Buttons {
    public class ButtonManager {
        private Vec3I passiveColor;
        private Vec3I activeColor;
        public LinkedList<Button> Buttons  { get; private set; }
        public LinkedListNode<Button> ActiveButton { get; private set; }

        public ButtonManager() {
            //Initialize Buttons
            passiveColor = new Vec3I(192,192,192);
            activeColor = new Vec3I(255,160,0);

            Buttons = new LinkedList<Button>();
        }

        public void AddButtonLast(ButtonTypes buttonType, Vec2F pos, Vec2F extent) {
            Button button;

            switch(buttonType) {
                case ButtonTypes.Continue:
                    button = new ContinueButton(pos, extent, activeColor, 
                        passiveColor);
                    break;
                case ButtonTypes.MainMenu:
                    button = new MainMenuButton(pos, extent, activeColor, 
                        passiveColor);
                    break;
                case ButtonTypes.NewGame:
                    button = new NewGameButton(pos, extent, activeColor, 
                        passiveColor);
                    break;
                case ButtonTypes.QuitGame:
                    button = new QuitGameButton(pos, extent, activeColor, 
                        passiveColor);
                    break;
                default:
                    button = new QuitGameButton(pos, extent, activeColor, 
                        passiveColor);
                    break;
            }

            Buttons.AddLast(button);
        }

        public void ButtonMover(KeyboardKey key) {
            ActiveButton.Value.SetPassive();

            if (key == KeyboardKey.Up && ActiveButton == Buttons.First) {
                ActiveButton = Buttons.Last;
            }
            else if (key == KeyboardKey.Down && ActiveButton == Buttons.Last) {
                ActiveButton = Buttons.First;
            }
            else if (key == KeyboardKey.Up) {
                ActiveButton = ActiveButton.Previous;
            }
            else if (key == KeyboardKey.Down) {
                ActiveButton = ActiveButton.Next;
            }

            ActiveButton.Value.SetActive();
        }

        public void RenderButtons() {
            foreach (Button button in Buttons) {
                button.Render();
            }
        }

        public void Reset() {
            ActiveButton.Value.SetPassive();
            SetFirstButtonActive();
        }

        public void SetFirstButtonActive() {
            if (ActiveButton != null) {
                ActiveButton.Value.SetPassive();
            }
            ActiveButton = Buttons.First;
            ActiveButton.Value.SetActive();
        }

        public void ActiveButtonAction() {
            ActiveButton.Value.Action();
        }
    }
}