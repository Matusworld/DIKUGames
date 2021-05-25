using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

using DIKUArcade.Events;
using DIKUArcade.GUI;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;

using Breakout.GamePlay.BlockEntity.PowerUpOrbEntity;

namespace BreakoutTest {
    public class PowerUpOrbOrganizerTest {
        PowerUpOrbOrganizer PUOrganizer;

        PowerUpOrb orb;

        GameEventBus eventBus;

        [SetUp]
        public void setup() {
            Window.CreateOpenGLContext();

            PUOrganizer = new PowerUpOrbOrganizer();

            DynamicShape shape = new DynamicShape(new Vec2F(0.5f, 0.5f), new Vec2F(0.05f, 0.05f));

            Image image = new Image(Path.Combine(TestProjectPath.getPath(),  
                "Assets", "Images", "LifePickUp.png"));

            PowerUpTypes type = PowerUpTypes.ExtraLife;

            orb = new PowerUpOrb (shape, image, type);

            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.ControlEvent});
            eventBus.Subscribe(GameEventType.ControlEvent, PUOrganizer);
        }

        // Testing that the PowerUpOrbOrganizer start with 0 orbs in the entitycontainer
        [Test]
        public void TestInitialPowerUpOrbOrganizer() {
            Assert.AreEqual(PUOrganizer.Orbs.CountEntities(), 0);
        }

        // Testing adding a orb to the PowerUpOrbOrganizer's entitycontainer
        [Test]
        public void TestAddOrb() {
            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "ADD_ORB",
                From = orb
            });

            eventBus.ProcessEvents();

            Assert.AreEqual(PUOrganizer.Orbs.CountEntities(), 1);
        }
    }
}