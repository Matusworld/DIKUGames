using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System;

using Breakout;
using Breakout.BlockEntity;

using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;
using Breakout.LevelLoading;

namespace BreakoutTest
{
    public class BlockTest {
        Block block;
        GameEventBus eventBus;

        [SetUp]
        public void Setup() {
            Window.CreateOpenGLContext();

            block = new Block(new DynamicShape(new Vec2F(0.45f, 0.45f), new Vec2F(0.1f, 0.05f)), 
                                        new Image(Path.Combine( TestProjectPath.getPath(),
                                            "Assets", "Images", "blue-block.png")));
            
            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.ControlEvent });
            eventBus.Subscribe(GameEventType.ControlEvent, block);
        }

        [Test]
        public void TestBlockDamage() {
            int beforedamage = block.HP;

            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "BlockCollision",
                To = block
            });

            eventBus.ProcessEvents();

            Assert.AreEqual(block.HP, beforedamage - 1);
        }

        [Test]
        public void TestBlockDead() {
            for(int i = 0; i < 15; i++) {
                eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "BlockCollision",
                To = block
                });

                eventBus.ProcessEvents();
            }

            Assert.IsFalse(block.Alive);
        }
    }
}