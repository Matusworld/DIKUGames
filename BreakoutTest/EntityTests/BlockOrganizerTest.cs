using NUnit.Framework;
using System.IO;

using DIKUArcade.GUI;
using DIKUArcade.Math;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;

using Breakout.GamePlay.BlockEntity;

namespace BreakoutTest {
    public class BlockOrganizerTest {
        BlockOrganizer blockOrganizer;

        public BlockOrganizerTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void SetUp() {
            blockOrganizer = new BlockOrganizer();
        }

        // Testing that the precondition holds
        [Test]
        public void TestInitialBlockOrganizer() {
            Assert.AreEqual(blockOrganizer.Entities.CountEntities(), 0);

            Assert.AreEqual(blockOrganizer.NumberOfUnbreakables, 0);
        }

        // Testing Adding blocks to the organizer, then reseting the organizer. 
        [Test]
        public void TestResetOrganizer() {
            Block block = new Block(new DynamicShape(
                new Vec2F(0.45f, 0.45f), new Vec2F(0.1f, 0.05f)), 
                new Image(Path.Combine(
                    TestProjectPath.getPath(),"Assets", "Images", "blue-block.png")),
                new Image(Path.Combine(TestProjectPath.getPath(),
                    "Assets", "Images", "blue-block-damaged.png")));

            Hardened hblock = new Hardened(new DynamicShape(
                new Vec2F(0.45f, 0.45f), new Vec2F(0.1f, 0.05f)), 
                new Image(Path.Combine(
                    TestProjectPath.getPath(),"Assets", "Images", "blue-block.png")),
                new Image(Path.Combine(TestProjectPath.getPath(),
                    "Assets", "Images", "blue-block-damaged.png")));
            
            Unbreakable uBlock = new Unbreakable(new DynamicShape(
                new Vec2F(0.45f, 0.45f), new Vec2F(0.1f, 0.05f)), 
                new Image(Path.Combine( TestProjectPath.getPath(),
                    "Assets", "Images", "blue-block.png")),
                new Image(Path.Combine( TestProjectPath.getPath(),
                    "Assets", "Images", "blue-block-damaged.png")));

            blockOrganizer.AddEntity(block);
            blockOrganizer.AddEntity(hblock);
            blockOrganizer.AddEntity(uBlock);

            blockOrganizer.NumberOfUnbreakables++;

            Assert.AreEqual(blockOrganizer.Entities.CountEntities(), 3);

            Assert.AreEqual(blockOrganizer.NumberOfUnbreakables, 1);

            blockOrganizer.ResetOrganizer();

            Assert.AreEqual(blockOrganizer.Entities.CountEntities(), 0);

            Assert.AreEqual(blockOrganizer.NumberOfUnbreakables, 0);

        }
    }
}