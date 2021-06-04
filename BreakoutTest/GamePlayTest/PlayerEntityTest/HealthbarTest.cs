using NUnit.Framework;

using DIKUArcade.Events;
using DIKUArcade.GUI;

using Breakout;
using Breakout.GamePlay.PlayerEntity;

namespace BreakoutTest.GamePlayTest.PlayerEntityTest {
    public class HealthbarTest {
        Healthbar healthbar;

        uint maxLives = 5;

        uint startLives = 2;

        public HealthbarTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void SetUp() {
            healthbar = new Healthbar(startLives, maxLives);
        }


        // Testing Preconditions
        [Test]
        public void PreconditionTest() {
            Assert.AreEqual(startLives, healthbar.Lives);
            Assert.AreEqual(maxLives, healthbar.MaxLives);
            Assert.AreEqual(maxLives, healthbar.HealthList.Count);

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, healthbar);
        }

        //Test that a life can be gained
        [Test]
        public void SingeLifeGainTest() {
            uint formerLife = healthbar.Lives;

            BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                EventType = GameEventType.ControlEvent, 
                StringArg1 = "HEALTH_GAINED"
            });

            BreakoutBus.GetBus().ProcessEventsSequentially();

            Assert.AreEqual(formerLife + 1, healthbar.Lives);

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, healthbar);
        }

        //Test that a life can be lost
        [Test]
        public void SingeLifeLossTest() {
            uint formerLife = healthbar.Lives;

            BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                EventType = GameEventType.ControlEvent, 
                StringArg1 = "HEALTH_LOST"
            });

            BreakoutBus.GetBus().ProcessEventsSequentially();

            Assert.AreEqual(formerLife - 1, healthbar.Lives);

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, healthbar);
        }

        //Test that life cannot be more than maxLives
        [Test]
        public void AllLifeGainTest() {

            for (int i = 0; i < maxLives; i++) {
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.ControlEvent, 
                    StringArg1 = "HEALTH_GAINED"
                });
            }
            
            BreakoutBus.GetBus().ProcessEventsSequentially();

            Assert.AreEqual(maxLives, healthbar.Lives);

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, healthbar);
        }

        //Test that life cannot be less than 1
        [Test]
        public void AllLifeLossTest() {

            for (int i = 0; i < maxLives+1; i++) {
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.ControlEvent, 
                    StringArg1 = "HEALTH_LOST"
                });
            }
            
            BreakoutBus.GetBus().ProcessEventsSequentially();

            Assert.AreEqual(1, healthbar.Lives);

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, healthbar);
        }
        
        // Testing resetting the health back to the players start lives. 
        [Test]
        public void ResetHealthTest() {
            for (int i = 0; i < maxLives; i++) {
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.ControlEvent, 
                    StringArg1 = "HEALTH_GAINED"
                });
            }
            
            BreakoutBus.GetBus().ProcessEventsSequentially();

            Assert.AreEqual(maxLives, healthbar.Lives);

            healthbar.Reset();

            Assert.AreEqual(startLives, healthbar.Lives);

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, healthbar);
        }
    }
}