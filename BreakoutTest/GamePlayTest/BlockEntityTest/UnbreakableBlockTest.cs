using NUnit.Framework;
using System.IO;

using DIKUArcade.GUI;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

using Breakout.GamePlay.BlockEntity;

namespace BreakoutTest.GamePlayTest.BlockEntityTest {
    public class UnbreakableBlockTest {
        Unbreakable uBlock;

        public UnbreakableBlockTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void Setup() {
            uBlock = new Unbreakable(new DynamicShape(
                new Vec2F(0.45f, 0.45f), new Vec2F(0.1f, 0.05f)), 
                new Image(Path.Combine( TestProjectPath.getPath(),
                    "Assets", "Images", "blue-block.png")),
                new Image(Path.Combine( TestProjectPath.getPath(),
                    "Assets", "Images", "blue-block-damaged.png")));
        }

        // Testing the unbreakable block cannot be destroyed 
        [Test]
        public void UnbreakableTest() {
            for(int i = 0; i < 100; i++) {
                uBlock.BlockHit();
            }

            Assert.AreEqual(uBlock.HP, 1);

            Assert.AreEqual(uBlock.Alive, true);
        }
    }
}