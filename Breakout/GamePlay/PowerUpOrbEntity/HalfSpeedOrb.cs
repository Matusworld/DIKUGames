using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Timers;

namespace Breakout.GamePlay.PowerUpOrbEntity {
    /// <summary>
    /// Subtyped PowerUpOrb that causes all Balls to move at half speed for a given duration 
    /// when consumed.
    /// HalfSpeed effect cannot overlap with itself and does not refresh duration if 
    /// another is in effect.
    /// HalfSpeed and DoubleSpeed cancel eachother out in the time interval where 
    /// both are in effect. 
    /// </summary>
    public class HalfSpeedOrb : PowerUpOrb {
        private int powerUpDuration;

        public HalfSpeedOrb(DynamicShape shape, IBaseImage image, int powerUpDuration) 
            : base(shape, image) {
                this.powerUpDuration = powerUpDuration;
            }

        /// <summary>
        /// Broadcast that the HalfSpeed effect is Activated.
        /// Delayed broadcast that the HalfSpeed effect is Deactivated.
        /// </summary>
        public override void ApplyEffect() {
            BreakoutBus.GetBus().RegisterEvent ( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "HALF_SPEED",
                Message = "ACTIVATE" });

            BreakoutBus.GetBus().RegisterTimedEvent(
                new GameEvent{ EventType = GameEventType.ControlEvent,
                    StringArg1 = "HALF_SPEED", Message = "DEACTIVATE"},
                TimePeriod.NewMilliseconds(powerUpDuration));
        }
    }
}