using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System;

namespace Breakout {
    public class Ball : Entity, IGameEventProcessor {   

        const float speed = 0.02f;
        float theta;

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

        private bool LeftBoudaryCheck() {
            if (this.Shape.Position.X <= 0.01f) {
                return true;
            } else {
                return false;
            }
        }
        private bool RightBoudaryCheck() {
            if (this.Shape.Position.X+this.Shape.Extent.X >= 0.99f) {
                return true;
            } else {
                return false;
            }
        }
        private bool UpperBoundaryCheck() {
            if (this.Shape.Position.Y+this.Shape.Extent.Y >= 0.99) {
                return true;
            } else {
                return false;
            }
        }

        private bool LowerBoundaryCheck() {
            if (this.Shape.Position.Y+this.Shape.Extent.Y <= 0.01) {
                return true;
            } else {
                return false;
            }
        }

        private void DirectionPlayerSetter(float PlayerPosition) {
            //rebound angle depending on hit position
            float theta = 0.75f * (float) Math.PI - (PlayerPosition*(float)Math.PI / 2f);
            
            SetDirection(theta);
        }

        private void DirectionBoundarySetter(){
            if (LeftBoudaryCheck() || RightBoudaryCheck()) {
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
            if (gameEvent.EventType == GameEventType.ControlEvent) {
                switch(gameEvent.StringArg1) {
                    case "PlayerCollision":
                        float PlayerPosition = float.Parse (gameEvent.Message);
                        DirectionPlayerSetter(PlayerPosition);
                        break;
                    case "Move":
                        this.Move();
                        break;
                }
            }
        }

    }
}