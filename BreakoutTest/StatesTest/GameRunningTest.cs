using NUnit.Framework;

using DIKUArcade.GUI;

using Breakout.States;
namespace BreakoutTest.StatesTest {
    public class GameRunningTest {
        GameRunning game;

        public GameRunningTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void SetUp() {
            game = GameRunning.GetInstance();
        }

        //Test collision checks
    }
}