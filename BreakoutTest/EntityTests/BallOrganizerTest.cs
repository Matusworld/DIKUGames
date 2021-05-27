using NUnit.Framework;

using DIKUArcade.GUI;

using Breakout.GamePlay.BallEntity;

namespace BreakoutTest {
    public class BallOrganizerTest {
        BallOrganizer ballOrganizer;

        [SetUp]
        public void setup() {
            Window.CreateOpenGLContext();

            ballOrganizer = new BallOrganizer();

        }

        // Testing that the BallOrganizer start with 0 balls in the entitycontainer
        [Test]
        public void TestInitialBallOrganizer() {
            Assert.AreEqual(ballOrganizer.Entities.CountEntities(), 0);
        }
        
        // Testing adding a ball to the BallOrganizer's entitycontainer
        [Test]
        public void TestAddBall() {
            ballOrganizer.AddEntity(ballOrganizer.GenerateBallRandomDir());

            Assert.AreEqual(ballOrganizer.Entities.CountEntities(), 1);
        }

    }
}