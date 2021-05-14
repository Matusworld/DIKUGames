using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.Blocks {
    public class Hardened : Block {
        private IBaseImage damageImage;
        private int maxHP;
        public Hardened(DynamicShape shape, IBaseImage image, IBaseImage damageImage) 
            : base(shape, image) {
                this.damageImage = damageImage;
                HP = HP * 2;
                maxHP = HP;
        }

        private bool HalfHpCheck() {
            if (maxHP / 2 >= HP) {
                return true;
            } else {
                return false;
            }
        }

        protected override void BlockHit() {
            Damage();
            if (HalfHpCheck()) {
                this.Image = damageImage;
            }
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
    }
}