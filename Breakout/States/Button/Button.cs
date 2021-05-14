using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout.States.Buttons {
    public abstract class Button : Text {
        protected Vec3I activeColor;
        protected Vec3I passiveColor;
        public bool Active { get; protected set; }

        public Button (string text, Vec2F pos, Vec2F extent, Vec3I actCol, 
            Vec3I pasCol) : base(text, pos, extent) {
                activeColor = actCol;
                passiveColor = pasCol;
            }

        public void SetActive() {
            this.SetColor(activeColor);
            this.Active = true;

        }
        public void SetPassive() {
            this.SetColor(passiveColor);
            this.Active = false;
        }

        public void Render() {
            this.RenderText();
        }

        /// <summary>
        /// Implement button action, typically with events
        /// </summary>
        public abstract void Action();
    }
}