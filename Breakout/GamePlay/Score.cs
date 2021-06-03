using System;

using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;

using Breakout.GamePlay.BlockEntity;

namespace Breakout.GamePlay {
    /// <summary>
    /// Score keeps track of the points awarded during the game.
    /// Points are awarded via destroyed Blocks and PowerUp effects.
    /// </summary>
    public class Score : IGameEventProcessor {
        //ExtraPointOrbs give a random amount of points
        Random rand;
        public const int MinPowerUpPoints = 1;
        public const int MaxPowerUpPoints = 30;

        private const int normalBlockScore = 1;
        private const int hardenedBlockScore = 2;
        private const int unbreakableBlockScore = 5;
        private const int powerupBlockScore = 1;

        //The score can only be positive due to its type
        public uint ScoreCount { get; private set; }

        private Text display;

        public Score(Vec2F pos, Vec2F extent) {
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);
            
            rand = new Random();

            ScoreCount = 0;

            display = new Text("Score: " + ScoreCount.ToString(), pos, extent);
            display.SetColor(System.Drawing.Color.Gold);
        }

        /// <summary>
        /// Reset this Score to itself initial state, i.e. 0 points.
        /// </summary>
        public void Reset() {
            ScoreCount = 0;

            display.SetText("Score: " + ScoreCount.ToString());
        }

        /// <summary>
        /// Add a random amount of point to this Score depending on the boundaries of this Score.
        /// </summary>
        private void AddPowerUpScore() {
            ScoreCount += (uint) rand.Next(MinPowerUpPoints, MaxPowerUpPoints+1);

            display.SetText("Score: " + ScoreCount.ToString());
        }

        /// <summary>
        /// Add points to this Score depending on the destroyed Block type.
        /// </summary>
        /// <param name="blocktype">The Block type of the destroyed block.</param>
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

        /// <summary>
        /// Process Events related to points.
        /// </summary>
        /// <param name="gameEvent"></param>
        public void ProcessEvent(GameEvent gameEvent) {
            switch (gameEvent.StringArg1) {
                case "BLOCK_DELETED":
                    if (gameEvent.From is Hardened) {
                        AddToScore(BlockTypes.Hardened);
                    } else if (gameEvent.From is PowerUp) {
                        AddToScore(BlockTypes.PowerUp);
                    } else if (gameEvent.From is Block) {
                        AddToScore(BlockTypes.Normal);
                    }
                    break;
                case "POWERUP_SCORE":
                    AddPowerUpScore();
                    break;
            }
        }
    }
}