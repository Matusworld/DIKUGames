using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.GamePlay.PowerUpOrbEntity {
    /// <summary>
    /// Subtyped PowerUpOrb that awards one extra ball that is immediately launched with all
    /// existing PowerUp effects applied to it.
    /// </summary>
    public class ExtraBallOrb : PowerUpOrb {
        public ExtraBallOrb(DynamicShape shape, IBaseImage image) : base(shape, image) {}

        /// <summary>
        /// Broadcast that the ExtraBall effect is activated.
        /// </summary>
        public override void ApplyEffect() {
            BreakoutBus.GetBus().RegisterEvent ( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "ADD_BALL"});
        }
    }
}