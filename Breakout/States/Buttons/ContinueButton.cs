using DIKUArcade.Events;
using DIKUArcade.Math;

namespace Breakout.States.Buttons {
    /// <summary>
    /// Continue Button enables the game to Continue.
    /// </summary>
    public class ContinueButton : Button {
        public ContinueButton(Vec2F pos, Vec2F extent, Vec3I actCol, Vec3I pasCol) 
            : base(pos, extent, actCol, pasCol){
                text.SetText("Continue");
        }

        /// <summary>
        /// Broadcast that the game should continue.
        /// </summary>
        public override void Action() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.GameStateEvent,
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_CONTINUE" });
        }
    }
}