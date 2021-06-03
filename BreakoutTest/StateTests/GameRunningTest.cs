using System.Collections.Generic;
using NUnit.Framework;

using Breakout.States;

using DIKUArcade.GUI;
using DIKUArcade.Events;

namespace BreakoutTest {
    public class GameRunningTest {
        GameRunning game;
        GameEventBus eventBus;

        public GameRunningTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void SetUp() {
            game = new GameRunning();

            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.ControlEvent });
            
            //eventBus.Subscribe(GameEventType.ControlEvent, game.LevelLoader.BlockOrganizer);
        }

        /*
        [Test]
        public void PreCondition() {
            //Assert.AreEqual(0, game.LevelIndex);
            //Assert.AreNotEqual(0, game.LevelLoader.BlockOrganizer.Entities.CountEntities());
        }


        //Cannot be tested because BreakoutBus is static and thus for some 
        //reason will not process events. All blocks in entitycontainer is automatically
        //subscribed to 
        
        //Not all blocks are destroyed
        [Test]
        public void TestNextLevelNotFinish() {
            //level 1 contains hardened blocks which survives one hit
            /*game.LevelLoader.BlockOrganizer.Entities.Iterate(block => {
                eventBus.RegisterEvent(new GameEvent {
                    EventType = GameEventType.ControlEvent, 
                    StringArg1 = "BlockCollision", To = game.LevelLoader.BlockOrganizer,
                    ObjectArg1 = block
                });
            });
            eventBus.ProcessEvents();
            game.UpdateState();
            Assert.AreEqual(0, game.LevelIndex);
        }

        //All (breakable) blocks are destroyed in one level
        [Test]
        public void TestNextLevel() {
            //damage twice
            /* for(int i = 0; i < 2; i++) {
                game.LevelLoader.BlockOrganizer.Entities.Iterate(block => {
                    eventBus.RegisterEvent(new GameEvent {
                        EventType = GameEventType.ControlEvent, 
                        StringArg1 = "BlockCollision", To = game.LevelLoader.BlockOrganizer,
                        ObjectArg1 = block
                    });
                });
            }
            eventBus.ProcessEvents();
            game.UpdateState();
            Assert.AreEqual(1, game.LevelIndex);
        } */ 
    }
}