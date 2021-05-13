using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using Breakout.Blocks;

namespace Breakout {
    public class Score : IGameEventProcessor {

        public uint ScoreCount { get; private set; }

        private Text display;

        public Score(Vec2F pos, Vec2F extent) {
            ScoreCount = 0;

            display = new Text("Score: " + ScoreCount.ToString(), pos, extent);
            display.SetColor(System.Drawing.Color.Gold);
        }

        // Unit only positive integers 
        private void AddToScore(BlockTypes blocktype) {
            switch (blocktype) {
                case BlockTypes.Normal:
                    ScoreCount += 1;
                    break;
                case BlockTypes.Hardened:
                    ScoreCount += 2;
                    break;
                case BlockTypes.Unbreakable:
                    break;
            }
            display.SetText("Score: " + ScoreCount.ToString());
        }

        public void RenderScore() {
            display.RenderText();
        }

        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.ControlEvent) {
                if (gameEvent.StringArg1 == "ADD_SCORE") {
                    //uint points = uint.Parse(gameEvent.Message);
                    //AddToScore(points);
                    if (gameEvent.From is HardenedBlock) {
                        AddToScore(BlockTypes.Hardened);
                    } else if (gameEvent.From is Block) {
                        AddToScore(BlockTypes.Normal);
                    }
                }
            }
        }
    }
}