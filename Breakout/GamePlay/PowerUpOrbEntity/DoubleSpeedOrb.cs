using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Timers;

namespace Breakout.GamePlay.PowerUpOrbEntity {
    public class DoubleSpeedOrb : PowerUpOrb {
        private int powerUpDuration;
        public DoubleSpeedOrb(DynamicShape shape, IBaseImage image, int powerUpDuration) 
            : base(shape, image) {
                this.powerUpDuration = powerUpDuration;
            }

        public override void ApplyEffect() {
            BreakoutBus.GetBus().RegisterEvent ( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "DoubleSpeed",
                Message = "Activate"                       
                });

            BreakoutBus.GetBus().RegisterTimedEvent(
                new GameEvent{ EventType = GameEventType.ControlEvent,
                    StringArg1 = "DoubleSpeed", Message = "Deactivate"},
                TimePeriod.NewMilliseconds(powerUpDuration));
        }
    }
}