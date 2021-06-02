using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Timers;

namespace Breakout.GamePlay.BlockEntity {
    /// <summary>
    /// Stationary Block in Window that interacts with Balls.
    /// Is deleted when hit enough times by Balls.
    /// Can be subtyped to give further functionality.
    /// </summary>
    public class Block : Entity {
        protected IBaseImage damageImage;

        //Hit Points determine how many hits this Block can take before being deleted
        public int StartHP { get; protected set; }
        public int HP { get; protected set; } = 1;
        public bool Alive { get; protected set; } = true;

        public Block(Shape shape, IBaseImage image, IBaseImage damageImage) 
            : base(shape, image) {
                this.damageImage = damageImage;
                StartHP = HP;
        }

        /// <summary>
        /// Apply one piece of damage to HP of this Block.
        /// </summary>
        protected void Damage() {
            HP--;
        }

        /// <summary>
        /// Check whether this Block is alive or not.
        /// </summary>
        /// <returns>A boolean answer.</returns>
        protected bool IsAlive() {
            if (HP <= 0) {
                Alive = false; 
                return Alive;
            } else {
                Alive = true;
                return Alive;
            }
        }

        /// <summary>
        /// Check whether this Block is at half or lower HP, counting from StartHP.
        /// </summary>
        /// <returns>A boolean answer.</returns>
        protected bool HalfHpCheck() {
            if (StartHP / 2 >= HP) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// React to Ball collision by taking damage and assessing the consequences.
        /// </summary>
        public virtual void BlockHit() {
            Damage();
            if (HalfHpCheck()) {
                this.Image = damageImage;
            }
            if (!IsAlive()) {
                Delete();
            }
        }

        /// <summary>
        /// Mark this Block for deletion if not already marked, then broadcast that 
        /// the deletion is happening.
        /// </summary>
        protected void Delete() {
            if (!IsDeleted()) {
                DeleteEntity();

                //Add small delay so that EntityContainer will have cleaned up
                //this Block marked for deletion by the time of Block counting
                BreakoutBus.GetBus().RegisterTimedEvent(
                    new GameEvent {
                        EventType = GameEventType.ControlEvent, StringArg1 = "BLOCK_DELETED",
                        From = this },
                    TimePeriod.NewMilliseconds(BreakoutBus.CountDelay));
            }  
        }
    }
}