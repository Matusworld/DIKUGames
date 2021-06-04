using System.Collections.Generic;
using System.IO;
using System;
using NUnit.Framework;

using Breakout.States;
using Breakout.GamePlay.BallEntity;
using Breakout;
using Breakout.GamePlay.BlockEntity;

using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace BreakoutTest.StatesTest {
    public class GameRunningTest {
        GameRunning game;
        float tolerance;

        public GameRunningTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void SetUp() {
            game = new GameRunning();

            tolerance = 0.0001f;
        }

        // Testing Player hit position left with ball
        [Test]
        public void TestPlayerHitPositionLeft() {
            game.ballOrganizer.Entities.Iterate( ball => {
                ball.Shape.Position = game.player.Shape.Position - ball.Shape.Extent;
            });

            game.ballOrganizer.Entities.Iterate( ball => {
                float playerHitPosition = game.PlayerHitPositionTruncator(game.PlayerHitPosition(ball));
                float diff = Math.Abs(playerHitPosition - 0.0f);

                Assert.LessOrEqual(diff, tolerance);
            });
        }

        // Testing Player hit position right with ball
        [Test]
        public void TestPlayerHitPositionRight() {
            game.ballOrganizer.Entities.Iterate( ball => {
                ball.Shape.Position = game.player.Shape.Position + game.player.Shape.Extent;
            });

            game.ballOrganizer.Entities.Iterate( ball => {
                float playerHitPosition = game.PlayerHitPositionTruncator(game.PlayerHitPosition(ball));
                float diff = Math.Abs(playerHitPosition - 1.0f);

                Assert.LessOrEqual(diff, tolerance);
            });
        }

        // Testing Player hit position middle with ball
        [Test]
        public void TestPlayerHitPositionMiddle() {
            game.ballOrganizer.Entities.Iterate( ball => {
                ball.Shape.Position = game.player.Shape.Position + game.player.Shape.Extent / 2;
            });

            game.ballOrganizer.Entities.Iterate( ball => {
                float playerHitPosition = game.PlayerHitPositionTruncator(game.PlayerHitPosition(ball));
                Assert.LessOrEqual(playerHitPosition, 1.0f);
                Assert.GreaterOrEqual(playerHitPosition, 0.0f);
            });
        }

        // Testing ball colliding with 2 blocks, 
        // both blocks a hit but ball only change direction once
        [Test]
        public void TestBallBlockCollision() {
            game.ballOrganizer.Entities.Iterate( ball => {
                Block block1 = new Block(new DynamicShape(
                        ball.Shape.Position - new Vec2F(0.05f, 0.05f), new Vec2F(0.1f, 0.05f)), 
                    new Image(Path.Combine(
                        ProjectPath.getPath(), "Breakout", "Assets", "Images", "blue-block.png")),
                    new Image(Path.Combine(ProjectPath.getPath(),
                        "Breakout", "Assets", "Images", "blue-block-damaged.png")));
                
                Block block2 = new Block(new DynamicShape(
                        ball.Shape.Position - new Vec2F(0.05f, 0.05f), new Vec2F(0.1f, 0.05f)), 
                    new Image(Path.Combine(
                        ProjectPath.getPath(), "Breakout", "Assets", "Images", "blue-block.png")),
                    new Image(Path.Combine(ProjectPath.getPath(),
                        "Breakout", "Assets", "Images", "blue-block-damaged.png")));

                game.LevelManager.LevelLoader.BlockOrganizer.Entities.ClearContainer();

                // Two blocks with same position as ball
                game.LevelManager.LevelLoader.BlockOrganizer.AddEntity(block1);
                game.LevelManager.LevelLoader.BlockOrganizer.AddEntity(block2);

                ball.Shape.AsDynamicShape().Direction = new Vec2F(0.0f,-0.005f);

                Vec2F oldBallDirection = ball.Shape.AsDynamicShape().Direction;

                game.BallBlockCollisionIterate();

                System.Threading.Thread.Sleep(1000);

                TestContext.Progress.WriteLine("Block1 pos x: " + block1.Shape.Position.X);
                TestContext.Progress.WriteLine("Block1 pos y: " + block1.Shape.Position.Y);
                TestContext.Progress.WriteLine(game.LevelManager.LevelLoader.BlockOrganizer.Entities.CountEntities());
                TestContext.Progress.WriteLine(game.ballOrganizer.Entities.CountEntities());

                Assert.IsTrue(block1.IsDeleted());
                Assert.IsTrue(block2.IsDeleted());
                Assert.AreEqual(ball.Shape.AsDynamicShape().Direction.Y, -oldBallDirection.Y);
                Assert.AreEqual(ball.Shape.AsDynamicShape().Direction.X, oldBallDirection.X);
            });
        }
    }
}