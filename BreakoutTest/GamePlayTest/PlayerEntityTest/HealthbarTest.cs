using System.Collections.Generic;

using NUnit.Framework;

using DIKUArcade.Events;
using DIKUArcade.GUI;

using Breakout.GamePlay.PlayerEntity;

namespace BreakoutTest.GamePlayTest.PlayerEntityTest {
    public class HealthbarTest {
        GameEventBus eventBus;
        Healthbar healthbar;

        uint maxLives = 5;

        uint startLives = 2;

        public HealthbarTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void SetUp() {
            healthbar = new Healthbar(startLives, maxLives);

            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.ControlEvent });
            eventBus.Subscribe(GameEventType.ControlEvent, healthbar);
        }


        // Testing Preconditions
        [Test]
        public void PreconditionTest() {
            Assert.AreEqual(startLives, healthbar.Lives);
            Assert.AreEqual(maxLives, healthbar.MaxLives);
            Assert.AreEqual(maxLives, healthbar.HealthList.Count);
        }

        //Test that a life can be gained
        [Test]
        public void SingeLifeGainTest() {
            uint formerLife = healthbar.Lives;

            eventBus.RegisterEvent(new GameEvent {
                EventType = GameEventType.ControlEvent, 
                StringArg1 = "HEALTH_GAINED"
            });

            eventBus.ProcessEvents();

            Assert.AreEqual(formerLife + 1, healthbar.Lives);
        }

        //Test that a life can be lost
        [Test]
        public void SingeLifeLossTest() {
            uint formerLife = healthbar.Lives;

            eventBus.RegisterEvent(new GameEvent {
                EventType = GameEventType.ControlEvent, 
                StringArg1 = "HEALTH_LOST"
            });

            eventBus.ProcessEvents();

            Assert.AreEqual(formerLife - 1, healthbar.Lives);
        }

        //Test that life cannot be more than 5
        [Test]
        public void AllLifeGainTest() {

            for (int i = 0; i < 5; i++) {
                eventBus.RegisterEvent(new GameEvent {
                    EventType = GameEventType.ControlEvent, 
                    StringArg1 = "HEALTH_GAINED"
                });
            }
            
            eventBus.ProcessEvents();

            Assert.AreEqual(5, healthbar.Lives);
        }

        //Test that life cannot be less than 1
        [Test]
        public void AllLifeLossTest() {

            for (int i = 0; i < 6; i++) {
                eventBus.RegisterEvent(new GameEvent {
                    EventType = GameEventType.ControlEvent, 
                    StringArg1 = "HEALTH_LOST"
                });
            }
            
            eventBus.ProcessEvents();

            Assert.AreEqual(1, healthbar.Lives);
        }
        
        // Testing resetting the health back to the players start lives. 
        [Test]
        public void ResetHealthTest() {
            for (int i = 0; i < 5; i++) {
                eventBus.RegisterEvent(new GameEvent {
                    EventType = GameEventType.ControlEvent, 
                    StringArg1 = "HEALTH_GAINED"
                });
            }
            
            eventBus.ProcessEvents();

            Assert.AreEqual(5, healthbar.Lives);

            healthbar.Reset();

            Assert.AreEqual(startLives, healthbar.Lives);
        }
    }
}