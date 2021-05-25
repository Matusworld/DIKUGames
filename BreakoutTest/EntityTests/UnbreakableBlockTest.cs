using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System;

using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

using Breakout.LevelLoading;
using Breakout;
using Breakout.GamePlay.BlockEntity;

namespace BreakoutTest {
    public class UnbreakableBlockTest {
        Unbreakable uBlock;

        GameEventBus eventBus;

        [SetUp]
        public void Setup() {
            Window.CreateOpenGLContext();

            uBlock = new Unbreakable(new DynamicShape(
                new Vec2F(0.45f, 0.45f), new Vec2F(0.1f, 0.05f)), 
                new Image(Path.Combine( TestProjectPath.getPath(),
                    "Assets", "Images", "blue-block.png")));
            
            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.ControlEvent });
            eventBus.Subscribe(GameEventType.ControlEvent, uBlock);
        }

        [Test]
        public void UnbreakableTest() {
            for(int i = 0; i < 100; i++) {
                eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "BlockCollision",
                To = uBlock
                });

                eventBus.ProcessEvents();
            }

            Assert.AreEqual(uBlock.HP, 1);

            Assert.AreEqual(uBlock.Alive, true);
        }
    }
}