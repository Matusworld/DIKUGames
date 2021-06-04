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
    }
}