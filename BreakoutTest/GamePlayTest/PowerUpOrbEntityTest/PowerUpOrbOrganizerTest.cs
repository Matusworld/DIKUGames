using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

using DIKUArcade.Events;
using DIKUArcade.GUI;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;

using Breakout;
using Breakout.GamePlay.PowerUpOrbEntity;
using Breakout.GamePlay.BlockEntity;

namespace BreakoutTest.GamePlayTest.PowerUpOrbEntityTest {
    public class PowerUpOrbOrganizerTest {
        PowerUpOrbOrganizer PUOrganizer;

        PowerUp Powerup;

        GameEventBus eventBus;

        public PowerUpOrbOrganizerTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void setup() {
            PUOrganizer = new PowerUpOrbOrganizer();

            DynamicShape shape = new DynamicShape(new Vec2F(0.5f, 0.5f), new Vec2F(0.05f, 0.05f));

            Powerup = new PowerUp (
                new DynamicShape(new Vec2F(0.5f, 0.5f), new Vec2F(0.05f, 0.05f)), 
                new Image(Path.Combine(
                    ProjectPath.getPath(), "Breakout", "Assets", "Images", "blue-block.png")),
                new Image(Path.Combine(ProjectPath.getPath(),
                    "Breakout", "Assets", "Images", "blue-block-damaged.png")));

            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.ControlEvent});
            eventBus.Subscribe(GameEventType.ControlEvent, PUOrganizer);
        }

        // Testing that the PowerUpOrbOrganizer start with 0 orbs in the entitycontainer
        [Test]
        public void TestInitialPowerUpOrbOrganizer() {
            Assert.AreEqual(PUOrganizer.Entities.CountEntities(), 0);
        }

        // Testing adding a orb to the PowerUpOrbOrganizer's entitycontainer
        [Test]
        public void TestAddOrb() {
            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "SPAWN_ORB",
                ObjectArg1 = Powerup.Shape.Position
            });

            eventBus.ProcessEvents();

            Assert.AreEqual(PUOrganizer.Entities.CountEntities(), 1);
        }
    }
}