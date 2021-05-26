using NUnit.Framework;
using System.IO;

using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

using Breakout.GamePlay.BlockEntity;

namespace BreakoutTest {
    public class UnbreakableBlockTest {
        Unbreakable uBlock;

        [SetUp]
        public void Setup() {
            Window.CreateOpenGLContext();

            uBlock = new Unbreakable(new DynamicShape(
                new Vec2F(0.45f, 0.45f), new Vec2F(0.1f, 0.05f)), 
                new Image(Path.Combine( TestProjectPath.getPath(),
                    "Assets", "Images", "blue-block.png")));
        }

        [Test]
        public void UnbreakableTest() {
            for(int i = 0; i < 100; i++) {
                uBlock.ReceiveEvent( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "BlockCollision",
                });

                //eventBus.ProcessEvents();
            }

            Assert.AreEqual(uBlock.HP, 1);

            Assert.AreEqual(uBlock.Alive, true);
        }
    }
}