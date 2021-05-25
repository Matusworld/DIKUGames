using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using Breakout.BlockEntity;

using System;
namespace Breakout.GamePlay {
    public class Score : IGameEventProcessor {
        Random rand;
        private const int minPowerUpPoints = 1;
        private const int maxPowerUpPoints = 30;
        public uint ScoreCount { get; private set; }

        private Text display;

        public Score(Vec2F pos, Vec2F extent) {
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);
            
            rand = new Random();

            ScoreCount = 0;

            display = new Text("Score: " + ScoreCount.ToString(), pos, extent);
            display.SetColor(System.Drawing.Color.Gold);
        }

        private void AddPowerUpScore() {
            ScoreCount += (uint) rand.Next(minPowerUpPoints, maxPowerUpPoints+1);

            display.SetText("Score: " + ScoreCount.ToString());
        }

        // Unit only positive integers 
        private void AddToScore(BlockTypes blocktype) {
            switch (blocktype) {
                case BlockTypes.Normal:
                    ScoreCount++;
                    break;
                case BlockTypes.Hardened:
                    ScoreCount += 2;
                    break;
                case BlockTypes.Unbreakable:
                    break;
                case BlockTypes.PowerUp:
                    ScoreCount++;
                    break;
            }
            display.SetText("Score: " + ScoreCount.ToString());
        }

        public void Render() {
            display.RenderText();
        }

        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.ControlEvent) {
                switch(gameEvent.StringArg1) {
                    case "ADD_SCORE":
                        if (gameEvent.From is Hardened) {
                            AddToScore(BlockTypes.Hardened);
                        } else if (gameEvent.From is PowerUp) {
                            AddToScore(BlockTypes.PowerUp);
                        } 
                        else if (gameEvent.From is Block) {
                            AddToScore(BlockTypes.Normal);
                        }
                        break;
                    case "PowerUpScore":
                        AddPowerUpScore();
                        break;
                }
            }
        }
    }
}