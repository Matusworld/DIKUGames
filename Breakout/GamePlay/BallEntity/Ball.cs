using System;

using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Physics;
using DIKUArcade.Timers;

using DIKUArcade.Graphics;

namespace Breakout.GamePlay.BallEntity {
    /// <summary>
    /// Ball that moves around the window and interacts with other Entities.
    /// Detects and computes new Direction in case of boundary collision.
    /// Computes new Direction in case of Block and Player collision (but does not detect collisions).
    /// Applies effect of PowerUpOrbs.
    /// </summary>
    public class Ball : Entity {   
        private const float baseSpeed = 0.02f;
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
        /// Set Direction vector of this Ball given by the canonical angle theta.
        /// </summary>
        /// <param name="theta">The input canonical angle.</param>
        private void SetAngularDirection(float theta) {
            Shape.AsDynamicShape().Direction.X = (float)Math.Cos((double)theta)*speed;
            Shape.AsDynamicShape().Direction.Y = (float)Math.Sin((double)theta)*speed;
        }

        /// <summary>
        /// Return canonical angle of the Direction vector of this Ball.
        /// </summary>
        /// <returns>The canonical angle.</returns>
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
        /// Detect whether this Ball has exceeded the left window border.
        /// </summary>
        /// <returns>The boolean result.</returns>
        public bool LeftBoundaryCheck() {
            if (Shape.Position.X < 0.0f) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Detect whether this Ball has exceeded the right window border.
        /// </summary>
        /// <returns>The boolean result.</returns>
        public bool RightBoundaryCheck() {
            if (Shape.Position.X + Shape.Extent.X > 1.0f) {
                return true;
            } else {
                return false;
            }           
        }

        /// <summary>
        /// Detect whether this Ball has exceeded the top window border.
        /// </summary>
        /// <returns>The boolean result.</returns>
        public bool UpperBoundaryCheck() {
            if (Shape.Position.Y + Shape.Extent.Y > 1.0f) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Detect whether this Ball has exceeded the bottom window border.
        /// </summary>
        /// <returns>The boolean result.</returns>
        public bool LowerBoundaryCheck() {
            if (Shape.Position.Y < 0.0f) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// React to boundary conditions by reflection of this Ball's Direction vector or 
        /// by deletion of this Ball. 
        /// </summary>
        public void BoundaryCollision() {
            if (LeftBoundaryCheck() || RightBoundaryCheck()) {
                InverseDirectionX();
            }
            if (UpperBoundaryCheck()) {
                InverseDirectionY();
            }
            if (LowerBoundaryCheck()) {
                Delete();
            }
        }

        /// <summary>
        /// React to Block collision by reflecting this Ball's direction vector according to the 
        /// CollisionDirection, i.e. from which side the Block was hit by this Ball.
        /// </summary>
        /// <param name="dir">The side from which the Block was hit by this Ball.</param>
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
        /// React to Player collision by computing the new Direction of this Ball based 
        /// on the PlayerPosition.
        /// Computed Direction has a canonical angle in the range of [45.0, 135.0] degrees
        /// </summary>
        /// <param name="PlayerPosition">PlayerPosition is a ratio in the range [0.0,1.0].</param>
        public void DirectionPlayerSetter(float PlayerPosition) {
            //rebound angle depending on hit position
            float theta = 0.75f * (float) Math.PI - (PlayerPosition * (float) Math.PI / 2f);
            SetAngularDirection(theta);
        }

        /// <summary>
        /// Mark this Ball for deletion if not already marked, then broadcast that 
        /// the deletion is happening.
        /// </summary>
        private void Delete() {
            if (!IsDeleted()) {
                DeleteEntity();

                //Add small delay so that EntityContainer will have cleaned up
                //this Ball marked for deletion by the time of Ball counting
                BreakoutBus.GetBus().RegisterTimedEvent(
                    new GameEvent { 
                        EventType = GameEventType.ControlEvent,
                        StringArg1 = "BALL_DELETED"},
                    TimePeriod.NewMilliseconds(BreakoutBus.CountDelay));
            }
        }

        /// <summary>
        /// Move this Ball by its Direction vector.
        /// </summary>
        public void Move() {
            Shape.Move();
        }

        /// <summary>
        /// Activate HalfSpeedOrb PowerUp effect.
        /// </summary>
        public void HalfSpeedActivate() {
            if (!HalfSpeedActive) {
                HalfSpeedActive = true;
                speed = speed * 0.5f;
                //update Direction vector length
                SetAngularDirection(GetTheta());
            }
        }

        /// <summary>
        /// Deactivate HalfSpeedOrb PowerUp effect.
        /// </summary>
        public void HalfSpeedDeactivate() {
            if (HalfSpeedActive) {
                HalfSpeedActive = false;
                speed = speed * 2f;
                //update direction vector length
                SetAngularDirection(GetTheta());
            }
        }

        /// <summary>
        /// Activate DoubleSpeedOrb PowerUp effect.
        /// </summary>
        public void DoubleSpeedActivate() {
            if (!DoubleSpeedActive) {
                DoubleSpeedActive = true;
                speed = speed * 2f;
                //update direction vector length
                SetAngularDirection(GetTheta());
            }
        }

        /// <summary>
        /// Deactivate DoubleSpeedOrb PowerUp effect.
        /// </summary>
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