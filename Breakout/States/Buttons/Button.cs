using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout.States.Buttons {
    /// <summary>
    /// Stationary Button in Window that can be either Active or Passive.
    /// Must be subtyped to add functionality to Button Action.
    /// Button is passive color by default.
    /// </summary>
    public abstract class Button {
        protected Text text;
        protected Vec3I activeColor;
        protected Vec3I passiveColor;

        public Button (Vec2F pos, Vec2F extent, Vec3I actCol, Vec3I pasCol) {
            activeColor = actCol;
            passiveColor = pasCol;

            text = new Text("", pos, extent);
            SetPassiveColor();
        }

        /// <summary>
        /// Change to color of this Button to its activeColor.
        /// </summary>
        public void SetActiveColor() {
            text.SetColor(activeColor);
        }

        /// <summary>
        /// Change to color of this Button to its passiveColor.
        /// </summary>
        public void SetPassiveColor() {
            text.SetColor(passiveColor);
        }

        public void Render() {
            text.RenderText();
        }

        /// <summary>
        /// Implement button action, typically with events.
        /// </summary>
        public abstract void Action();
    }
}