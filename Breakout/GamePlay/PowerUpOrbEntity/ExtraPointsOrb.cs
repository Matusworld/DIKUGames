using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.GamePlay.PowerUpOrbEntity {
    /// <summary>
    /// Subtyped PowerUpOrb that awards a random amount of points (defined by the Score class) 
    /// when consumed.
    /// </summary>
    public class ExtraPointsOrb : PowerUpOrb {
        public ExtraPointsOrb(DynamicShape shape, IBaseImage image) : base(shape, image) {}

        /// <summary>
        /// Broadcast that the ExtraPoints effect is activated.
        /// </summary>
        public override void ApplyEffect() {
            BreakoutBus.GetBus().RegisterEvent ( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "POWERUP_SCORE"});
        }
    }
}