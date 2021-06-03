using System.IO;
using System;
using NUnit.Framework;

using DIKUArcade.GUI;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

using Breakout.GamePlay.PowerUpOrbEntity;
namespace BreakoutTest {
    public class PowerUpOrbTest {
        PowerUpOrb Orb;
        float tolerance;

        public PowerUpOrbTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void setup() {
            PowerUpTypes draw = PowerUpRandom.RandomType();
            
            IBaseImage image;
            image = new Image(Path.Combine(TestProjectPath.getPath(), 
                        "Assets", "Images", "LifePickUp.png"));
            Vec2F extent = new Vec2F(0.05f, 0.05f);
            DynamicShape shape = new DynamicShape(new Vec2F(0.05f, 0.05f), extent);
            Orb = new ExtraBallOrb(shape, image);
                                
            tolerance = 0.0001f;
            
        }

        // Testing that the orb can move downwards to the player
        [Test]
        public void MoveTest() {
            float oldYPosition = 0.05f;
            Orb.Move();
            Assert.IsTrue(0.005f + tolerance >= Math.Abs(Orb.Shape.Position.Y-oldYPosition));
        }

        // Testing that the orb has a Image
        [Test]
        public void PictureTest() {
            Assert.IsTrue(Orb.Image != null);
        }

        // Testing that when the orb is at the bottom of the map
        [Test]
        public void TestLOBoundaryCheckers() {
            Orb.Shape.Position.Y = -0.05f;
            Assert.IsTrue(Orb.LowerBoundaryCheck());
        }
    }
}