using System.Collections.Generic;
using System.IO;
using System;
using NUnit.Framework;
using Breakout;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

namespace BreakoutTest
{
    public class BallTest{
        Ball ball;
        GameEventBus eventBus;
        float tolerance;

        [SetUp]
        
        public void setup() {
            Window.CreateOpenGLContext();
            ball = new Ball(
                new DynamicShape (new Vec2F(0.45f, 0.5f), new Vec2F(0.05f,0.05f)),
                new Image (Path.Combine(TestProjectPath.getPath(),  
                "Assets", "Images", "ball.png")),
                (float) Math.PI /4f);
            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.ControlEvent });
            eventBus.Subscribe(GameEventType.ControlEvent, ball);
            tolerance = 0.0001f;
        }
        
        [Test]
        public void TestRBoundaryCheckers(){
            ball.Shape.Position.X = 1.0f;
            Assert.IsTrue(ball.RightBoundaryCheck());
        }
        [Test]
        public void TestLBoundaryCheckers(){
            ball.Shape.Position.X = 0.0f;
            Assert.IsTrue(ball.LeftBoundaryCheck());
        }
    }
}