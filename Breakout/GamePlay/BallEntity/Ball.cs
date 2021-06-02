using System;

using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Physics;

using DIKUArcade.Graphics;

namespace Breakout.GamePlay.BallEntity {
    public class Ball : Entity {   
        const float baseSpeed = 0.02f;
        public float speed { get; private set; }
        public bool HalfSpeedActive { get; private set; }
        public bool DoubleSpeedActive { get; private set; }

        public Ball(DynamicShape shape, IBaseImage image, float theta, bool halfSpeedActive, 
            bool doubleSpeedActive): base (shape, image) {
                HalfSpeedActive = halfSpeedActive;
                DoubleSpeedActive = doubleSpeedActive;
                speed = baseSpeed;
                if (HalfSpeedActive) {
                    speed *= 0.5f;
                }
                if (DoubleSpeedActive) {
                    speed *= 2f;
                }

                SetAngularDirection(theta);
        }

        private void SetAngularDirection(float theta) {
            this.Shape.AsDynamicShape().Direction.X = (float)Math.Cos((double)theta)*speed;
            this.Shape.AsDynamicShape().Direction.Y = (float)Math.Sin((double)theta)*speed;
        }

        public float GetTheta() {
            float Dx = Shape.AsDynamicShape().Direction.X;
            float Dy = Shape.AsDynamicShape().Direction.Y;
            return (float) Math.Atan2(Dy,Dx);
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
        }

        private void InverseDirectionY() {
            Shape.AsDynamicShape().Direction.Y = -Shape.AsDynamicShape().Direction.Y;
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

        public void DirectionPlayerSetter(float PlayerPosition) {
            //rebound angle depending on hit position
            float theta = 0.75f * (float) Math.PI - (PlayerPosition*(float)Math.PI / 2f);
            SetAngularDirection(theta);
        }



        private void Delete() {
            if (!IsDeleted()) {
                this.DeleteEntity();

                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.ControlEvent, StringArg1 = "BALL_REMOVED"});
            }
        }

        public void Move() {
            if (LowerBoundaryCheck()) {
                Delete();
                
            } else {
                DirectionBoundarySetter(); 
                this.Shape.Move();
            }
        }

        public void HalfSpeedActivate() {
            if (!HalfSpeedActive) {
                HalfSpeedActive = true;
                speed = speed * 0.5f;
                //update direction vector length
                SetAngularDirection(GetTheta());
            }
        }

        public void HalfSpeedDeactivate() {
            if (HalfSpeedActive) {
                HalfSpeedActive = false;
                speed = speed * 2f;
                //update direction vector length
                SetAngularDirection(GetTheta());
            }
        }

        public void DoubleSpeedActivate() {
            if (!DoubleSpeedActive) {
                DoubleSpeedActive = true;
                speed = speed * 2f;
                //update direction vector length
                SetAngularDirection(GetTheta());
            }
        }

        public void DoubleSpeedDeactivate() {
            if (DoubleSpeedActive) {
                DoubleSpeedActive = false;
                speed = speed * 0.5f;
                //update direction vector length
                SetAngularDirection(GetTheta());
            }
        }
    }
}