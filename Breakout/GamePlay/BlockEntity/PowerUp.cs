using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;

namespace Breakout.GamePlay.BlockEntity {
    /// <summary>
    /// PowerUp Block spawns an PowerUpOrb when deleted.
    /// </summary>
    public class PowerUp : Block {
        public PowerUp (Shape shape, IBaseImage image, IBaseImage damageImage) 
            : base(shape, image, damageImage) {}

        /// <summary>
        /// Send Event that an Orb should be spawned and provide this PowerUp Block's position.
        /// </summary>
        private void SendSpawnOrbEvent() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "SPAWN_ORB",
                ObjectArg1 = Shape.Position});
        }

        public override void BlockHit() {
            Damage();
            if (HalfHpCheck()) {
                this.Image = damageImage;
            }
            if (!IsAlive()) {
                Delete();
                SendSpawnOrbEvent();
            }
        }
    }
}