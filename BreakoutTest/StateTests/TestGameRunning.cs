// Test that we can increment the current level 
// when all blocks are dead. 
// Also when no more levels are available, 
// it should return the game state to main menu

using System.Collections.Generic;
using System.IO;
using System;
using NUnit.Framework;
using Breakout;
using Breakout.States;
using Breakout.States.Buttons;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Input;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

namespace BreakoutTest {
    public class GameRunningTest {
        GameRunning game;
        GameEventBus eventBus;

        [SetUp]
        public void SetUp() {
            Window.CreateOpenGLContext();
            game = new GameRunning();

            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.ControlEvent });
            
            game.LevelLoader.Blocks.Iterate(block => {
                eventBus.Subscribe(GameEventType.ControlEvent, block);
            });

        }

        //First level is loaded
        [Test]
        public void PreCondition() {
            Assert.AreEqual(0, game.LevelIndex);
            Assert.AreNotEqual(0, game.LevelLoader.Blocks.CountEntities());
        }


        //Cannot be tested because BreakoutBus is static and thus for some 
        //reason will not process events. All blocks in entitycontainer is automatically
        //subscribed to 
        
        //Not all blocks are destroyed
        [Test]
        public void TestNextLevelNotFinish() {
            //level 1 contains hardened blocks which survives one hit
            game.LevelLoader.Blocks.Iterate(block => {
                eventBus.RegisterEvent(new GameEvent {
                    EventType = GameEventType.ControlEvent, 
                    StringArg1 = "BlockCollision", To = block
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
            for(int i = 0; i < 2; i++) {
                game.LevelLoader.Blocks.Iterate(block => {
                    eventBus.RegisterEvent(new GameEvent {
                        EventType = GameEventType.ControlEvent, 
                        StringArg1 = "BlockCollision", To = block
                    });
                });
            }
            eventBus.ProcessEvents();
            game.UpdateState();
            Assert.AreEqual(1, game.LevelIndex);


            //Test ball in the middle start
        } 
    }
}