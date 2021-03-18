using  DIKUArcade.Entities;
using DIKUArcade.Math;
namespace Galaga.MovementStrategy {
    public static class Down {
        public static void MoveEnemy(Enemy enemy) {
            enemy.Shape.AsDynamicShape().ChangeDirection(new Vec2F(0.0f,-enemy.MOVEMENT_SPEED));
            enemy.Shape.AsDynamicShape().Move();
        }
        public static void MoveEnemies(EntityContainer<Enemy> enemies) {
            enemies.Iterate( enemy => {
                MoveEnemy(enemy);
            });     
        }
    }
}