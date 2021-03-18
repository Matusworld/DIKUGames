using  DIKUArcade.Entities;
using DIKUArcade.Math;
using System;
namespace Galaga.MovementStrategy {
    public static class ZigZagDown{
        public static void MoveEnemy(Enemy enemy) {
            float s = -enemy.MOVEMENT_SPEED;
            float p = 0.045f;
            float a = 0.05f;
            float yi = enemy.Shape.Position.Y + s;
            float xi = enemy.startPos.X + a * 
                ((float) Math.Sin((2 * ((float) Math.PI) * (enemy.startPos.Y - yi)) / p));
            
            enemy.Shape.AsDynamicShape().ChangeDirection(new Vec2F(0f,yi));
            enemy.Shape.AsDynamicShape().Move();
        }
        public static void MoveEnemies(EntityContainer<Enemy> enemies) {
            enemies.Iterate( enemy => {
                MoveEnemy(enemy);
            });     
        }
    }
}