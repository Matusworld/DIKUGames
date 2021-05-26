using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

using System;
using System.IO;
using System.Collections.Generic;

namespace Breakout.GamePlay.BallEntity {
    public class BallOrganizer : EntityOrganizer<Ball> {
        private Random rand = new Random();
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
        /// </summary>
        public Ball GenerateBallRandomDir() {
            int factor = rand.Next(100,301);
            float theta = 0.25f * (float) Math.PI * factor/100f;
            Ball ball = new Ball(
                new DynamicShape (new Vec2F(0.5f, 0.5f), new Vec2F(0.025f,0.025f)),
                new Image (Path.Combine(ProjectPath.getPath(),  
                "Breakout", "Assets", "Images", "ball.png")),
                theta);
            return ball;
        }

        public override void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.To == this) {
                ProcessEventValidator(gameEvent);

                Ball ball = (Ball) gameEvent.ObjectArg1;
                ball.ReceiveEvent(gameEvent);
            }
        }
    }
}