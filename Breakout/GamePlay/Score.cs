using System;

using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;

using Breakout.GamePlay.BlockEntity;

namespace Breakout.GamePlay {
    public class Score : IGameEventProcessor {
        Random rand;
        private const int normalBlockScore = 1;
        private const int hardenedBlockScore = 2;
        private const int unbreakableBlockScore = 5;
        private const int powerupBlockScore = 1;
        public const int MinPowerUpPoints = 1;
        public const int MaxPowerUpPoints = 30;
        public uint ScoreCount { get; private set; }

        private Text display;

        public Score(Vec2F pos, Vec2F extent) {
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);
            
            rand = new Random();

            ScoreCount = 0;

            display = new Text("Score: " + ScoreCount.ToString(), pos, extent);
            display.SetColor(System.Drawing.Color.Gold);
        }

        public void Reset() {
            ScoreCount = 0;

            display.SetText("Score: " + ScoreCount.ToString());
        }

        private void AddPowerUpScore() {
            ScoreCount += (uint) rand.Next(MinPowerUpPoints, MaxPowerUpPoints+1);

            display.SetText("Score: " + ScoreCount.ToString());
        }

        // Unit only positive integers 
        private void AddToScore(BlockTypes blocktype) {
            switch (blocktype) {
                case BlockTypes.Normal:
                    ScoreCount += normalBlockScore;
                    break;
                case BlockTypes.Hardened:
                    ScoreCount += hardenedBlockScore;
                    break;
                case BlockTypes.Unbreakable:
                    ScoreCount += unbreakableBlockScore;
                    break;
                case BlockTypes.PowerUp:
                    ScoreCount += powerupBlockScore;
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