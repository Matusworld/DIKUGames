using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout.GamePlay.BlockEntity {
    public class Block : Entity {
        public bool Alive { get; protected set; } = true;
        public int HP { get; protected set; } = 1;

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

        protected void ScoreEvent() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent { 
                EventType = GameEventType.ControlEvent, StringArg1 = "ADD_SCORE",
                    From = this});
        }

        protected virtual void BlockHit() {
            Damage();
            if (!IsAlive()) {
                DeleteEntity();
                ScoreEvent();
            }
        }

        public void ReceiveEvent(GameEvent gameEvent) {
            if(gameEvent.EventType == GameEventType.ControlEvent ) {
                if(gameEvent.StringArg1 == "BlockCollision") {
                    BlockHit();
                }
            }
        }
    }
}