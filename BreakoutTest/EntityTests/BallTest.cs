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
    public class BallTest {
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
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.MovementEvent });
            eventBus.Subscribe(GameEventType.MovementEvent, ball);

            tolerance = 0.0001f;
        }
        
        [Test]
        public void TestRBoundaryCheckers() {

            ball.Shape.Position.X = 1.0f;
            Assert.IsTrue(ball.RightBoundaryCheck());
        }

        [Test]
        public void TestLBoundaryCheckers() {

            ball.Shape.Position.X = 0.0f;
            Assert.IsTrue(ball.LeftBoundaryCheck());
        }

        [Test]
        public void TestUBoundaryCheckers() {

            ball.Shape.Position.Y = 1.0f;
             Assert.IsTrue(ball.UpperBoundaryCheck());
        }

        [Test]
        public void TestLOBoundaryCheckers() {

            ball.Shape.Position.Y = -0.1f;
             Assert.IsTrue(ball.LowerBoundaryCheck());
        }

        [Test]
        public void TestDirectionBoundarySetter() {   

            ball.Shape.Position.X = 0.0f;
            ball.Shape.Position.Y = 1.0f;

            ball.Shape.AsDynamicShape().Direction.X = 0.01f;
            ball.Shape.AsDynamicShape().Direction.Y = 0.01f;

            float oldDirectionX = ball.Shape.AsDynamicShape().Direction.X;
            float oldDirectionY = ball.Shape.AsDynamicShape().Direction.Y;

            ball.DirectionBoundarySetter();
            Assert.IsTrue(tolerance >= Math.Abs(oldDirectionX+ball.Shape.AsDynamicShape().Direction.X) && 
                tolerance >= Math.Abs(oldDirectionY+ball.Shape.AsDynamicShape().Direction.Y));
        }

        [Test]
        public void testDirectionPlayerSetter( ) {
            float testTheta = ball.ReturnTheta(0.23f);
            ball.DirectionPlayerSetter(0.23f);
            Assert.IsTrue(tolerance >=Math.Abs(testTheta-1.9949113f));           
            //The values in this test have been calculated in maple to find the appropriate values
            //to test for.
        }
        [Test]
        public void testDirectionPlayerSetterLeftBounce() {

            float testTheta = ball.ReturnTheta(0.0f);
            ball.DirectionPlayerSetter(0.0f);
            Assert.IsTrue(tolerance >= Math.Abs(testTheta - (3f/4f*(float)Math.PI)));
        }
        [Test]
        public void testDirectionPlayerSetterMiddleBounce() {

            float testTheta = ball.ReturnTheta(0.5f);
            ball.DirectionPlayerSetter(0.5f);
            Assert.IsTrue(tolerance >= Math.Abs(testTheta - (2f/4f*(float)Math.PI)));
        }
        [Test]
        public void testDirectionPlayerSetterRightBounce() {
            float testTheta = ball.ReturnTheta(1.0f);

            ball.DirectionPlayerSetter(1.0f);
            Assert.IsTrue(tolerance >= Math.Abs(testTheta - (1f/4f*(float)Math.PI)));
        }
        [Test]
        public void testMove() {

            float newPosX = 0.5f;
            float newPosY = 0.5f;

            ball.Shape.Position.X = 0.49f;
            ball.Shape.Position.Y = 0.49f;

            ball.Shape.AsDynamicShape().Direction.X = 0.01f;
            ball.Shape.AsDynamicShape().Direction.Y = 0.01f;

            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.MovementEvent,
                StringArg1 = "Move" });
            eventBus.ProcessEvents();

            Assert.IsTrue(tolerance >= Math.Abs(newPosX-ball.Shape.Position.X) && 
                tolerance >= Math.Abs(newPosY-ball.Shape.Position.Y));
        }
    }
}