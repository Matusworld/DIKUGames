using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using DIKUArcade.Events;

namespace Breakout.GamePlay {
    /// <summary>
    /// Integer division
    /// </summary>
    public class BreakoutTimer : StaticTimer {
        private Text display;
        //internal level time counter, from levelTime down to 0
        private int timer;
        //Max duration of level
        private int levelTime;
        //The static/global time at point of level start
        private int staticStartTime;

        public BreakoutTimer(int levelTime, Vec2F pos, Vec2F extent) {
            timer = levelTime;
            this.levelTime = levelTime;
            staticStartTime = (int) GetElapsedMilliseconds() / 1000;

            display = new Text("Time left: " + timer.ToString(), pos, extent);
            display.SetColor(System.Drawing.Color.Gold);
        }

        public void SetNewLevelTime(int levelTime) {
            timer = levelTime;
            this.levelTime = levelTime;
            staticStartTime = (int) GetElapsedMilliseconds() / 1000;
        }
        

        public void UpdateTimer() {
            timer = (levelTime + staticStartTime) - (int) GetElapsedMilliseconds() / 1000;
            display.SetText("Time left: " + timer.ToString());

            if (TimeRunOut()) {
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.GameStateEvent, Message = "CHANGE_STATE",
                    StringArg1 = "GAME_LOST"
                });
            }
        }

        public bool TimeRunOut() {
            if (timer <= 0) {
                return true;
            } else {
                return false;
            }
        }
        
        public void Render() {
            display.RenderText();
        }
    }
}