using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace Breakout.GamePlay.PowerUpOrbEntity {
    public class PowerUpOrb : Entity {
        const float speed = 0.005f;
        public PowerUpTypes Type { get; private set;}

        public PowerUpOrb(
            DynamicShape shape, IBaseImage image, PowerUpTypes type) : base(shape, image) {
                Type = type;
                this.Shape.AsDynamicShape().Direction = new Vec2F(0f, -speed);
        }

        public bool LowerBoundaryCheck() {
            if (this.Shape.Position.Y+this.Shape.Extent.Y <= 0.0f) {
                return true;
            } else {
                return false;
            }
        }

        public void Move() {
            if (LowerBoundaryCheck()) {
                this.DeleteEntity();
            } else {
                this.Shape.Move();
            }
        }
    }
}