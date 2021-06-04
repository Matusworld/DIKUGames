using NUnit.Framework;
using System.Collections.Generic;

using DIKUArcade.Events;

using Breakout;
using Breakout.GamePlay;

namespace BreakoutTest.GamePlayTest {
    public class LevelManagerTest {
        LevelManager levelManager;

        LinkedList<string> levelSequence;

        [SetUp]
        public void SetUp() {
            levelSequence = new LinkedList<string> (new List<string> { "level1.txt", "level2.txt", "level3.txt",
                "central-mass.txt", "columns.txt", "wall.txt" });

            levelManager = new LevelManager(levelSequence);
        }

        // Testing that the first level loaded is level 1
        [Test]
        public void TestInitialLevelManager() {
            Assert.AreEqual(levelManager.LevelLoader.Meta.Name, "LEVEL 1");

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, levelManager);
        }

        // Testing going to the next level
        [Test]
        public void TestNextLevel() {
            levelManager.NextLevel();

            Assert.AreEqual(levelManager.LevelLoader.Meta.Name, "LEVEL 2");

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, levelManager);
        }

        // Testing going to the previousLevel
        [Test]
        public void TestPreviousLevel() {
            levelManager.PreviousLevel();
            // first level so do nothing
            Assert.AreEqual(levelManager.LevelLoader.Meta.Name, "LEVEL 1");

            levelManager.NextLevel();
            
            levelManager.NextLevel();

            levelManager.PreviousLevel();

            Assert.AreEqual(levelManager.LevelLoader.Meta.Name, "LEVEL 2");

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, levelManager);
        }

        // Testing resetting the level manager
        [Test]
        public void TestResetLevelManager() {
            levelManager.NextLevel();
            
            levelManager.NextLevel();

            levelManager.NextLevel();
            
            Assert.AreEqual(levelManager.LevelLoader.Meta.Name, "Central Mass");

            levelManager.ResetToFirst();

            Assert.AreEqual(levelManager.LevelLoader.Meta.Name, "LEVEL 1");

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, levelManager);
        }
    }
}