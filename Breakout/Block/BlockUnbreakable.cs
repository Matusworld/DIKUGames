using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.Blocks {
    public class UnbreakableBlock : Block {
        
        public UnbreakableBlock(DynamicShape shape, IBaseImage image) : base (shape, image) {
        }

        protected override void BlockHit() {
        }
    }
}