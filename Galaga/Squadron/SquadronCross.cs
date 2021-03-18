using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;

namespace Galaga.Squadron {
    public class SquadronCross : ISquadron {
        public EntityContainer<Enemy> Enemies { get; private set; }

        public int MaxEnemies { get; } = 5;

        public void CreateEnemies(List<Image> enemyStrides, 
            List<Image> alternativeEnemystrideStrides) {
                Enemies = new EntityContainer<Enemy>(MaxEnemies);
            
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 3; j++) {
                    if ((i+j) % 2 == 0) {
                        Enemy enemy = new Enemy(
                            new DynamicShape(new Vec2F(0.0f + (float)i * 0.1f, 0.9f - (float)j * 0.1f), 
                                new Vec2F(0.1f, 0.1f)),
                            new ImageStride(80, enemyStrides),
                            new ImageStride (80, alternativeEnemystrideStrides));
                        Enemies.AddEntity(enemy);
                        Game.eventBus.Subscribe(GameEventType.EnemyEvent, enemy);
                    }
                }
            }
        }
    }
}
