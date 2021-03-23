using DIKUArcade.Math;
using DIKUArcade.Graphics;

namespace Galaga {
    public class Score {
        public int ScoreCount { get; private set; }
        private Text display;
        public Score (Vec2F position, Vec2F extent) {
            ScoreCount = 0;
            display = new Text(ScoreCount.ToString(), position, extent);
            display.SetColor(new Vec3I(255,0,0));
        }

        public void AddPoint() {
            ScoreCount++;
            display.SetText(ScoreCount.ToString());
        }

        public void RenderScore() {
            display.RenderText();
        }
    }
}