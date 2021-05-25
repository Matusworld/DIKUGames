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

namespace Breakout.GamePlay.BallEntity {
    public class Ball : Entity, IGameEventProcessor {   

        const float baseSpeed = 0.02f;
        float speed;
        public float Theta { get; private set; }
        public bool Active { get; private set; } = true;
        public bool HalfSpeedActive;
        public bool DoubleSpeedActive;
        const int bounceDelay = 2;

        public Ball(DynamicShape shape, IBaseImage image, float theta): base (shape, image) {
            speed = baseSpeed;
            Theta = theta;
            BreakoutBus.GetBus().Subscribe(GameEventType.MovementEvent, this);
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);

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
            UpdateTheta();
        }
        public void Move() {
            if (LowerBoundaryCheck()) {
                this.DeleteEntity();
                BreakoutBus.GetBus().Unsubscribe(GameEventType.MovementEvent, this);
                BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, this);
                
            } else {
                DirectionBoundarySetter(); 
                this.Shape.Move();
            }
        }
        private void Deactivate() {
            //Don't switch direction for a short while
            this.Active = false;
            //After some time Activate again
            BreakoutBus.GetBus().RegisterTimedEvent(
                new GameEvent{ EventType = GameEventType.ControlEvent,
                    StringArg1 = "BallActivate", To = this },
                TimePeriod.NewMilliseconds(bounceDelay));
        }
        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.To == this) {
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
                                        UpdateTheta();
                                        break;
                                    case "LeftRight":
                                        this.Shape.AsDynamicShape().Direction.X = 
                                            -this.Shape.AsDynamicShape().Direction.X;
                                        UpdateTheta();
                                        break;
                                }
                                Deactivate();
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
                        case "DoubleSpeed":
                            switch(gameEvent.Message) {
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
                            break; 
                            
                        case "HalfSpeed":
                            switch(gameEvent.Message) {
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
                            break; 
                    }
                }
            }
        }

    }
}