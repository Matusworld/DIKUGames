using DIKUArcade.Events;
using DIKUArcade.Math;

namespace Breakout.States.Buttons {
    /// <summary>
    /// NewGame Button enables the game to start over.
    /// </summary>
    public class NewGameButton : Button {
        public NewGameButton(Vec2F pos, Vec2F extent, Vec3I actCol, Vec3I pasCol) 
            : base(pos, extent, actCol, pasCol) {
                text.SetText("New Game");
        }

        /// <summary>
        /// Broadcast that a new game should be started.
        /// </summary>
        public override void Action() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.GameStateEvent, 
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_NEWGAME" });
        }
    }
}