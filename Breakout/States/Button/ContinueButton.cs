using DIKUArcade.Events;
using DIKUArcade.Math;

namespace Breakout.States.Buttons {
    public class ContinueButton : Button {

        public ContinueButton(string text, Vec2F pos, Vec2F extent, Vec3I actCol, 
            Vec3I pasCol) : base(text, pos, extent, actCol, pasCol) {}

        public override void Action() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.GameStateEvent,
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_CONTINUE" });
        }
    }
}