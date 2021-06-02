using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout.States.Buttons {
    public abstract class Button {
        protected Text text;
        protected Vec3I activeColor;
        protected Vec3I passiveColor;
        public bool Active { get; protected set; }

        public Button (Vec2F pos, Vec2F extent, Vec3I actCol, Vec3I pasCol) {
                activeColor = actCol;
                passiveColor = pasCol;
            }

        public void SetActive() {
            text.SetColor(activeColor);
            this.Active = true;

        }
        public void SetPassive() {
            text.SetColor(passiveColor);
            this.Active = false;
        }

        public void Render() {
            text.RenderText();
        }

        /// <summary>
        /// Implement button action, typically with events
        /// </summary>
        public abstract void Action();
    }
}