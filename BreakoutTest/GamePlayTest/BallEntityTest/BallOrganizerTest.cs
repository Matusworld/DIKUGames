using NUnit.Framework;
using System.Collections.Generic;

using DIKUArcade.GUI;
using DIKUArcade.Events;

using Breakout;
using Breakout.GamePlay.BallEntity;

namespace BreakoutTest.GamePlayTest.BallEntityTest {
    public class BallOrganizerTest {
        BallOrganizer ballOrganizer;

        public BallOrganizerTest() {
            //NUnit executes test classes in alphebitical order
            //the Singleton BreakoutBus will now be initializing in whole test context for
            //other tests to use. 
            BreakoutBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.WindowEvent, GameEventType.ControlEvent,
                GameEventType.GameStateEvent } );
            
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void setup() {
            ballOrganizer = new BallOrganizer();

        }

        // Testing that the BallOrganizer start with 0 balls in the entitycontainer
        [Test]
        public void TestInitialBallOrganizer() {
            Assert.AreEqual(ballOrganizer.Entities.CountEntities(), 0);
        }
        
        // Testing that the BallOrganizer can generate a ball with random dir.
        [Test]
        public void TestGenrateBallRandomDir() {
            
            bool check = true;

            for (int i = 0; i < 100; i++) {
                Ball ball = ballOrganizer.GenerateBallRandomDir();

                float radtoDeg = ball.GetTheta() * 180 / (float) System.Math.PI;
                if (!(radtoDeg >= 44.0f && radtoDeg <= 136f)) {
                    check = false;
                }
            }

            Assert.IsTrue(check);
        }


        // Testing adding a ball to the BallOrganizer's entitycontainer
        [Test]
        public void TestAddBall() {
            ballOrganizer.AddEntity(ballOrganizer.GenerateBallRandomDir());

            Assert.AreEqual(ballOrganizer.Entities.CountEntities(), 1);
        }

        // Testing reseting the Organizer
        [Test]
        public void TestResetOrganizer() {
            Assert.AreEqual(ballOrganizer.Entities.CountEntities(), 0);

            ballOrganizer.AddEntity(ballOrganizer.GenerateBallRandomDir());

            ballOrganizer.AddEntity(ballOrganizer.GenerateBallRandomDir());

            Assert.AreEqual(ballOrganizer.Entities.CountEntities(), 2);

            ballOrganizer.ResetOrganizer();

            Assert.AreEqual(ballOrganizer.Entities.CountEntities(), 1);
        }

    }
}