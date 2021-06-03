using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.GamePlay.PowerUpOrbEntity {
    /// <summary>
    /// Subtyped PowerUpOrb that award one extra life to the Healthbar of the Player.
    /// </summary>
    public class ExtraLifeOrb : PowerUpOrb {
        public ExtraLifeOrb(DynamicShape shape, IBaseImage image) : base(shape, image) {}

        /// <summary>
        /// Broadcast that the ExtraLife effect is activated.
        /// </summary>
        public override void ApplyEffect() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "HealthGained"});
        }
    }
}