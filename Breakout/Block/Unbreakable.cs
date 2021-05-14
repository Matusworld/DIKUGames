using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.Blocks {
    public class Unbreakable : Block {
        
        public Unbreakable(DynamicShape shape, IBaseImage image) : base (shape, image) {}

        protected override void BlockHit() {}
    }
}