using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

using Breakout.GamePlay;
using Breakout.GamePlay.BlockEntity;

namespace BreakoutTest {
    public class ScoreTest {
        Score score;
        GameEventBus eventBus;

        Block block;

        Hardened hblock;

        PowerUp publock;

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

            publock = new PowerUp(new DynamicShape(
                new Vec2F(0.45f, 0.45f), new Vec2F(0.1f, 0.05f)), 
                new Image(Path.Combine(TestProjectPath.getPath(),
                    "Assets", "Images", "blue-block.png")));

            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.ControlEvent });
            eventBus.Subscribe(GameEventType.ControlEvent, score);
            eventBus.Subscribe(GameEventType.ControlEvent, block);
        }

        //Precondition
        [Test]
        public void TestInitialScore() {
            Assert.AreEqual(score.ScoreCount, 0);
        }

        //Test that a normal block increases the score correctly
        [Test]
        public void TestNormalBlockIncreaseScore() {

            eventBus.RegisterEvent( new GameEvent { 
                EventType = GameEventType.ControlEvent, StringArg1 = "ADD_SCORE", From = block});

            eventBus.ProcessEvents();
            
            Assert.AreEqual(score.ScoreCount, 1);
        }

        //Test that a hardended block increases the score correctly
        [Test]
        public void TestHardenedBlockIncreaseScore() {
            eventBus.RegisterEvent( new GameEvent { 
                EventType = GameEventType.ControlEvent, StringArg1 = "ADD_SCORE", From = hblock});

            eventBus.ProcessEvents();
            
            Assert.AreEqual(score.ScoreCount, 2);
        }

        //Test that a normal block increases the score correctly
        [Test]
        public void TestPowerUpBlockIncreaseScore() {
            eventBus.RegisterEvent( new GameEvent { 
                EventType = GameEventType.ControlEvent, StringArg1 = "ADD_SCORE", From = publock});

            eventBus.ProcessEvents();
            
            Assert.AreEqual(score.ScoreCount, 1);
        }

        //Test that PowerUp bonus score between 1 and 30 is awarded
        [Test]
        public void PowerUpScoreTest() {
            int min = Score.MinPowerUpPoints;
            int max = Score.MaxPowerUpPoints;

            uint formerScore = score.ScoreCount;

            for (int i = 0; i < 50; i++) {
                eventBus.RegisterEvent( new GameEvent { 
                    EventType = GameEventType.ControlEvent, 
                    StringArg1 = "PowerUpScore"});

                Thread.Sleep(10);
                eventBus.ProcessEvents();

                Assert.GreaterOrEqual(score.ScoreCount, formerScore + min);
                Assert.LessOrEqual(score.ScoreCount, formerScore + max);

                formerScore = score.ScoreCount;
            }
            
        }

        //Test that score cannot go below 0
        [Test]
        public void TestScoreuint() {
            // uint only positive numbers
            Assert.That(score.ScoreCount, Is.InstanceOf<uint>());
        }
    }
}