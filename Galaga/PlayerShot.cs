using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

namespace Galaga {
    class PlayerShot : Entity {
        private static Vec2F extent = new Vec2F(0.008f, 0.021f);
        private static Vec2F direction = new Vec2F(0.0f, 0.025f);    

        public PlayerShot(Vec2F position, IBaseImage image) 
            : base(new DynamicShape(position,extent,direction), image) {}
    }
}