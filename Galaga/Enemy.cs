using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.EventBus;
using DIKUArcade.Math;
namespace Galaga {
    public class Enemy : Entity, IGameEventProcessor<object> {
        public int Hitpoints { get; private set; } = 50;
        private int HPThreshhold = 15;
        private IBaseImage redImage;
        public float MOVEMENT_SPEED { get; private set; }
        public bool Enraged { get; private set; } = false;
        public bool Dead { get; private set; } = false;
        private float enragedMovementspeed; 

        public float EnrangedMultiplier { get; private set; } = 10f;

        public readonly Vec2F startPos;

        public Enemy(DynamicShape shape, IBaseImage image, IBaseImage redImage, float speed)
            : base(shape, image) {this.redImage = redImage; this.startPos = shape.Position;
                this.MOVEMENT_SPEED = speed; enragedMovementspeed = speed * EnrangedMultiplier;}

        private void Damage() {
            Hitpoints -= 10;
        }

        private bool isDead() {
            if (Hitpoints <= 0) {
                Dead = true;
                return Dead;
            } else {
                Dead = false;
                return Dead;
            }
        }

        private bool EnrageCheck() {
            if (Hitpoints < HPThreshhold) { 
                Enraged = true; 
                return Enraged;
            } else {
                Enraged = false;
                return Enraged;
            }
        }
        
        private void Enrage() {
            if (Enraged) {
                MOVEMENT_SPEED = enragedMovementspeed;
                this.Image = redImage;
            }
        }

        public void ProcessEvent(GameEventType type, GameEvent<object> gameEvent) {
            if (type == GameEventType.ControlEvent) {
                 //check if this is the hit enemy
                if ((Enemy)gameEvent.To == this) {
                    switch (gameEvent.Parameter1) {
                        case "Damage": 
                            Damage();
                            if (isDead()) {
                                GalagaBus.GetBus().RegisterEvent(
                                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                                        GameEventType.GraphicsEvent, this, "", "Explosions", ""));
                                DeleteEntity();
                            }
                            if (EnrageCheck()) {
                                Enrage();
                            }
                            break;
                    }
                }
            }
        }
    }
}