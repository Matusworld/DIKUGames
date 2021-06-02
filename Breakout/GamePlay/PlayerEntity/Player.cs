using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout.GamePlay.PlayerEntity {
    public class Player : Entity, IGameEventProcessor {
        private readonly Vec2F startPos;
        private float moveLeft = 0.0f;
        private float moveRight = 0.0f;
        public const float MOVEMENT_SPEED = 0.02f;
        public float LeftBound { get; private set; }
        public float RightBound { get; private set; }

        public Healthbar Healthbar { get; private set; }
        private uint startLives = 3;
        private uint maxLives = 5;

        public Player(DynamicShape shape, IBaseImage image) : base(shape, image) {
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this); 

            LeftBound = 0f;
            RightBound = 1.0f - shape.Extent.X;

            startPos = Shape.Position;

            Healthbar = new Healthbar(startLives, maxLives);
        }

        public void Render() {
            this.RenderEntity();
        }

        public void UpdateDirection() {
            Shape.AsDynamicShape().Direction.X = moveLeft + moveRight;
        }

        public void Reset() {
            SetMoveLeft(false);
            SetMoveRight(false);

            Shape.Position = startPos;
        }

        //Move if position after move will not be out of bounds
        public void Move() {
            if (Shape.Position.X + Shape.AsDynamicShape().Direction.X < LeftBound) {
                Shape.Position.X = LeftBound;
            } //pos is from bottom left corner
            else if (Shape.Position.X + Shape.AsDynamicShape().Direction.X > RightBound) { 
                Shape.Position.X = RightBound;
            } else {
                Shape.Move();
            }
        }
        
        public void SetMoveLeft(bool val) {
            if (val) {
                moveLeft = -MOVEMENT_SPEED;
            }
            else {
                moveLeft = 0.0f;
            }
            UpdateDirection();
        }
        public void SetMoveRight(bool val) {
            if (val){
                moveRight = MOVEMENT_SPEED;
            } 
            else {
                moveRight = 0.0f;
            }
            UpdateDirection();
        }

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

