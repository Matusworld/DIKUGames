using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout.GamePlay.PlayerEntity {
    /// <summary>
    /// Player moves horizontally in the lower part of the window based on user input.
    /// Cannot move past window boundaries.
    /// Interacts with Balls and PowerUpOrbs.
    /// </summary>
    public class Player : Entity, IGameEventProcessor {
        private readonly Vec2F startPos;

        private float moveLeft = 0.0f;
        private float moveRight = 0.0f;
        public const float MOVEMENT_SPEED = 0.02f;

        public Healthbar Healthbar { get; private set; }
        private uint startLives = 3;
        private uint maxLives = 5;

        public Player(DynamicShape shape, IBaseImage image) : base(shape, image) {
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this); 

            startPos = Shape.Position;

            Healthbar = new Healthbar(startLives, maxLives);
        }

        /// <summary>
        /// Detect whether this Player will exceed the left window border if one more movement.
        /// </summary>
        /// <returns>The boolean result.</returns>
        public bool LeftBoundaryCheck() {
            if (Shape.Position.X + Shape.AsDynamicShape().Direction.X < 0.0f) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Detect whether this Player will exceed the right window border if one more movement.
        /// </summary>
        /// <returns>The boolean result.</returns>
        public bool RightBoundaryCheck() {
            if (Shape.Position.X + Shape.Extent.X + Shape.AsDynamicShape().Direction.X > 1.0f) {
                return true;
            } else {
                return false;
            }           
        }

        /// <summary>
        /// React to upcoming boundary exceedance by setting Position to boundary.
        /// </summary>
        /// <returns>A boolean value depending on whether the Position was set or not.</returns>
        public bool BoundaryCollision() {
            if (LeftBoundaryCheck()) {
                Shape.Position.X = 0.0f;
                return true;
            } else if (RightBoundaryCheck()) {
                Shape.Position.X = 1.0f - Shape.Extent.X;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Update the Direction vector of this Player according to the sum 
        /// of moveLeft and moveRight
        /// </summary>
        public void UpdateDirection() {
            Shape.AsDynamicShape().Direction.X = moveLeft + moveRight;
        }

        /// <summary>
        /// Set or nullify left-pointing constant contribution to the Player's movement.
        /// </summary>
        /// <param name="move">true if it should move</param>
        public void SetMoveLeft(bool move) {
            if (move) {
                moveLeft = -MOVEMENT_SPEED;
            }
            else {
                moveLeft = 0.0f;
            }
            UpdateDirection();
        }

        /// <summary>
        /// Set or nullify right-pointing constant contribution to the Player's movement.
        /// </summary>
        /// <param name="move">true if it should move</param>
        public void SetMoveRight(bool move) {
            if (move){
                moveRight = MOVEMENT_SPEED;
            } 
            else {
                moveRight = 0.0f;
            }
            UpdateDirection();
        }

        /// <summary>
        /// Move this Player by its Direction vector.
        /// Do not move if it would exceed a boundary.
        /// </summary>
        public void Move() {
            if (!BoundaryCollision()) {
                Shape.Move();
            }
        }

        public void Render() {
            this.RenderEntity();
        }
        
        /// <summary>
        /// Reset this Player to its initial state.
        /// </summary>
        public void Reset() {
            SetMoveLeft(false);
            SetMoveRight(false);

            Shape.Position = startPos;
        }

        /// <summary>
        /// Process Events related to Player.
        /// </summary>
        /// <param name="gameEvent"></param>
        public void ProcessEvent(GameEvent gameEvent) {
            switch (gameEvent.StringArg1) {
                case "LEVEL_ENDED":
                case "LEVEL_BACK":
                    Reset();
                    break;
            }
        }
    }
}

