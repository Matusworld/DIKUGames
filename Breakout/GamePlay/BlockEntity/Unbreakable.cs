using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.GamePlay.BlockEntity {
    /// <summary>
    /// Unbreakable Block cannot be destroyed. 
    /// Level will progress when only Unbreakable Blocks remain.
    /// </summary>
    public class Unbreakable : Block {
        public Unbreakable(Shape shape, IBaseImage image, IBaseImage damageImage) 
            : base (shape, image, damageImage) {}

        /// <summary>
        /// Unbreakable Blocks does not take damage.
        /// </summary>
        public override void BlockHit() {}
    }
}