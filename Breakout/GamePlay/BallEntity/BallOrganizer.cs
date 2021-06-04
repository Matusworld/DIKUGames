using System;
using System.IO;

using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;

namespace Breakout.GamePlay.BallEntity {
    /// <summary>
    /// Organizing class for containing and giving mass functionality to Balls.
    /// Processes Events on behalf of contained Balls.
    /// </summary>
    public class BallOrganizer : EntityOrganizer<Ball> {
        private Random rand = new Random();
        public Vec2F BallsSpawnPosition { get; private set; }
        public Vec2F BallsSpawnExtent { get; private set; }
        public bool HalfSpeedActive { get; private set; }
        public bool DoubleSpeedActive { get; private set; }

        public BallOrganizer() : base() {
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);

            BallsSpawnExtent = new Vec2F(0.025f,0.025f);
            //Spawn in center
            BallsSpawnPosition = new Vec2F(0.5f, 0.5f) - (BallsSpawnExtent / (float) 2.0);
        }

        public override void MoveEntities() {
            Entities.Iterate(ball => {
                ball.Move();
            });
        }
        
        /// <summary>
        /// Detect whether or not any Balls remain in this BallOrganizer.
        /// </summary>
        /// <returns>The checked boolean result.</returns>
        public bool CheckNoBalls() {
            return Entities.CountEntities() == 0;
        }

        /// <summary>
        /// Generate a random Ball in the middle of the Window with Direction vector 
        /// from 45 to 135 degree.
        /// PowerUp Active states are set according to this BallOrganizer.
        /// </summary>
        public Ball GenerateBallRandomDir() {
            //random factor
            int factor = rand.Next(100,301);
            float theta = 0.25f * (float) Math.PI * factor / 100f;

            Ball ball = new Ball(
                new DynamicShape (BallsSpawnPosition, BallsSpawnExtent),
                new Image (Path.Combine(ProjectPath.getPath(),  
                "Breakout", "Assets", "Images", "ball.png")),
                theta, HalfSpeedActive, DoubleSpeedActive);

            return ball;
        }

        /// <summary>
        /// Reset to initial state with one single disempowered Ball at spawn position.
        /// </summary>
        public override void ResetOrganizer() {
            Entities.ClearContainer();

            HalfSpeedActive = false;
            DoubleSpeedActive = false;

            Ball ball = GenerateBallRandomDir();
            Entities.AddEntity(ball);
        }

        /// <summary>
        /// Process Events related to the Balls.
        /// </summary>
        /// <param name="gameEvent">gameEvent is a string that describes what event happened
        ///  and that is being sent.</param>
        public override void ProcessEvent(GameEvent gameEvent) {
            switch (gameEvent.StringArg1) {
                case "HALF_SPEED":
                    switch (gameEvent.Message) {
                        case "ACTIVATE":
                            HalfSpeedActive = true;
                            Entities.Iterate(ball => {
                                ball.HalfSpeedActivate();
                            });
                            break;
                        case "DEACTIVATE":
                            HalfSpeedActive = false;
                            Entities.Iterate(ball => {
                                ball.HalfSpeedDeactivate();
                            });
                            break;
                    }
                    break;
                
                case "DOUBLE_SPEED":
                    switch (gameEvent.Message) {
                        case "ACTIVATE":
                            DoubleSpeedActive = true;
                            Entities.Iterate(ball => {
                                ball.DoubleSpeedActivate();
                            });
                            break;
                        case "DEACTIVATE":
                            DoubleSpeedActive = false;
                            Entities.Iterate(ball => {
                                ball.DoubleSpeedDeactivate();
                            });
                            break;
                    }
                    break;
                
                case "ADD_BALL":
                    Entities.AddEntity(GenerateBallRandomDir());
                    break;
                
                case "BALL_DELETED":
                    if (CheckNoBalls()) {
                        ResetOrganizer();
                        BreakoutBus.GetBus().RegisterTimedEvent(
                            new GameEvent {
                                EventType = GameEventType.ControlEvent, 
                                StringArg1 = "HEALTH_LOST"},
                            TimePeriod.NewMilliseconds(BreakoutBus.CountDelay));
                    }
                    break;
                
                case "LEVEL_ENDED":
                case "LEVEL_BACK":
                    ResetOrganizer();
                    break;
            }
        }
    }
}