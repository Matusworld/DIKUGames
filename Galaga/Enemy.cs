using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.EventBus;
using DIKUArcade.Math;
namespace Galaga {
    public class Enemy : Entity, IGameEventProcessor<object> {
        private int hitpoints = 50;
        private int HPThreshhold = 15;
        private IBaseImage redImage;
        public float MOVEMENT_SPEED { get; private set; }
        private bool enraged = false;
        private bool dead = false;
        private float enragedMovementspeed; 

        private float enrangedMultiplier = 10f;

        public readonly Vec2F startPos;

        public Enemy(DynamicShape shape, IBaseImage image, IBaseImage redImage, float speed)
            : base(shape, image) {this.redImage = redImage; this.startPos = shape.Position;
                this.MOVEMENT_SPEED = speed; enragedMovementspeed = speed * enrangedMultiplier;}

        private void Damage() {
            hitpoints -= 10;
        }

        private bool isDead(){
            if (hitpoints <= 0) {
                dead = true;
                return dead;
            } else {
                dead = false;
                return dead;
            }
        }

        private bool EnrageCheck() {
            if (hitpoints < HPThreshhold) { 
                enraged = true; 
                return enraged;
            } else {
                enraged = false;
                return enraged;
            }
        }
        
        private void Enrage() {
            if (enraged) {
                MOVEMENT_SPEED = enragedMovementspeed;
                this.Image = redImage;
            }
        }

        public void ProcessEvent(GameEventType type, GameEvent<object> gameEvent) {
            if (type == GameEventType.EnemyEvent) {
                 //check if this is the hit enemy
                if ((Enemy)gameEvent.To == this) {
                    switch (gameEvent.Parameter1) {
                        case "Damage": 
                            Damage();
                            if (isDead()) {
                                Game.eventBus.RegisterEvent(
                                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                                        GameEventType.GraphicsEvent, this, "", "Explosions", ""));
                                DeleteEntity();
                                // Explosions
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