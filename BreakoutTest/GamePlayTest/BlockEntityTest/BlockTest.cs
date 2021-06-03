using NUnit.Framework;
using System.IO;

using Breakout.GamePlay.BlockEntity;

using DIKUArcade.GUI;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

namespace BreakoutTest.GamePlayTest.BlockEntityTest {
    public class BlockTest {
        Block block;

        public BlockTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void Setup() {
            block = new Block(new DynamicShape(new Vec2F(0.45f, 0.45f), new Vec2F(0.1f, 0.05f)), 
                new Image(Path.Combine(
                    TestProjectPath.getPath(),"Assets", "Images", "blue-block.png")),
                new Image(Path.Combine(TestProjectPath.getPath(),
                    "Assets", "Images", "blue-block-damaged.png")));
        }

        /*
        // Testing the block taking damage
        [Test]
        public void TestBlockDamage() {
            int beforedamage = block.HP;

            block.BlockHit();

            Assert.AreEqual(block.HP, beforedamage - 1);
        }

        // Testing that the block "dies"
        [Test]
        public void TestBlockDead() {
            for(int i = 0; i < 15; i++) {
                block.BlockHit();
            }

            Assert.IsTrue(block.IsDeleted());
        } */
    }
}