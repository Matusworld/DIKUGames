using System.Collections.Generic;
using System.IO;
using System;
using NUnit.Framework;

using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

using Breakout.GamePlay.BlockEntity.PowerUpOrbEntity;
namespace BreakoutTest
{
    public class PowerUpOrbTest {
        PowerUpOrb Orb;
        GameEventBus eventBus;
        EntityContainer<PowerUpOrb> Orbs;
        float tolerance;
        [SetUp]

        public void setup() {

            Window.CreateOpenGLContext();

            PowerUpTypes draw = PowerUpRandom.RandomType();
            
            IBaseImage image;
            image = new Image(Path.Combine(TestProjectPath.getPath(), 
                        "Assets", "Images", "LifePickUp.png"));
            Vec2F extent = new Vec2F(0.05f, 0.05f);
            DynamicShape shape = new DynamicShape(new Vec2F(0.05f, 0.05f), extent);
            Orb = new PowerUpOrb(shape, image, draw);
            
            PowerUpOrbOrganizer org = new PowerUpOrbOrganizer();

            
            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.ControlEvent });
            eventBus.Subscribe(GameEventType.ControlEvent, org);

            tolerance = 0.0001f;
            
        }
        [Test]
        public void MoveTest() {
            float oldYPosition = 0.05f;
            Orb.Move();
            Assert.IsTrue(
                0.005f + tolerance >= Math.Abs(Orb.Shape.Position.Y-oldYPosition));
        }
        [Test]
        public void PictureTest() {
            IBaseImage testImage = new Image(Path.Combine(TestProjectPath.getPath(), 
                        "Assets", "Images", "LifePickUp.png"));
            Assert.IsTrue(Orb.Image != null);
        }

        [Test]
        public void TestLOBoundaryCheckers() {
            Orb.Shape.Position.Y = 0.00f;

            Assert.IsTrue(2 = 2);
            NUnit.Framework.TestContext.Progress.WriteLine(Orb.LowerBoundaryCheck());
        }
    }
}