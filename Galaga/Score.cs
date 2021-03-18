using DIKUArcade.Math;
using DIKUArcade.Graphics;
using System;

namespace Galaga {
    public class Score {
        private int score;
        private Text display;
        public Score (Vec2F position, Vec2F extent) {
            score = 0;
            display = new Text(score.ToString(), position, extent);
            display.SetColor(new Vec3I(255,0,0));
        }

        public void AddPoint() {
            score++;
            display.SetText(score.ToString());
        }

        public void RenderScore() {
            display.RenderText();
        }
    }
}