using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

using System;
using System.IO;

namespace Breakout.GamePlay.BallEntity {
    public class BallOrganizer : EntityOrganizer<Ball> {
        private Random rand = new Random();
        public bool HalfSpeedActive { get; private set; }
        public bool DoubleSpeedActive { get; private set; }
        public BallOrganizer() : base() {
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);
        }

        public override void MoveEntities() {
            Entities.Iterate(ball => {
                ball.Move();
            });
        }
        
        /// <summary>
        /// Generate a random ball in mid of map with dir from 45 to 135 degree.
        /// PowerUp states correspond to existing balls
        /// </summary>
        public Ball GenerateBallRandomDir() {
            //random factor
            int factor = rand.Next(100,301);
            float theta = 0.25f * (float) Math.PI * factor / 100f;

            Ball ball = new Ball(
                new DynamicShape (new Vec2F(0.5f, 0.5f), new Vec2F(0.025f,0.025f)),
                new Image (Path.Combine(ProjectPath.getPath(),  
                "Breakout", "Assets", "Images", "ball.png")),
                theta, HalfSpeedActive, DoubleSpeedActive);
            return ball;
        }

        /// <summary>
        /// Reset to initial state with one single disempowered ball at spawn position
        /// </summary>
        public void ResetBalls() {
            Entities.ClearContainer();

            HalfSpeedActive = false;
            DoubleSpeedActive = false;

            Ball ball = GenerateBallRandomDir();
            Entities.AddEntity(ball);
        }

        public override void ProcessEvent(GameEvent gameEvent) {
            switch(gameEvent.StringArg1) {
                case "HalfSpeed":
                    switch(gameEvent.Message) {
                        case "Activate":
                            HalfSpeedActive = true;
                            Entities.Iterate(ball => {
                                ball.ReceiveEvent(gameEvent);
                            });
                            break;
                        case "Deactivate":
                            HalfSpeedActive = false;
                            Entities.Iterate(ball => {
                                ball.ReceiveEvent(gameEvent);
                            });
                            break;
                    }
                    break;
                case "DoubleSpeed":
                    switch(gameEvent.Message) {
                        case "Activate":
                            DoubleSpeedActive = true;
                            Entities.Iterate(ball => {
                                ball.ReceiveEvent(gameEvent);
                            });
                            break;
                        case "Deactivate":
                            DoubleSpeedActive = false;
                            Entities.Iterate(ball => {
                                ball.ReceiveEvent(gameEvent);
                            });
                            break;
                    }
                    break;
                case "AddBall":
                    Ball ball = GenerateBallRandomDir();
                    Entities.AddEntity(ball);
                    break;
            }

        }
    }
}