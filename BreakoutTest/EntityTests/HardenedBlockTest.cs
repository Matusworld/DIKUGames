using NUnit.Framework;
using System.IO;

using Breakout.GamePlay.BlockEntity;

using DIKUArcade.GUI;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;


namespace BreakoutTest {
    public class HardenedBlockTest {
        Hardened hblock;

        Block block;

        public HardenedBlockTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void Setup() {
            block = new Block(new DynamicShape(new Vec2F(0.45f, 0.45f), new Vec2F(0.1f, 0.05f)), 
                new Image(Path.Combine(
                    TestProjectPath.getPath(),"Assets", "Images", "blue-block.png")),
                new Image(Path.Combine(TestProjectPath.getPath(),
                    "Assets", "Images", "blue-block-damaged.png")));

            hblock = new Hardened(new DynamicShape(
                new Vec2F(0.45f, 0.45f), new Vec2F(0.1f, 0.05f)), 
                new Image(Path.Combine(
                    TestProjectPath.getPath(),"Assets", "Images", "blue-block.png")),
                new Image(Path.Combine(TestProjectPath.getPath(),
                    "Assets", "Images", "blue-block-damaged.png")));
        }

        // Testing that the Hardened block has twice the amount of HP than Block
        [Test]
        public void TestDoubleHPThanNormalBlock() {
            Assert.AreEqual(block.HP * 2, hblock.HP);
        }

        // Testing when damaging the Hardened block it changes to the damaged image
        [Test]
        public void TestDamageImage() {
            var beforeimg = hblock.Image;

            hblock.BlockHit();

            var afterimg = hblock.Image;

            Assert.AreNotEqual(beforeimg, afterimg);
        }
    }
}