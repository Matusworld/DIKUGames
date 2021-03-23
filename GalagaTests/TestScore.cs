using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using Galaga;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

namespace GalagaTests {
    public class TestScore {
        private Score score;

        [SetUp]
        public void Setup() {
            DIKUArcade.Window.CreateOpenGLContext();

            score = new Score(new Vec2F (0.485f, -0.2f), new Vec2F (0.3f, 0.3f));
        }

        [Test]
        public void TestStart() {
            
            Assert.AreEqual(score.ScoreCount, 0);
        }

        [Test]
        public void TestAddScore() {

            score.AddPoint();
            Assert.AreEqual(score.ScoreCount, 1);
        }
    }
}