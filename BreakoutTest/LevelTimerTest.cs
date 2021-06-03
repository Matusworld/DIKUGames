using NUnit.Framework;

using DIKUArcade.GUI;
using DIKUArcade.Math;

using Breakout.GamePlay;

namespace BreakoutTest {
    public class LevelTimerTest {
        LevelTimer timer;

        [SetUp]
        public void SetUp() {
            Window.CreateOpenGLContext();

            timer = new LevelTimer(100, new Vec2F(0.33f, -0.26f), 
                new Vec2F(0.3f, 0.3f));
            
        }

        [Test]
        public void TestInitialTimer() {
            Assert.AreEqual(timer.levelTime, 100);
            Assert.AreEqual(timer.timer, 100);
            Assert.AreEqual(timer.staticStartTime, LevelTimer.GetElapsedMilliseconds() / 1000);
        }

        
    }
}