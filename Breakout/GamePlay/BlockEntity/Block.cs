using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout.GamePlay.BlockEntity {
    public class Block : Entity, IGameEventProcessor {
        public bool Alive { get; protected set; } = true;

        public int HP { get; protected set; } = 1;
        public string Value { get; protected set; }

        public Block(DynamicShape shape, IBaseImage image): base(shape, image) {}
        public Vec2F GetPosition() {
            return this.Shape.Position; 
        }

        protected void Damage() {
            HP--;
        }

        protected bool IsAlive() {
            if(HP <= 0) {
                Alive = false; 
                return Alive;
            } else {
                Alive = true;
                return Alive;
            }
        }

        protected virtual void BlockHit() {
            Damage();
            if (!IsAlive()) {
                // Unsubscribe deleted blocks
                BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, this);
                DeleteEntity();

                // Could add points here
                BreakoutBus.GetBus().RegisterEvent( new GameEvent { 
                    EventType = GameEventType.ControlEvent, StringArg1 = "ADD_SCORE",
                        From = this});
            }
        }

        public void ProcessEvent(GameEvent gameEvent) {
            if(gameEvent.EventType == GameEventType.ControlEvent ) {
                if((Block) gameEvent.To == this) {
                    if(gameEvent.StringArg1 == "BlockCollision") {
                        BlockHit();
                    }
                }
            }
        }
    }
}