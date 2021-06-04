using System.IO;
using System;
using NUnit.Framework;

using DIKUArcade.GUI;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

using Breakout;
using Breakout.GamePlay.PlayerEntity;

namespace BreakoutTest.GamePlayTest.PlayerEntityTest {
    //Move() must have 100% statement coverage
    //Unit tests that cover a statement of Move is marked with: *Move statement covered*
    public class PlayerTest {
        Player player;
        float beforeX;
        float beforeY;

        float tolerance;

        float rightBound;

        float leftBound;

        public PlayerTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void Setup() {
            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.02f)),
                new Image(Path.Combine(ProjectPath.getPath(), 
                    "Breakout", "Assets", "Images", "player.png")));

            beforeX = player.Shape.Position.X;
            beforeY = player.Shape.Position.Y;
            
            rightBound = 1.0f - player.Shape.Extent.X;
            leftBound = 0f;

            tolerance = 0.0001f;
        }

        //Assert that player starts in center and bottom half 
        //which is the precondition of other tests
        [Test]
        public void TestPrecondition() {
            
            float diffx = (player.Shape.Position.X + player.Shape.Extent.X / 2.0f - 0.5f);

            Assert.LessOrEqual(diffx, tolerance);

            Assert.LessOrEqual(0.0f, player.Shape.Position.Y);
            Assert.LessOrEqual(player.Shape.Position.Y, 0.5f-player.Shape.Extent.Y);
        }

        //Assert that player does not initially move
        //*Move statement covered*
        [Test]
        public void TestInitMove() {

            player.Move();

            float diffX = Math.Abs(player.Shape.Position.X - beforeX);
            float diffY = Math.Abs(player.Shape.Position.Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }        
        
        //*Move statement covered*
        [Test]
        public void TestMoveRight() {
            player.SetMoveRight(true);

            player.Move();

            float diffX = Math.Abs(player.Shape.Position.X - (beforeX + Player.MOVEMENT_SPEED));
            float diffY = Math.Abs(player.Shape.Position.Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }
        
        //*Move statement covered*
        [Test]
        public void TestMoveLeft() {
            player.SetMoveLeft(true);

            player.Move();

            float diffX = Math.Abs(player.Shape.Position.X - (beforeX - Player.MOVEMENT_SPEED));
            float diffY = Math.Abs(player.Shape.Position.Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }
        
        //Assert that the right position bound is respected
        //*Move statement covered*
        [Test]
        public void TestMoveRightOutOfBounds() {            
            player.SetMoveRight(true);

            //peform "too many" right moves
            for(float p = beforeX; p <= 1.0f; p += Player.MOVEMENT_SPEED) {
                player.Move();
            }

            float diffX = Math.Abs(player.Shape.Position.X - rightBound);
            float diffY = Math.Abs(player.Shape.Position.Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
            Assert.AreEqual(player.RightBoundaryCheck(), true);
        }
        
        //Assert that the left position bound is respected
        //*Move statement covered*
        [Test]
        public void TestMoveLeftOutOfBounds() {
            player.SetMoveLeft(true);
                
            //peform "too many" left moves
            for(float p = beforeX; p >= 0.0f; p -= Player.MOVEMENT_SPEED) {
                player.Move();
            }

            float diffX = Math.Abs(player.Shape.Position.X - leftBound);
            float diffY = Math.Abs(player.Shape.Position.Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
            Assert.AreEqual(player.LeftBoundaryCheck(), true);
        }
        
        //Assert no stutter on right bound
        [Test]
        public void TestMoveRightNoStutter() {
            player.SetMoveRight(true);

            //peform "too many" right moves
            for(float p = beforeX; p <= 1.0f; p += Player.MOVEMENT_SPEED) {
                player.Move();
            }

            //try repeatedly to move out of bounds and assert it keeps same position
            int repeats = 100;

            for(int i = 0; i < repeats; i++) {
                player.Move();

                float diffX = Math.Abs(player.Shape.Position.X - rightBound);
                float diffY = Math.Abs(player.Shape.Position.Y - beforeY);

                Assert.LessOrEqual(diffX, tolerance);
                Assert.LessOrEqual(diffY, tolerance);
            }
        }
        
        //Assert no stutter on left bound
        [Test]
        public void TestMoveLeftNoStutter() {
            player.SetMoveLeft(true);

            //peform "too many" right moves
            for(float p = beforeX; p > 0.0f; p -= Player.MOVEMENT_SPEED) {
                player.Move();
            }

            //try repeatedly to move out of bounds and assert it keeps same position
            int repeats = 100;

            for(int i = 0; i < repeats; i++) {

                player.Move();

                float diffX = Math.Abs(player.Shape.Position.X - leftBound);
                float diffY = Math.Abs(player.Shape.Position.Y - beforeY);

                Assert.LessOrEqual(diffX, tolerance);
                Assert.LessOrEqual(diffY, tolerance);
            }
        }
        
        //Assert simultaneous movement will cancel out 
        [Test]
        public void TestSimulMove() {
            
            player.SetMoveLeft(true);

            player.SetMoveRight(true);

            player.Move();

            float diffX = Math.Abs(player.Shape.Position.X - beforeX);
            float diffY = Math.Abs(player.Shape.Position.Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }
        
        //Assert that position will not be changed by updates after movement has stopped.
        [Test]
        public void TestStopMove() {

            player.SetMoveRight(true);
            
            player.Move();

            float diffX = Math.Abs(player.Shape.Position.X - (beforeX + Player.MOVEMENT_SPEED));
            float diffY = Math.Abs(player.Shape.Position.Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);

            player.SetMoveRight(false);

            player.Move();

            float newdiffX = Math.Abs(player.Shape.Position.X - (beforeX + Player.MOVEMENT_SPEED));
            float newdiffY = Math.Abs(player.Shape.Position.Y - beforeY);

            Assert.LessOrEqual(newdiffX, tolerance);
            Assert.LessOrEqual(newdiffY, tolerance);
        }

        [Test]
        public void PlayerResetTest() {
            player.SetMoveLeft(true);

            player.Move();

            float diffX = Math.Abs(player.Shape.Position.X - (beforeX - Player.MOVEMENT_SPEED));
            float diffY = Math.Abs(player.Shape.Position.Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);

            player.Reset();


            float diffXx = Math.Abs(player.Shape.Position.X - 0.45f);
            float diffYy = Math.Abs(player.Shape.Position.Y - 0.1f);

            Assert.LessOrEqual(diffXx, tolerance);
            Assert.LessOrEqual(diffYy, tolerance);
        }
    }
}