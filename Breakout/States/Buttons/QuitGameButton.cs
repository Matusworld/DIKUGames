using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout.States.Buttons {
    public class QuitGameButton : Button {

        public QuitGameButton(Vec2F pos, Vec2F extent, Vec3I actCol, Vec3I pasCol) 
            : base(pos, extent, actCol, pasCol) {
                text = new Text("Quit Game", pos, extent);
                SetPassive();
            }

        public override void Action() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.GameStateEvent, 
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_QUIT" });
        }
    }
}