using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System;

namespace Breakout {
    public class Ball : Entity, IGameEventProcessor {   

        //bool Trajectory; //This variable when false signals that the ball is moving left
            // And when it is true, it should be moving right.
        const float speed = 0.02f;
        float theta;

        public Ball(DynamicShape shape, IBaseImage image): base (shape, image) {
            this.theta = (float)Math.PI/4f;
            this.Shape.AsDynamicShape().Direction.X = (float)Math.Cos((double)theta)*speed;
            this.Shape.AsDynamicShape().Direction.Y = (float)Math.Sin((double)theta)*speed;
        }
        public Vec2F GetPosition() {
            return this.Shape.Position; 
        }
        
        /*public void BoundaryCheck(){
            if (this.Shape.Position.X <= 0.01){
                this.Shape.AsDynamicShape().Direction.X = 
            }
        //We add extent on to the right side check, since entities are rendered from left to right
            if (this.Shape.Position.X+this.Shape.Extent.X >= 0.99){
                Trajectory = false;
            }
            if (this.Shape.Position.Y+this.Shape.Extent.Y >= 0.99){
                if 
            }
        } */
        public bool LeftBoudaryCheck() {
            if (this.Shape.Position.X <= 0.01f) {
                return true;
            } else {
                return false;
            }
        }
        public bool RightBoudaryCheck() {
            if (this.Shape.Position.X+this.Shape.Extent.X >= 0.99f) {
                return true;
            } else {
                return false;
            }
        }
        public bool UpperBoundaryCheck() {
            if (this.Shape.Position.Y+this.Shape.Extent.Y >= 0.99) {
                return true;
            } else {
                return false;
            }
        }

        public bool LowerBoundaryCheck() {
            if (this.Shape.Position.Y+this.Shape.Extent.Y >= 0.01) {
                return true;
            } else {
                return false;
            }
        }
//Eventuelt opdel den her funktion
        public void DirectionPlayerSetter(float PlayerPosition) {
            theta = 0.75f * (float) Math.PI - (PlayerPosition*(float)Math.PI/2f);
            this.Shape.AsDynamicShape().Direction.X = (float)Math.Cos((double)theta)*speed;
            this.Shape.AsDynamicShape().Direction.Y = (float)Math.Sin((double)theta)*speed;
        }

        public void DirectionBoundarySetter(){
            if (LeftBoudaryCheck() || RightBoudaryCheck()) {
                this.Shape.AsDynamicShape().Direction.X = -this.Shape.AsDynamicShape().Direction.X;
            }
            if (UpperBoundaryCheck()) {
                this.Shape.AsDynamicShape().Direction.Y = -this.Shape.AsDynamicShape().Direction.Y;
            }
            
        }
        public void Move() {
            DirectionBoundarySetter(); 
            this.Shape.Move();
        }
        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.ControlEvent) {
                switch(gameEvent.StringArg1) {
                    case "PlayerCollision":
                        float PlayerPosition = float.Parse (gameEvent.Message);
                        DirectionPlayerSetter(PlayerPosition);
                        break;
                }
            }
        }

    }
}