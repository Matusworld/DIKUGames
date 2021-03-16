using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.EventBus;

namespace Galaga {
    public class Enemy : Entity, IGameEventProcessor<object> {
        private int hitpoints = 50;
        private int HPThreshhold = 15;
        private IBaseImage redImage;
        private float MOVEMENT_SPEED = 0.01f;
        private bool enraged = false;
        private bool dead = false;

        public Enemy(DynamicShape shape, IBaseImage image, IBaseImage redImage)
            : base(shape, image) {this.redImage = redImage;}

        public void Damage() {
            hitpoints -= 10;
        }

        public bool isDead(){
            if (hitpoints <= 0) {
                dead = true;
                return dead;
            } else {
                dead = false;
                return dead;
            }
        }

        public bool EnrageCheck() {
            if (hitpoints < HPThreshhold) { 
                enraged = true; 
                return enraged;
            } else {
                enraged = false;
                return enraged;
            }
        }
        
        public void Enrage() {
            if (enraged) {
                MOVEMENT_SPEED = 0.02f;
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