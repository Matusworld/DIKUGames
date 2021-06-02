using System;

using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Physics;

using DIKUArcade.Graphics;

/// <summary>
/// Ball that moves around the window and interacts with other entities.
/// Detects and computes new direction in case of boundary collision.
/// Computes new direction in case of block and player collision (but does not detect collisions).
/// Applies effect of power ups.
/// </summary>
namespace Breakout.GamePlay.BallEntity {
    public class Ball : Entity {   
        const float baseSpeed = 0.02f;
        public float speed { get; private set; }
        public bool HalfSpeedActive { get; private set; }
        public bool DoubleSpeedActive { get; private set; }

        public Ball(DynamicShape shape, IBaseImage image, float theta, bool halfSpeedActive, 
            bool doubleSpeedActive): base (shape, image) {
                speed = baseSpeed;
                
                HalfSpeedActive = halfSpeedActive;
                DoubleSpeedActive = doubleSpeedActive;
                if (HalfSpeedActive) {
                    speed *= 0.5f;
                }
                if (DoubleSpeedActive) {
                    speed *= 2f;
                }

                SetAngularDirection(theta);
        }

        /// <summary>
        /// Set direction vector given by canonical angle theta.
        /// </summary>
        /// <param name="theta"></param>
        private void SetAngularDirection(float theta) {
            this.Shape.AsDynamicShape().Direction.X = (float)Math.Cos((double)theta)*speed;
            this.Shape.AsDynamicShape().Direction.Y = (float)Math.Sin((double)theta)*speed;
        }

        /// <summary>
        /// Return canonical angle of direction vector.
        /// </summary>
        /// <returns></returns>
        public float GetTheta() {
            float Dx = Shape.AsDynamicShape().Direction.X;
            float Dy = Shape.AsDynamicShape().Direction.Y;
            return (float) Math.Atan2(Dy, Dx);
        }

        private void InverseDirectionX() {
            Shape.AsDynamicShape().Direction.X = -Shape.AsDynamicShape().Direction.X;
        }

        private void InverseDirectionY() {
            Shape.AsDynamicShape().Direction.Y = -Shape.AsDynamicShape().Direction.Y;
        }

        /// <summary>
        /// Detects whether the ball has exceeded the left window border.
        /// </summary>
        /// <returns></returns>
        public bool LeftBoundaryCheck() {
            if (Shape.Position.X < 0.0f) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Detects whether the ball has exceeded the right window border.
        /// </summary>
        /// <returns></returns>
        public bool RightBoundaryCheck() {
            if (Shape.Position.X + Shape.Extent.X > 1.0f) {
                return true;
            } else {
                return false;
            }           
        }

        /// <summary>
        /// Detects whether the ball has exceeded the top window border.
        /// </summary>
        /// <returns></returns>
        public bool UpperBoundaryCheck() {
            if (Shape.Position.Y + Shape.Extent.Y > 1.0f) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Detects whether the ball has exceeded the bottom window border.
        /// </summary>
        /// <returns></returns>
        public bool LowerBoundaryCheck() {
            if (Shape.Position.Y < 0.0f) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Reacts to left-, right-, and top boundary detection by reflecting the direction vector 
        /// accordingly.
        /// </summary>
        public void DirectionBoundarySetter() {
            if (LeftBoundaryCheck() || RightBoundaryCheck()) {
                InverseDirectionX();
            }
            if (UpperBoundaryCheck()) {
                InverseDirectionY();
            }
        }

        /// <summary>
        /// Reacts to block collision by reflecting the direction vector according to the 
        /// collision direction, i.e. from which side the ball was hit.
        /// </summary>
        /// <param name="dir"></param>
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

        /// <summary>
        /// Reacts to player collision by computing the direction based on the PlayerPosition.
        /// PlayerPosition is a ratio in the range [0.0,1.0]
        /// Computed direction has a canonical angle in the range of [45.0, 135.0] degrees
        /// </summary>
        /// <param name="PlayerPosition"></param>
        public void DirectionPlayerSetter(float PlayerPosition) {
            //rebound angle depending on hit position
            float theta = 0.75f * (float) Math.PI - (PlayerPosition * (float) Math.PI / 2f);
            SetAngularDirection(theta);
        }

        /// <summary>
        /// Mark for deletion if not already marked, then broadcast it.
        /// </summary>
        private void Delete() {
            if (!IsDeleted()) {
                this.DeleteEntity();

                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.ControlEvent, StringArg1 = "BALL_DELETED"});
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