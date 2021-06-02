using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.GamePlay.PowerUpOrbEntity {
    public class ExtraPointsOrb : PowerUpOrb {
        public ExtraPointsOrb(DynamicShape shape, IBaseImage image) : base(shape, image) {}

        public override void ApplyEffect() {
            BreakoutBus.GetBus().RegisterEvent ( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "POWERUP_SCORE"});
        }
    }
}