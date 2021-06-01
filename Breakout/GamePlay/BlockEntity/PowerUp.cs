using System.IO;

using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;

using Breakout.GamePlay.PowerUpOrbEntity;

namespace Breakout.GamePlay.BlockEntity {
    public class PowerUp : Block {

        public PowerUp (DynamicShape shape, IBaseImage image, IBaseImage damageImage) 
            : base(shape, image, damageImage) {}

        /// <summary>
        /// Randomly choose power up and and spawn it as an entity
        /// </summary>
        private void SpawnOrbShape() {
            Vec2F extent = new Vec2F(0.05f, 0.05f);
            DynamicShape shape = new DynamicShape(this.Shape.Position, extent);

            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "ADD_ORB",
                ObjectArg1 = shape});
        }

        protected override void BlockHit() {
            Damage();
            if (HalfHpCheck()) {
                this.Image = damageImage;
            }
            if (!IsAlive()) {
                Delete();
                SpawnOrbShape();
            }
        }
    }
}