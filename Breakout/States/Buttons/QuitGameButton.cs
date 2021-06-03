using DIKUArcade.Events;
using DIKUArcade.Math;

namespace Breakout.States.Buttons {
    /// <summary>
    /// QuitGame Button enables the game to Quit.
    /// </summary>
    public class QuitGameButton : Button {
        public QuitGameButton(Vec2F pos, Vec2F extent, Vec3I actCol, Vec3I pasCol) 
            : base(pos, extent, actCol, pasCol) {
                text.SetText("Quit Game");
        }

        /// <summary>
        /// Broadcast that the game should be quit.
        /// </summary>
        public override void Action() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.GameStateEvent, 
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_QUIT" });
        }
    }
}