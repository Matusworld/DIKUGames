using  DIKUArcade.Entities;
using DIKUArcade.Math;
namespace Galaga.MovementStrategy {
    
    public static class NoMove {
        public static void MoveEnemy(Enemy enemy) {
            enemy.Shape.AsDynamicShape().ChangeDirection(new Vec2F(0.0f,0.0f));
        }
        public static void MoveEnemies(EntityContainer<Enemy> enemies) {
            enemies.Iterate( enemy => {
                MoveEnemy(enemy);
            });     
        }
    }
}