using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System;

using Breakout;
using Breakout.GamePlay.BlockEntity;
using Breakout.LevelLoading;

using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;


namespace BreakoutTest {
    public class HardenedBlockTest {
        Hardened hblock;

        Block block;

        GameEventBus eventBus;

        [SetUp]
        public void Setup() {
            Window.CreateOpenGLContext();

            block = new Block(new DynamicShape(new Vec2F(0.45f, 0.45f), new Vec2F(0.1f, 0.05f)), 
                                        new Image(Path.Combine( TestProjectPath.getPath(),
                                            "Assets", "Images", "blue-block.png")));

            hblock = new Hardened(new DynamicShape(
                new Vec2F(0.45f, 0.45f), new Vec2F(0.1f, 0.05f)), 
                new Image(Path.Combine(
                    TestProjectPath.getPath(),"Assets", "Images", "blue-block.png")),
                new Image(Path.Combine(TestProjectPath.getPath(),
                    "Assets", "Images", "blue-block-damaged.png")));
            
            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.ControlEvent });
            eventBus.Subscribe(GameEventType.ControlEvent, block);
            eventBus.Subscribe(GameEventType.ControlEvent, hblock);
        }

        [Test]
        public void TestDoubleHPThanNormalBlock() {
            Assert.AreEqual(block.HP * 2, hblock.HP);
        }

        [Test]
        public void TestDamageImage() {
            var beforeimg = hblock.Image;

            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "BlockCollision",
                To = hblock
            });

            eventBus.ProcessEvents();

            var afterimg = hblock.Image;

            Assert.AreNotEqual(beforeimg, afterimg);
        }

    }
}