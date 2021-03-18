using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
namespace Galaga {
    public class Player : IGameEventProcessor<object> {
        private Entity entity;
        private DynamicShape shape;
        private float moveLeft = 0.0f;
        private float moveRight = 0.0f;
        private const float MOVEMENT_SPEED = 0.01f;
        public Player(DynamicShape shape, IBaseImage image) {
            entity = new Entity(shape, image);
            this.shape = shape;
        }

        public Vec2F GetPosition() {
            return shape.Position; 
        }

        public void Render() {
            entity.RenderEntity();
        }

        public void UpdateDirection() {
            shape.Direction.X = moveLeft + moveRight;
        }

        //Move if position after move will not be out of bounds
        private void Move() {
            if (shape.Position.X+shape.Direction.X < 0.0f) {
                shape.Position.X = 0.01f;
            }
            if (shape.Position.X+shape.Direction.X > 0.9f) { //pos is from bottom left corner 
                shape.Position.X = 0.9f;
            } else {
                shape.Move();
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

        public void ProcessEvent(GameEventType type, GameEvent<object> gameEvent) {
            if (type == GameEventType.PlayerEvent) {
                switch (gameEvent.Parameter1) {
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

