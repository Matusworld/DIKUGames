using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace Breakout.GamePlay.PowerUpOrbEntity {
    /// <summary>
    /// Vertically moving Orb spawned at the destruction of a PowerUp Block.
    /// Interacts with the Player and applies effect at collision after which it is deleted.
    /// Must be subtyped to give functionality.
    /// </summary>
    public abstract class PowerUpOrb : Entity {
        const float speed = 0.005f;

        public PowerUpOrb(DynamicShape shape, IBaseImage image) 
            : base(shape, image) {
                Shape.AsDynamicShape().Direction = new Vec2F(0f, -speed);
        }

        /// <summary>
        /// Detect whether this PowerUpOrb has exceeded the bottom window border.
        /// </summary>
        /// <returns>The boolean result.</returns>
        public bool LowerBoundaryCheck() {
            if (Shape.Position.Y + Shape.Extent.Y <= 0.0f) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Move this PowerUpOrbs by its Direction vector.
        /// Delete instead if it would exceed the lower boundary.
        /// </summary>
        public void Move() {
            if (LowerBoundaryCheck()) {
                DeleteEntity();
            } else {
                Shape.Move();
            }
        }

        /// <summary>
        /// Apply the effect of this PowerUpOrb depending on its subtype.
        /// </summary>
        public abstract void ApplyEffect();

        /// <summary>
        /// React to Player Collision by applying effect of this PowerUpOrb and 
        /// its subsequent deletion.
        /// </summary>
        public void PlayerCollision() {
            ApplyEffect();
            DeleteEntity();
        }
    }
}