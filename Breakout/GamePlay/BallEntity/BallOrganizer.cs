using System;
using System.IO;


using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;

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
        
        private bool CheckNoBalls() {
            return Entities.CountEntities() == 0;
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
        public override void ResetOrganizer() {
            Entities.ClearContainer();

            HalfSpeedActive = false;
            DoubleSpeedActive = false;

            Ball ball = GenerateBallRandomDir();
            Entities.AddEntity(ball);
        }

        public override void ProcessEvent(GameEvent gameEvent) {
            switch(gameEvent.StringArg1) {
                case "HALF_SPEED":
                    switch(gameEvent.Message) {
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
                    switch(gameEvent.Message) {
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
                    //Add small delay so that EntityContainer will have cleaned up
                    //the block marked for deletion by the time of block counting
                    BreakoutBus.GetBus().RegisterTimedEvent(
                        new GameEvent { EventType = GameEventType.ControlEvent,
                            StringArg1 = "BALL_DELETED_DELAY"},
                        TimePeriod.NewMilliseconds(5));
                    break;
                
                case "BALL_DELETED_DELAY":
                    if (CheckNoBalls()) {
                        ResetOrganizer();
                        BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                            EventType = GameEventType.ControlEvent, 
                            StringArg1 = "HEALTH_LOST"});
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