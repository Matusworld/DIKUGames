using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Timers;

namespace Breakout.GamePlay.PowerUpOrbEntity {
    /// <summary>
    /// Subtyped PowerUpOrb that causes all Balls to move at double speed for a given duration
    /// when consumed.
    /// DoubleSpeed effect cannot overlap with itself and does not refresh duration if 
    /// another is in effect.
    /// HalfSpeed and DoubleSpeed cancel eachother out in the time interval where 
    /// both are in effect. 
    /// </summary>
    public class DoubleSpeedOrb : PowerUpOrb {
        private int powerUpDuration;
        public DoubleSpeedOrb(DynamicShape shape, IBaseImage image, int powerUpDuration) 
            : base(shape, image) {
                this.powerUpDuration = powerUpDuration;
            }

        /// <summary>
        /// Broadcast that the DoubleSpeed effect is Activated.
        /// Delayed broadcast that the DoubleSpeed effect is Deactivated.
        /// </summary>
        public override void ApplyEffect() {
            BreakoutBus.GetBus().RegisterEvent ( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "DOUBLE_SPEED",
                Message = "ACTIVATE"
                });

            BreakoutBus.GetBus().RegisterTimedEvent(
                new GameEvent{ EventType = GameEventType.ControlEvent,
                    StringArg1 = "DOUBLE_SPEED", Message = "DEACTIVE"},
                TimePeriod.NewMilliseconds(powerUpDuration));
        }
    }
}