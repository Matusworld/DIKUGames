using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.GamePlay.BlockEntity {
    public class Unbreakable : Block {
        
        public Unbreakable(DynamicShape shape, IBaseImage image, IBaseImage damageImage) 
            : base (shape, image, damageImage) {}

        protected override void BlockHit() {}
    }
}