using System.Collections.Generic;
using System.IO;
using System;
using Breakout;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Physics;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using DIKUArcade.Graphics;

namespace Breakout.GamePlay.BallEntity {
    public class Ball : Entity {   

        const float baseSpeed = 0.02f;
        public float speed { get; private set; }
        public float Theta { get; private set; }
        public bool HalfSpeedActive { get; private set; }
        public bool DoubleSpeedActive { get; private set; }

        public Ball(DynamicShape shape, IBaseImage image, float theta): base (shape, image) {
            speed = baseSpeed;
            Theta = theta;

            SetDirection(theta);
        }

        private void UpdateTheta() {
            float Dx = Shape.AsDynamicShape().Direction.X;
            float Dy = Shape.AsDynamicShape().Direction.Y;
            Theta = (float) Math.Atan2(Dy,Dx);
        }

        private void SetDirection(float theta) {
            this.Shape.AsDynamicShape().Direction.X = (float)Math.Cos((double)theta)*speed;
            this.Shape.AsDynamicShape().Direction.Y = (float)Math.Sin((double)theta)*speed;
            UpdateTheta();
        }

        public bool LeftBoundaryCheck() {
            if (this.Shape.Position.X < 0.0f) {
                return true;
            } else {
                return false;
            }
        }
        public bool RightBoundaryCheck() {
            if (this.Shape.Position.X+this.Shape.Extent.X > 1.0f) {
                return true;
            } else {
                return false;
            }           
        }
        public bool UpperBoundaryCheck() {
            if (this.Shape.Position.Y+this.Shape.Extent.Y > 1.0f) {
                return true;
            } else {
                return false;
            }
        }

        public bool LowerBoundaryCheck() {
            if (this.Shape.Position.Y < 0.0f) {
                return true;
            } else {
                return false;
            }
        }

        private void InverseDirectionX() {
            Shape.AsDynamicShape().Direction.X = -Shape.AsDynamicShape().Direction.X;
            UpdateTheta();
        }

        private void InverseDirectionY() {
            Shape.AsDynamicShape().Direction.Y = -Shape.AsDynamicShape().Direction.Y;
            UpdateTheta();
        }

        public void DirectionPlayerSetter(float PlayerPosition) {
            //rebound angle depending on hit position
            float theta = 0.75f * (float) Math.PI - (PlayerPosition*(float)Math.PI / 2f);
            SetDirection(theta);
        }

        public void DirectionBoundarySetter() {
            if (LeftBoundaryCheck() || RightBoundaryCheck()) {
                InverseDirectionX();
            }
            if (UpperBoundaryCheck()) {
                InverseDirectionY();
            }
        }

        public void DirectionBlockSetter(CollisionDirection dir) {
            switch (dir) {
                case CollisionDirection.CollisionDirUp:
                case CollisionDirection.CollisionDirDown:
                    InverseDirectionY();
                    break;
                case CollisionDirection.CollisionDirLeft:
                case CollisionDirection.CollisionDirRight:
                    InverseDirectionX();
                    break;
            }
        }

        public void Move() {
            if (LowerBoundaryCheck()) {
                this.DeleteEntity();
                
            } else {
                DirectionBoundarySetter(); 
                this.Shape.Move();
            }
        }

        private void ReceiveEventHalfSpeed(string Message) {
            switch(Message) {
                case "Activate":
                    if (!HalfSpeedActive) {
                        HalfSpeedActive = true;
                        speed = speed * 0.5f;
                        SetDirection(Theta);
                    }
                    break;
                case "Deactivate":
                    if (HalfSpeedActive) {
                        HalfSpeedActive = false;
                        speed = speed * 2f;
                        SetDirection(Theta);
                    }
                    break;
            }
        }

        private void ReceiveEventDoubleSpeed(string Message) {
            switch(Message) {
                case "Activate":
                    if (!DoubleSpeedActive) {
                        DoubleSpeedActive = true;
                        speed = speed * 2f;
                        //Update the direction vector
                        SetDirection(Theta);
                    }
                    break;
                case "Deactivate":
                    if (DoubleSpeedActive) {
                        DoubleSpeedActive = false;
                        speed = speed * 0.5f;
                        SetDirection(Theta);
                    }
                    break;
            }
        }

        public void ReceiveEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.ControlEvent) {
                switch(gameEvent.StringArg1) {
                    case "DoubleSpeed":
                        ReceiveEventDoubleSpeed(gameEvent.Message);
                        break;     
                    case "HalfSpeed":
                        ReceiveEventHalfSpeed(gameEvent.Message);
                        break; 
                }
            }
        }

    }
}