using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using Galaga;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;
using Galaga.Squadron;

namespace GalagaTests {
    public class TestSquadron {
        private ISquadron squadron;
        List<Image> enemyStrides;

        List<Image> alternativeEnemystrideStrides;

        [SetUp]
        public void Setup() {


            enemyStrides = ImageStride.CreateStrides(4, Path.Combine("Assets", 
                "Images", "BlueMonster.png"));

            alternativeEnemystrideStrides = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "RedMonster.png"));
        }

        [Test]
        public void TestSquadronHoriLine() {

            squadron = new SquadronHoriLine();

            squadron.CreateEnemies(enemyStrides, alternativeEnemystrideStrides, 0.003f);

            int i = 0;

            foreach (Enemy enemy in squadron.Enemies) {
                Assert.AreEqual(enemy.Shape.Position.X, 0.1f + (float)i * 0.1f);
                Assert.AreEqual(enemy.Shape.Position.Y, 0.9f);
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
                Assert.AreEqual(enemy.Shape.Position.X, 0.1f + (float)i * 0.35f);
                Assert.AreEqual(enemy.Shape.Position.Y, 0.9f - (float)j * 0.1f);
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
                    Assert.AreEqual(enemy.Shape.Position.X, 0.1f + (float)i * 0.1f);
                    Assert.AreEqual(enemy.Shape.Position.Y, 0.9f - (float)j * 0.1f);
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