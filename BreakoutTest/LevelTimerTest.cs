using NUnit.Framework;

using DIKUArcade.GUI;
using DIKUArcade.Math;

using Breakout.GamePlay;

namespace BreakoutTest {
    public class LevelTimerTest {
        LevelTimer timer;

        public LevelTimerTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void SetUp() {
            timer = new LevelTimer(100, new Vec2F(0.33f, -0.26f), 
                new Vec2F(0.3f, 0.3f));
            
        }

        // Testing precondition
        [Test]
        public void TestInitialTimer() {
            Assert.AreEqual(timer.levelTime, 100);
            Assert.AreEqual(timer.timer, 100);
            Assert.AreEqual(timer.staticStartTime, LevelTimer.GetElapsedMilliseconds() / 1000);
        }

        // Testing setting a new level timer
        [Test]
        public void TestSetNewLevelTime() {
            int newTime = 150;

            timer.SetNewLevelTime(newTime);

            Assert.AreEqual(timer.timer, newTime);
            Assert.AreEqual(timer.levelTime, newTime);
            Assert.AreEqual(timer.staticStartTime, LevelTimer.GetElapsedMilliseconds() / 1000);

        }

        // testing that the timer can be updated
        [Test]
        public void TestUpdateTimer() {
            int beforeleveltime = timer.levelTime;
            int beforestaticStartTime = timer.staticStartTime;

            timer.UpdateTimer();

            int newtime = (beforeleveltime + beforestaticStartTime)
                - (int) LevelTimer.GetElapsedMilliseconds() / 1000;

            Assert.AreEqual(timer.timer, newtime);
        }

        // Testing when the time runs out the method returns true
        [Test]
        public void TestTimeRunOut() {
            Assert.IsFalse(timer.TimeRunOut());

            timer.SetNewLevelTime(0);

            Assert.IsTrue(timer.TimeRunOut());
        }

    }
}