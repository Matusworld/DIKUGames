using NUnit.Framework;
using System.IO;

using Breakout.GamePlay.BlockEntity;

using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;


namespace BreakoutTest {
    public class HardenedBlockTest {
        Hardened hblock;

        Block block;

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
        }

        [Test]
        public void TestDoubleHPThanNormalBlock() {
            Assert.AreEqual(block.HP * 2, hblock.HP);
        }

        [Test]
        public void TestDamageImage() {
            var beforeimg = hblock.Image;

            hblock.ReceiveEvent( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "BlockCollision",
            });

            //eventBus.ProcessEvents();

            var afterimg = hblock.Image;

            Assert.AreNotEqual(beforeimg, afterimg);
        }

    }
}