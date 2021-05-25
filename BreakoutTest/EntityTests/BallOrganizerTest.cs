using System.Collections.Generic;
using System;
using System.IO;
using NUnit.Framework;

using DIKUArcade.Events;
using DIKUArcade.GUI;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;

using Breakout.GamePlay.BallEntity;

namespace BreakoutTest {
    public class BallOrganizerTest {
        BallOrganizer ballOrganizer;

        [SetUp]
        public void setup() {
            Window.CreateOpenGLContext();

            ballOrganizer = new BallOrganizer();

        }

        // Testing that the BallOrganizer start with 0 balls in the entitycontainer
        [Test]
        public void TestInitialBallOrganizer() {
            Assert.AreEqual(ballOrganizer.Balls.CountEntities(), 0);
        }
        
        // Testing adding a ball to the BallOrganizer's entitycontainer
        [Test]
        public void TestAddBall() {
            ballOrganizer.AddBall();

            Assert.AreEqual(ballOrganizer.Balls.CountEntities(), 1);
        }

    }
}