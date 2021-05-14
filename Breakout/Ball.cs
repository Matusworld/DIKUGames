using System.Collections.Generic;
using System.IO;
using System;
using Breakout;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using DIKUArcade.Graphics;

namespace Breakout {
    public class Ball : Entity, IGameEventProcessor {   

        const float speed = 0.02f;
        float theta;
        public bool Active { get; private set; } = true;

        public Ball(DynamicShape shape, IBaseImage image, float theta): base (shape, image) {
            SetDirection(theta);
        }
        public Vec2F GetPosition() {
            return this.Shape.Position; 
        }
        
        private void SetDirection(float theta) {
            this.theta = theta;
            this.Shape.AsDynamicShape().Direction.X = (float)Math.Cos((double)theta)*speed;
            this.Shape.AsDynamicShape().Direction.Y = (float)Math.Sin((double)theta)*speed;
        }

        public bool LeftBoundaryCheck() {
            if (this.Shape.Position.X <= 0.0f) {
                return true;
            } else {
                return false;
            }
        }
        public bool RightBoundaryCheck() {
            if (this.Shape.Position.X+this.Shape.Extent.X >= 1.0f) {
                return true;
            } else {
                return false;
            }
        }
        public bool UpperBoundaryCheck() {
            if (this.Shape.Position.Y+this.Shape.Extent.Y >= 1.0f) {
                return true;
            } else {
                return false;
            }
        }

        public bool LowerBoundaryCheck() {
            if (this.Shape.Position.Y+this.Shape.Extent.Y <= 0.0f) {
                return true;
            } else {
                return false;
            }
        }
        public float ReturnTheta(float PlayerPosition){
            float theta = 0.75f * (float) Math.PI - (PlayerPosition*(float)Math.PI / 2f);
            
            return theta;
        }
        public void DirectionPlayerSetter(float PlayerPosition) {
            //rebound angle depending on hit position
            float theta = 0.75f * (float) Math.PI - (PlayerPosition*(float)Math.PI / 2f);
            SetDirection(theta);
        }

        public void DirectionBoundarySetter(){
            if (LeftBoundaryCheck() || RightBoundaryCheck()) {
                this.Shape.AsDynamicShape().Direction.X = -this.Shape.AsDynamicShape().Direction.X;
            }
            if (UpperBoundaryCheck()) {
                this.Shape.AsDynamicShape().Direction.Y = -this.Shape.AsDynamicShape().Direction.Y;
            }
            
        }
        private void Move() {
            if(LowerBoundaryCheck()) {
                this.DeleteEntity(); //not currently in entity container
            }
            else {
                DirectionBoundarySetter(); 
                this.Shape.Move();
            }
        }
        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.MovementEvent) {
                switch(gameEvent.StringArg1) {
                    case "PlayerCollision":
                        float PlayerPosition = float.Parse (gameEvent.Message);
                        DirectionPlayerSetter(PlayerPosition);
                        break;
                    case "BlockCollision":
                        if(this.Active) {
                            switch (gameEvent.Message) {
                                case "UpDown":
                                    this.Shape.AsDynamicShape().Direction.Y = 
                                        -this.Shape.AsDynamicShape().Direction.Y;
                                    break;
                                case "LeftRight":
                                    this.Shape.AsDynamicShape().Direction.X = 
                                        -this.Shape.AsDynamicShape().Direction.X;
                                    break;
                            }
                            //Don't switch direction for a short while
                            this.Active = false;
                            //After some time Activate again
                            BreakoutBus.GetBus().RegisterTimedEvent(
                                new GameEvent{ EventType = GameEventType.ControlEvent,
                                    StringArg1 = "BallActivate"},
                                TimePeriod.NewMilliseconds(50));
                        }
                        break;
                    case "Move":
                        this.Move();
                        break;
                }
            }
            else if (gameEvent.EventType == GameEventType.ControlEvent) {
                switch(gameEvent.StringArg1) {
                    case "BallActivate":
                        this.Active = true;
                        break;
                }
            }
        }

    }
}