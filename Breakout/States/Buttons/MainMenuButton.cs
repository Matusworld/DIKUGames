using DIKUArcade.Events;
using DIKUArcade.Math;

namespace Breakout.States.Buttons {
    /// <summary>
    /// MainMenu Button enables the transition to the Main Menu.
    /// </summary>
    public class MainMenuButton : Button {
        public MainMenuButton(Vec2F pos, Vec2F extent, Vec3I actCol, Vec3I pasCol) 
            : base(pos, extent, actCol, pasCol) {
                text.SetText("Main Menu");
        }

        /// <summary>
        /// Broadcast that the Main Menu should be entered.
        /// </summary>
        public override void Action() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.GameStateEvent,
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_MAINMENU" });
        }
    }
}