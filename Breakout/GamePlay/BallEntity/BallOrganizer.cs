using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

using System;
using System.IO;

namespace Breakout.GamePlay.BallEntity {
    public class BallOrganizer {
        public EntityContainer<Ball> Balls { get; private set; }

        public BallOrganizer() {
            Balls = new EntityContainer<Ball>();
        }


        //made public (not IGameEventProcessor) to fix eventbus causing crashes
        public void AddBall() {
            Ball ball = new Ball(
                new DynamicShape (new Vec2F(0.45f, 0.5f), new Vec2F(0.025f,0.025f)),
                new Image (Path.Combine(ProjectPath.getPath(),  
                "Breakout", "Assets", "Images", "ball.png")),
                (float) Math.PI /4f);
            Balls.AddEntity(ball);
        }

        public void MoveBalls() {
            Balls.Iterate(ball => {
                ball.Move();
            });
        }

        public void RenderBalls() {
            Balls.Iterate(ball => {
                ball.RenderEntity();
            });

        }

        /*
        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.ControlEvent) {
                switch(gameEvent.StringArg1) {
                    case "BallGained":
                        AddBall();
                        break;
                }
            }
        }*/
    }
}