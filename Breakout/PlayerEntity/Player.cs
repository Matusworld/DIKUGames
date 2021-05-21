using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout.PlayerEntity {
    public class Player : Entity, IGameEventProcessor {
        //private Entity entity;
        //private DynamicShape shape;
        private float moveLeft = 0.0f;
        private uint startLives = 3;
        private uint maxLives = 5;
        private float moveRight = 0.0f;
        private const float MOVEMENT_SPEED = 0.02f;
        public float LeftBound { get; private set; }
        public float RightBound { get; private set; }

        public Healthbar Hpbar { get; private set; }
        public Player(DynamicShape shape, IBaseImage image) : base(shape, image) {
            BreakoutBus.GetBus().Subscribe(GameEventType.PlayerEvent, this); 

            LeftBound = 0f;
            RightBound = 1.0f - shape.Extent.X;

            Hpbar = new Healthbar(startLives, maxLives);
        }

        public Vec2F GetPosition() {
            return Shape.Position; 
        }

        public Vec2F GetExtent() {
            return Shape.Extent;
        }

        public float GetMoveSpeed() {
            return MOVEMENT_SPEED;
        }

        public void Render() {
            this.RenderEntity();
        }

        public void UpdateDirection() {
            Shape.AsDynamicShape().Direction.X = moveLeft + moveRight;
        }

        //Move if position after move will not be out of bounds
        private void Move() {
            if (Shape.Position.X + Shape.AsDynamicShape().Direction.X < LeftBound) {
                Shape.Position.X = LeftBound;
            } //pos is from bottom left corner
            else if (Shape.Position.X + Shape.AsDynamicShape().Direction.X > RightBound) { 
                Shape.Position.X = RightBound;
            } else {
                Shape.Move();
            }
        }
        
        private void SetMoveLeft(bool val) {
            if (val) {
                moveLeft = -MOVEMENT_SPEED;
            }
            else {
                moveLeft = 0.0f;
            }
            UpdateDirection();
        }
        private void SetMoveRight(bool val) {
            if (val){
                moveRight = MOVEMENT_SPEED;
            } 
            else {
                moveRight = 0.0f;
            }
            UpdateDirection();
        }

        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.PlayerEvent) {  
                switch (gameEvent.StringArg1) {
                    case "SetMoveLeft":
                        switch (gameEvent.Message) {
                            case "true":
                                SetMoveLeft(true);
                                break;
                            case "false":
                                SetMoveLeft(false);
                                break;
                        }
                        break;
                    case "SetMoveRight":
                        switch (gameEvent.Message) {
                            case "true":
                                SetMoveRight(true);
                                break;
                            case "false":
                                SetMoveRight(false);
                                break;
                        }
                        break;
                    case "Move":
                        Move();
                        break;
                }
            }
        }
    }
}

