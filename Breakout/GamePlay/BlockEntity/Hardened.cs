using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.GamePlay.BlockEntity {
    /// <summary>
    /// Hardened Block has twice the amount of StartHP in relation to Normal Block.
    /// </summary>
    public class Hardened : Block {
        public Hardened(Shape shape, IBaseImage image, IBaseImage damageImage) 
            : base(shape, image, damageImage) {
                HP = HP * 2;
                //assignment necessary due to constructor call order
                StartHP = HP;
        }
    }
}