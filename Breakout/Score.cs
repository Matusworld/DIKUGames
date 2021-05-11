using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;

namespace Breakout {
    public class Score : IGameEventProcessor {

        public uint ScoreCount { get; private set; }

        private Text display;

        public Score(Vec2F pos, Vec2F extent) {
            ScoreCount = 0;

            display = new Text(ScoreCount.ToString(), pos, extent);
            display.SetColor(new Vec3I(255,0,0));
        }

        // Unit only positive integers 
        private void AddToScore(uint n) {
            ScoreCount += n;
        }

        public void RenderScore() {
            display.RenderText();
        }

        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.ControlEvent) {
                if (gameEvent.StringArg1 == "ADD_SCORE") {
                    uint points = uint.Parse(gameEvent.Message);
                    AddToScore(points);
                }
            }
        }
    }
}