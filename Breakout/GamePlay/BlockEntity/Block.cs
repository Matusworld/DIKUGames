using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout.GamePlay.BlockEntity {
    public class Block : Entity {
        public bool Alive { get; protected set; } = true;
        protected IBaseImage damageImage;
        public int MaxHP { get; protected set; }
        public int HP { get; protected set; } = 1;

        public Block(DynamicShape shape, IBaseImage image, IBaseImage damageImage) 
            : base(shape, image) {
                this.damageImage = damageImage;
                MaxHP = HP;
        }
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

        protected bool HalfHpCheck() {
            if (MaxHP / 2 >= HP) {
                return true;
            } else {
                return false;
            }
        }

        protected void ScoreEvent() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent { 
                EventType = GameEventType.ControlEvent, StringArg1 = "ADD_SCORE",
                    From = this});
        }

        public virtual void BlockHit() {
            Damage();
            if (HalfHpCheck()) {
                this.Image = damageImage;
            }
            if (!IsAlive()) {
                Delete();
            }
        }

        protected void Delete() {
            DeleteEntity();

            ScoreEvent();

            BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                EventType = GameEventType.ControlEvent,
                StringArg1 = "BLOCK_DELETED"});
        }
    }
}