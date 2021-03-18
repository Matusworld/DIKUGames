using DIKUArcade.Math;
using DIKUArcade.Graphics;
using System;

namespace Galaga {
    public class Score {
        private int score;
        private Text display;
        public Score (Vec2F position, Vec2F extent) {
            score = 0;
            display = new Text("abc", position, extent);
        }

        public void AddPoint() {
            score++;
        }

        public void RenderScore() {
            //display.RenderText();
            //Console.WriteLine(score);
        }
    }
}