using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.GamePlay.BlockEntity {
    public class Unbreakable : Block {
        
        public Unbreakable(DynamicShape shape, IBaseImage image) : base (shape, image) {}

        protected override void BlockHit() {}
    }
}