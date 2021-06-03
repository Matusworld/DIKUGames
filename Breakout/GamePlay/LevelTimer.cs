using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using DIKUArcade.Events;

namespace Breakout.GamePlay {
    /// <summary>
    /// LevelTimer keeps track of the internal level time counting downwards to 0.
    /// All time is in seconds (converted from milliseconds and floored)
    /// </summary>
    public class LevelTimer : StaticTimer {
        private Text display;
        //internal level time counter, from levelTime down to 0
        public int timer { get; private set; }
        //Max duration of level
        public int levelTime { get; private set; }
        //The static/global time at point of level start
        public int staticStartTime { get; private set; }

        public LevelTimer(int levelTime, Vec2F pos, Vec2F extent) {
            timer = levelTime;
            this.levelTime = levelTime;
            staticStartTime = (int) GetElapsedMilliseconds() / 1000;

            display = new Text("Time left: " + timer.ToString(), pos, extent);
            display.SetColor(System.Drawing.Color.Gold);
        }

        /// <summary>
        /// Set this LevelTimer to given time.
        /// </summary>
        /// <param name="levelTime">The start time of the given level.</param>
        public void SetNewLevelTime(int levelTime) {
            timer = levelTime;
            this.levelTime = levelTime;
            staticStartTime = (int) GetElapsedMilliseconds() / 1000;
        }
        
        /// <summary>
        /// Update this LevelTimer. It counts down towards 0 seconds.
        /// At 0 seconds broadcast that the game has been lost.
        /// </summary>
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

        /// <summary>
        /// Check that time of this LevelTimer has reached 0.
        /// </summary>
        /// <returns>The boolean result.</returns>
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