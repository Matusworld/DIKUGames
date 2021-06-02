using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.GamePlay.PowerUpOrbEntity {
    public class ExtraBallOrb : PowerUpOrb {
        public ExtraBallOrb(DynamicShape shape, IBaseImage image) : base(shape, image) {}

        public override void ApplyEffect() {
            BreakoutBus.GetBus().RegisterEvent ( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "ADD_BALL"});
        }
    }
}