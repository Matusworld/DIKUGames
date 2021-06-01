using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.GamePlay.BlockEntity {
    public class Hardened : Block {

        public Hardened(DynamicShape shape, IBaseImage image, IBaseImage damageImage) 
            : base(shape, image, damageImage) {
                HP = HP * 2;
                //necesarry due to constructor call order
                MaxHP = HP;
        }
    }
}