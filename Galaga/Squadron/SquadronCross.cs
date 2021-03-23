using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;

namespace Galaga.Squadron {
    public class SquadronCross : ISquadron {
        public EntityContainer<Enemy> Enemies { get; private set; }

        public int MaxEnemies { get; } = 24;

        public void CreateEnemies(List<Image> enemyStrides, 
            List<Image> alternativeEnemystrideStrides, float speed) {
                Enemies = new EntityContainer<Enemy>(MaxEnemies);
            
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 3; j++) {
                    if ((i+j) % 2 == 0) {
                        Enemy enemy = new Enemy(
                            new DynamicShape(
                                new Vec2F(0.1f + (float)i * 0.1f, 0.9f - (float)j * 0.1f), 
                                new Vec2F(0.1f, 0.1f)),
                            new ImageStride(80, enemyStrides),
                            new ImageStride (80, alternativeEnemystrideStrides), speed);
                        Enemies.AddEntity(enemy);
                        GalagaBus.GetBus().Subscribe(GameEventType.ControlEvent, enemy);
                    }
                }
            }
        }
    }
}
