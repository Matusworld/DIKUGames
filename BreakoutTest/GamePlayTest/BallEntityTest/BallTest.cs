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
        float tolerance;

        public BallTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void setup() {
            ball = new Ball (
                new DynamicShape (new Vec2F(0.45f, 0.5f), new Vec2F(0.05f,0.05f)),
                new Image (Path.Combine(TestProjectPath.getPath(),  
                "Assets", "Images", "ball.png")),
                (float) Math.PI /4f, false, false);

            ballOrganizer = new BallOrganizer();
            ballOrganizer.AddEntity(ball);

            tolerance = 0.0001f;
        }
        
        // Testing Right boundary
        [Test]
        public void TestRBoundaryCheckers() {

            ball.Shape.Position.X = 1.01f;
            Assert.IsTrue(ball.RightBoundaryCheck());

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, ballOrganizer);
        }

        // Testing Left boundary
        [Test]
        public void TestLBoundaryCheckers() {

            ball.Shape.Position.X = -0.01f;
            Assert.IsTrue(ball.LeftBoundaryCheck());

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, ballOrganizer);
        }

        // Testing Upper boundary
        [Test]
        public void TestUBoundaryCheckers() {

            ball.Shape.Position.Y = 1.01f;
            Assert.IsTrue(ball.UpperBoundaryCheck());

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, ballOrganizer);
        }

        // Testing Lower boundary
        [Test]
        public void TestLOBoundaryCheckers() {

            ball.Shape.Position.Y = -0.01f;
            Assert.IsTrue(ball.LowerBoundaryCheck());

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, ballOrganizer);
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

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, ballOrganizer);
        }

        [Test]
        public void testDirectionPlayerSetterLeftBounce() {
            float playerHitPos = 0.0f;
            ball.DirectionPlayerSetter(playerHitPos);

            Assert.IsTrue(tolerance >= Math.Abs(ball.GetTheta() - (0.75f*(float)Math.PI)));

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, ballOrganizer);
        }
        [Test]
        public void testDirectionPlayerSetterMiddleBounce() {
            float playerHitPos = 0.5f;
            ball.DirectionPlayerSetter(playerHitPos);

            Assert.IsTrue(tolerance >= Math.Abs(ball.GetTheta() - (0.5f*(float)Math.PI)));

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, ballOrganizer);
        }
        [Test]
        public void testDirectionPlayerSetterRightBounce() {
            float playerHitPos = 1.0f;
            ball.DirectionPlayerSetter(playerHitPos);

            Assert.IsTrue(tolerance >= Math.Abs(ball.GetTheta() - (0.25*(float)Math.PI)));

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, ballOrganizer);
        }

        [Test]
        public void testDirectionPlayerSetterNonTrivialBounce() {
            float playerHitPos = 0.23f;
            ball.DirectionPlayerSetter(playerHitPos);
            //math to compute rebounce dir
            float expTheta = 0.75f * (float) Math.PI - (playerHitPos*(float)Math.PI / 2f);

            Assert.IsTrue(tolerance >= Math.Abs(ball.GetTheta() - expTheta));   

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, ballOrganizer);        
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

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, ballOrganizer);
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

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, ballOrganizer);
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

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, ballOrganizer);
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

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, ballOrganizer);
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

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, ballOrganizer);
        }
        
        
        // Testing When player picks up powerorb double speed, that the ball gains double speed
        [Test]
        public void TestDoubleSpeed() {
            float startspeed = ball.speed;

            PowerUpOrbOrganizer PUorganizer = new PowerUpOrbOrganizer();

            BreakoutBus.GetBus().RegisterEvent ( new GameEvent { EventType = GameEventType.ControlEvent, 
                StringArg1 = "DOUBLE_SPEED", Message = "ACTIVATE"});

            BreakoutBus.GetBus().RegisterTimedEvent (
                new GameEvent{ EventType = GameEventType.ControlEvent,
                    StringArg1 = "DOUBLE_SPEED", Message = "DEACTIVATE"},
                    TimePeriod.NewMilliseconds(PUorganizer.PowerUpDuration));

            BreakoutBus.GetBus().ProcessEventsSequentially();

            float diff = Math.Abs(ball.speed - startspeed * 2f);

            Assert.LessOrEqual(diff, tolerance);

            Assert.IsTrue(ball.DoubleSpeedActive);

            //Extra sleep time added to be sure timed event is completed
            Thread.Sleep(PUorganizer.PowerUpDuration + 500);

            BreakoutBus.GetBus().ProcessEventsSequentially();

            Assert.IsFalse(ball.DoubleSpeedActive);

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, PUorganizer);
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, ballOrganizer);
        }
        
        // Testing When player picks up powerorb half speed, that the ball gets half speed
        [Test]
        public void TestHalfSpeed() {
            float startspeed = ball.speed;

            PowerUpOrbOrganizer PUorganizer = new PowerUpOrbOrganizer();

            BreakoutBus.GetBus().RegisterEvent ( new GameEvent { EventType = GameEventType.ControlEvent, 
                StringArg1 = "HALF_SPEED", Message = "ACTIVATE"});

            BreakoutBus.GetBus().RegisterTimedEvent (
                new GameEvent{ EventType = GameEventType.ControlEvent,
                    StringArg1 = "HALF_SPEED", Message = "DEACTIVATE"},
                    TimePeriod.NewMilliseconds(PUorganizer.PowerUpDuration));

            BreakoutBus.GetBus().ProcessEventsSequentially();

            float diff = Math.Abs(ball.speed - startspeed * 0.5f);

            Assert.LessOrEqual(diff, tolerance);

            Assert.IsTrue(ball.HalfSpeedActive);

            //Extra sleep time added to be sure timed event is completed
            Thread.Sleep(PUorganizer.PowerUpDuration + 500);

            BreakoutBus.GetBus().ProcessEventsSequentially();

            Assert.IsFalse(ball.HalfSpeedActive);

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, PUorganizer);
            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, ballOrganizer);
        }
    }
}