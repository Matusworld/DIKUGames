using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System;
using Breakout;
using Breakout.Blocks;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;
using Breakout.LevelLoading;

namespace BreakoutTest {
    public class TestScore {
        Score score;
        GameEventBus eventBus;

        Block block;

        Hardened hblock;

        [SetUp]
        public void Setup() {
            Window.CreateOpenGLContext();
            score = new Score(new Vec2F(0.06f, -0.25f), new Vec2F(0.3f, 0.3f));

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
            eventBus.Subscribe(GameEventType.ControlEvent, score);
            eventBus.Subscribe(GameEventType.ControlEvent, block);
        }

        [Test]
        public void TestInitialScore() {
            Assert.AreEqual(score.ScoreCount, 0);
        }

        [Test]
        public void TestNormalBlockIncreaseScore() {

            eventBus.RegisterEvent( new GameEvent { 
                EventType = GameEventType.ControlEvent, StringArg1 = "ADD_SCORE", From = block});

            eventBus.ProcessEvents();
            
            Assert.AreEqual(score.ScoreCount, 1);
        }
        [Test]
        public void TestHardenedBlockIncreaseScore() {
            eventBus.RegisterEvent( new GameEvent { 
                EventType = GameEventType.ControlEvent, StringArg1 = "ADD_SCORE", From = hblock});

            eventBus.ProcessEvents();
            
            Assert.AreEqual(score.ScoreCount, 2);
        }

        [Test]
        public void TestScoreuint() {
            // uint only positive numbers
            Assert.That(score.ScoreCount, Is.InstanceOf<uint>());
        }
    }
}