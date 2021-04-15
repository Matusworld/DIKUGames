using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using Galaga;
using DIKUArcade.Graphics;
using Galaga.Squadron;
using System;

namespace GalagaTests {
    public class TestSquadron {
        private ISquadron squadron;
        List<Image> enemyStrides;

        List<Image> alternativeEnemystrideStrides;

        float tolerance;

        [SetUp]
        public void Setup() {


            enemyStrides = ImageStride.CreateStrides(4, Path.Combine("Assets", 
                "Images", "BlueMonster.png"));

            alternativeEnemystrideStrides = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "RedMonster.png"));

            tolerance = 0.0001f;
        }

        [Test]
        public void TestSquadronHoriLine() {

            squadron = new SquadronHoriLine();

            squadron.CreateEnemies(enemyStrides, alternativeEnemystrideStrides, 0.003f);

            int i = 0;

            foreach (Enemy enemy in squadron.Enemies) {
                float diffX = Math.Abs(enemy.Shape.Position.X - (0.1f + (float)i * 0.1f));
                float diffY = Math.Abs(enemy.Shape.Position.Y - 0.9f);

                Assert.LessOrEqual(diffX, tolerance);
                Assert.LessOrEqual(diffY, tolerance);

                i++;
            }
        }

        [Test]
        public void TestSquadronVertiLines() {
            squadron = new SquadronVertiLines();

            squadron.CreateEnemies(enemyStrides, alternativeEnemystrideStrides, 0.003f);

            int i = 0;

            int j = 0;

            foreach (Enemy enemy in squadron.Enemies) {
                float diffX = Math.Abs(enemy.Shape.Position.X - (0.1f + (float)i * 0.35f));
                float diffY = Math.Abs(enemy.Shape.Position.Y - (0.9f - (float)j * 0.1f));

                Assert.LessOrEqual(diffX, tolerance);
                Assert.LessOrEqual(diffY, tolerance);

                j++;
                if (j > 2) {
                    i++;
                    j = 0;
                }
            }
        }

        [Test]
        public void TestSquadronCross() {
            squadron = new SquadronCross();

            squadron.CreateEnemies(enemyStrides, alternativeEnemystrideStrides, 0.003f);

            int i = 0;

            int j = 0;

            foreach (Enemy enemy in squadron.Enemies) {

                if ((i+j) % 2 == 0) {
                    float diffX = Math.Abs(enemy.Shape.Position.X - (0.1f + (float)i * 0.1f));
                    float diffY = Math.Abs(enemy.Shape.Position.Y - (0.9f - (float)j * 0.1f));

                    Assert.LessOrEqual(diffX, tolerance);
                    Assert.LessOrEqual(diffY, tolerance);
                }

                j += 2;
                if (j > 2) {
                    i++;
                    j = j - 3;
                }
            }
        }
    }
}