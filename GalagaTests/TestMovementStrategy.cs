using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using Galaga;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;
using Galaga.MovementStrategy;
using System;
namespace GalagaTests {
    public class TestMovementStrategy {

        Enemy enemy;

        List<Image> enemyStrides;

        List<Image> alternativeEnemystrideStrides;

        float tolerance;
        

        [SetUp]
        public void Setup() {

            DIKUArcade.Window.CreateOpenGLContext();

            enemyStrides = ImageStride.CreateStrides(4, Path.Combine("Assets", 
                "Images", "BlueMonster.png"));

            alternativeEnemystrideStrides = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "RedMonster.png"));

            enemy = new Enemy (new DynamicShape(
                                new Vec2F(0.5f, 0.5f), 
                                new Vec2F(0.1f, 0.1f)),
                            new ImageStride(80, enemyStrides),
                            new ImageStride (80, alternativeEnemystrideStrides), 0.003f);

            tolerance = 0.0001f;
        }

        [Test]
        public void TestNoMove() {
            NoMove.MoveEnemy(enemy);

            float diffX = Math.Abs(enemy.Shape.Position.X - enemy.startPos.X);
            float diffY = Math.Abs(enemy.Shape.Position.Y - enemy.startPos.Y);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }

        [Test]
        public void TestDown() {
            Down.MoveEnemy(enemy);

            float diff = Math.Abs(enemy.Shape.Position.Y
                - (enemy.startPos.Y - enemy.MOVEMENT_SPEED));

            Assert.LessOrEqual(diff, tolerance);
        }

        [Test]
        public void TestZigZagDown() {
            ZigZagDown.MoveEnemy(enemy);

            Assert.AreEqual(enemy.Shape.Position.Y, 
                enemy.startPos.Y - enemy.MOVEMENT_SPEED);
            
            // Udregn x position
            float s = -enemy.MOVEMENT_SPEED;
            float p = 0.045f;
            float a = 0.05f;
            float yi = enemy.startPos.Y + s;
            float xi = enemy.startPos.X + a * 
                ((float) Math.Sin((2 * ((float) Math.PI) * (enemy.startPos.Y - yi)) / p));

            float diff = Math.Abs(enemy.Shape.Position.X - xi);

            Assert.LessOrEqual(diff, tolerance);
        }
    }
}