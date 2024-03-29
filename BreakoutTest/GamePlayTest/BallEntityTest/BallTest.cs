using System.Collections.Generic;
using System.IO;
using System;
using System.Threading;
using NUnit.Framework;

using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;
using DIKUArcade.Timers;
using DIKUArcade.Physics;

using Breakout;
using Breakout.GamePlay.BallEntity;
using Breakout.GamePlay.PowerUpOrbEntity;

namespace BreakoutTest.GamePlayTest.BallEntityTest {
    public class BallTest {
        Ball ball;
        BallOrganizer ballOrganizer;
        GameEventBus eventBus;
        float tolerance;

        public BallTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void setup() {
            ball = new Ball (
                new DynamicShape (new Vec2F(0.45f, 0.5f), new Vec2F(0.05f,0.05f)),
                new Image (Path.Combine(ProjectPath.getPath(),  
                "Breakout", "Assets", "Images", "ball.png")),
                (float) Math.PI /4f, false, false);

            ballOrganizer = new BallOrganizer();
            ballOrganizer.AddEntity(ball);

            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.ControlEvent });
            eventBus.Subscribe(GameEventType.ControlEvent, ballOrganizer);


            tolerance = 0.0001f;
        }
        
        // Testing Right boundary
        [Test]
        public void TestRBoundaryCheckers() {

            ball.Shape.Position.X = 1.01f;
            Assert.IsTrue(ball.RightBoundaryCheck());
        }

        // Testing Left boundary
        [Test]
        public void TestLBoundaryCheckers() {

            ball.Shape.Position.X = -0.01f;
            Assert.IsTrue(ball.LeftBoundaryCheck());
        }

        // Testing Upper boundary
        [Test]
        public void TestUBoundaryCheckers() {

            ball.Shape.Position.Y = 1.01f;
             Assert.IsTrue(ball.UpperBoundaryCheck());
        }

        // Testing Lower boundary
        [Test]
        public void TestLOBoundaryCheckers() {

            ball.Shape.Position.Y = -0.01f;
             Assert.IsTrue(ball.LowerBoundaryCheck());
        }

        //Test that correct dimension direction is reversed when hitting a boundary
        [Test]
        public void TestDirectionBoundarySetter() {   

            ball.Shape.Position.X = -0.01f;
            ball.Shape.Position.Y = 1.01f;

            ball.Shape.AsDynamicShape().Direction.X = 0.01f;
            ball.Shape.AsDynamicShape().Direction.Y = 0.01f;

            float oldDirectionX = ball.Shape.AsDynamicShape().Direction.X;
            float oldDirectionY = ball.Shape.AsDynamicShape().Direction.Y;

            ball.BoundaryCollision();
            Assert.IsTrue(
                tolerance >= Math.Abs(oldDirectionX + ball.Shape.AsDynamicShape().Direction.X) 
                && 
                tolerance >= Math.Abs(oldDirectionY + ball.Shape.AsDynamicShape().Direction.Y));
        }

        [Test]
        public void testDirectionPlayerSetterLeftBounce() {
            float playerHitPos = 0.0f;
            ball.DirectionPlayerSetter(playerHitPos);

            Assert.IsTrue(tolerance >= Math.Abs(ball.GetTheta() - (0.75f*(float)Math.PI)));
        }
        [Test]
        public void testDirectionPlayerSetterMiddleBounce() {
            float playerHitPos = 0.5f;
            ball.DirectionPlayerSetter(playerHitPos);

            Assert.IsTrue(tolerance >= Math.Abs(ball.GetTheta() - (0.5f*(float)Math.PI)));
        }
        [Test]
        public void testDirectionPlayerSetterRightBounce() {
            float playerHitPos = 1.0f;
            ball.DirectionPlayerSetter(playerHitPos);

            Assert.IsTrue(tolerance >= Math.Abs(ball.GetTheta() - (0.25*(float)Math.PI)));
        }

        [Test]
        public void testDirectionPlayerSetterNonTrivialBounce() {
            float playerHitPos = 0.23f;
            ball.DirectionPlayerSetter(playerHitPos);
            //math to compute rebounce dir
            float expTheta = 0.75f * (float) Math.PI - (playerHitPos*(float)Math.PI / 2f);

            Assert.IsTrue(tolerance >= Math.Abs(ball.GetTheta() - expTheta));           
        }

        [Test]
        public void testMove() {

            ball.Shape.Position.X = 0.49f;
            ball.Shape.Position.Y = 0.49f;

            ball.Shape.AsDynamicShape().Direction.X = 0.01f;
            ball.Shape.AsDynamicShape().Direction.Y = 0.01f;

            float expPosX = ball.Shape.Position.X + ball.Shape.AsDynamicShape().Direction.X;
            float expPosY = ball.Shape.Position.Y + ball.Shape.AsDynamicShape().Direction.Y;

            //update position by moving
            ball.Move();

            Assert.IsTrue(tolerance >= Math.Abs(expPosX - ball.Shape.Position.X) 
                && 
                tolerance >= Math.Abs(expPosY - ball.Shape.Position.Y));
        }

        // Testing when the ball hits a block from the direction up
        [Test]
        public void TestBallBlockCollisionUP() {
            float Dy = -ball.Shape.AsDynamicShape().Direction.Y;
            float Dx = ball.Shape.AsDynamicShape().Direction.X;

            float theta = (float) Math.Atan2(Dy,Dx);

            ball.DirectionBlockSetter(CollisionDirection.CollisionDirUp);

            float diff = (ball.GetTheta() - theta);

            Assert.LessOrEqual(diff, tolerance);

        }

        // Testing when the ball hits a block from the direction down
        [Test]
        public void TestBallBlockCollisionDown() {
            float Dy = -ball.Shape.AsDynamicShape().Direction.Y;
            float Dx = ball.Shape.AsDynamicShape().Direction.X;

            float theta = (float) Math.Atan2(Dy,Dx);

            ball.DirectionBlockSetter(CollisionDirection.CollisionDirDown);

            float diff = (ball.GetTheta() - theta);

            Assert.LessOrEqual(diff, tolerance);
        }

        // Testing when the ball hits a block from the direction left
        [Test]
        public void TestBallBlockCollisionLeft() {
            float Dy = ball.Shape.AsDynamicShape().Direction.Y;
            float Dx = -ball.Shape.AsDynamicShape().Direction.X;

            float theta = (float) Math.Atan2(Dy,Dx);

            ball.DirectionBlockSetter(CollisionDirection.CollisionDirLeft);

            float diff = (ball.GetTheta() - theta);

            Assert.LessOrEqual(diff, tolerance);
        }

        // Testing when the ball hits a block from the direction right
        [Test]
        public void TestBallBlockCollisionRight() {
            float Dy = ball.Shape.AsDynamicShape().Direction.Y;
            float Dx = -ball.Shape.AsDynamicShape().Direction.X;

            float theta = (float) Math.Atan2(Dy,Dx);

            ball.DirectionBlockSetter(CollisionDirection.CollisionDirRight);

            float diff = (ball.GetTheta() - theta);

            Assert.LessOrEqual(diff, tolerance);
        }
        
        
        // Testing When player picks up powerorb double speed, that the ball gains double speed
        [Test]
        public void TestDoubleSpeed() {
            float startspeed = ball.speed;

            ball.DoubleSpeedActivate();

            float diff = Math.Abs(ball.speed - startspeed * 2f);

            Assert.LessOrEqual(diff, tolerance);

            Assert.IsTrue(ball.DoubleSpeedActive);

            ball.DoubleSpeedDeactivate();

            float diffs = Math.Abs(ball.speed - startspeed);
            
            Assert.LessOrEqual(diffs, tolerance);
            Assert.IsFalse(ball.DoubleSpeedActive);
        }
        
        // Testing When player picks up powerorb half speed, that the ball gets half speed
        [Test]
        public void TestHalfSpeed() {
            float startspeed = ball.speed;

            ball.HalfSpeedActivate();

            float diff = Math.Abs(ball.speed - startspeed * 0.5f);

            Assert.LessOrEqual(diff, tolerance);
            Assert.IsTrue(ball.HalfSpeedActive);

            ball.HalfSpeedDeactivate();

            float diffs = Math.Abs(ball.speed - startspeed);

            Assert.LessOrEqual(diffs, tolerance);
            Assert.IsFalse(ball.HalfSpeedActive);
        }
    }
}