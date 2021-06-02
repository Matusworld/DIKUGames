using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace Breakout.GamePlay.PowerUpOrbEntity {
    public abstract class PowerUpOrb : Entity {
        const float speed = 0.005f;

        public PowerUpOrb(DynamicShape shape, IBaseImage image) 
            : base(shape, image) {
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

        public abstract void ApplyEffect();

        public void PlayerCollision() {
            ApplyEffect();
            DeleteEntity();
        }
    }
}