using NUnit.Framework;
using System.IO;
using System.Threading;

using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

using Breakout;
using Breakout.GamePlay;
using Breakout.GamePlay.BlockEntity;

namespace BreakoutTest.GamePlayTest {
    public class ScoreTest {
        Score score;

        Block block;
        Hardened hblock;
        PowerUp publock;

        public ScoreTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void Setup() {
            score = new Score(new Vec2F(0.06f, -0.25f), new Vec2F(0.3f, 0.3f));

            block = new Block(new DynamicShape(new Vec2F(0.45f, 0.45f), new Vec2F(0.1f, 0.05f)), 
                                        new Image(Path.Combine( TestProjectPath.getPath(),
                                            "Assets", "Images", "blue-block.png")),
                                        new Image(Path.Combine(TestProjectPath.getPath(),
                                            "Assets", "Images", "blue-block-damaged.png")));

            hblock = new Hardened(new DynamicShape(
                new Vec2F(0.45f, 0.45f), new Vec2F(0.1f, 0.05f)), 
                new Image(Path.Combine(
                    TestProjectPath.getPath(),"Assets", "Images", "blue-block.png")),
                new Image(Path.Combine(TestProjectPath.getPath(),
                    "Assets", "Images", "blue-block-damaged.png")));

            publock = new PowerUp(new DynamicShape(
                new Vec2F(0.45f, 0.45f), new Vec2F(0.1f, 0.05f)), 
                new Image(Path.Combine(TestProjectPath.getPath(),
                    "Assets", "Images", "blue-block.png")),
                    new Image(Path.Combine(TestProjectPath.getPath(),
                    "Assets", "Images", "blue-block-damaged.png")));
        }

        //Precondition
        [Test]
        public void TestInitialScore() {
            Assert.AreEqual(score.ScoreCount, 0);
        }

        //Test that a normal block increases the score correctly
        [Test]
        public void TestNormalBlockIncreaseScore() {

            BreakoutBus.GetBus().RegisterEvent( new GameEvent { 
                EventType = GameEventType.ControlEvent, StringArg1 = "BLOCK_DELETED", From = block});

            BreakoutBus.GetBus().ProcessEvents();
            
            Assert.AreEqual(score.ScoreCount, 1);

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, score);
        }

        //Test that a hardended block increases the score correctly
        [Test]
        public void TestHardenedBlockIncreaseScore() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent { 
                EventType = GameEventType.ControlEvent, StringArg1 = "BLOCK_DELETED", From = hblock});

            BreakoutBus.GetBus().ProcessEventsSequentially();
            
            Assert.AreEqual(score.ScoreCount, 2);

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, score);
        }

        //Test that a normal block increases the score correctly
        [Test]
        public void TestPowerUpBlockIncreaseScore() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent { 
                EventType = GameEventType.ControlEvent, StringArg1 = "BLOCK_DELETED", From = publock});

            BreakoutBus.GetBus().ProcessEventsSequentially();
            
            Assert.AreEqual(score.ScoreCount, 1);

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, score);
        }

        //Test that PowerUp bonus score between 1 and 30 is awarded
        [Test]
        public void PowerUpScoreTest() {
            int min = Score.MinPowerUpPoints;
            int max = Score.MaxPowerUpPoints;

            uint formerScore = score.ScoreCount;

            for (int i = 0; i < 50; i++) {
                BreakoutBus.GetBus().RegisterEvent( new GameEvent { 
                    EventType = GameEventType.ControlEvent, 
                    StringArg1 = "POWERUP_SCORE"});

                Thread.Sleep(10);
                BreakoutBus.GetBus().ProcessEventsSequentially();

                Assert.GreaterOrEqual(score.ScoreCount, formerScore + min);
                Assert.LessOrEqual(score.ScoreCount, formerScore + max);

                formerScore = score.ScoreCount;
            }

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, score);
        }

        // Testing reseting the score. 
        [Test]
        public void TestResetScore() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent { 
                EventType = GameEventType.ControlEvent, StringArg1 = "BLOCK_DELETED", From = block});

            BreakoutBus.GetBus().ProcessEventsSequentially();
            
            Assert.AreEqual(score.ScoreCount, 1);

            score.Reset();

            Assert.AreEqual(score.ScoreCount, 0);

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, score);
        }

        //Test that score cannot go below 0
        [Test]
        public void TestScoreuint() {
            // uint only positive numbers
            Assert.That(score.ScoreCount, Is.InstanceOf<uint>());

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, score);
        }
    }
}