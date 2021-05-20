using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;

namespace Breakout.States {
    public class Timer : StaticTimer {
        private Text display;

        private int timer;

        private int startTime;

        public Timer(int starttime, Vec2F pos, Vec2F extent) {
            this.startTime = starttime;
            timer = starttime;

            display = new Text("Time left: " + timer.ToString(), pos, extent);
            display.SetColor(System.Drawing.Color.Gold);
        }

        

        public void UpdateTimer() {
            timer = startTime - (int) GetElapsedSeconds();
            display.SetText("Time left: " + timer.ToString());
        }

        public bool TimeRunOut() {
            if (timer <= 0) {
                return true;
            } else {
                return false;
            }
        }

        public void NewTimer(int starttime) {
            this.startTime = starttime;
            timer = starttime;
        }

        public void Render() {
            display.RenderText();
        }
    }
}