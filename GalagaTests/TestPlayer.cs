using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Galaga;
using DIKUArcade.EventBus;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;
using System;


//Blackbox test - specification-based

// Player Movement Specifications: 
//Horizontal movement only: Left and right key moves player (changes position state) 
//Move in direction of key pressed with constant movement speed until key is released
//If both keys are pressed: no movement 
//Spawn in bottom center (0.45, 0.1) 
//Must not move out of window border

namespace GalagaTests {
    public class TestPlayer {
        Player player;
        GameEventBus<object> eventBus;
        float beforeX;
        float beforeY;

        float tolerance;


        [SetUp]
        public void Setup() {
            DIKUArcade.Window.CreateOpenGLContext();

            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));

            beforeX = player.GetPosition().X;
            beforeY = player.GetPosition().Y;

            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.PlayerEvent});

            eventBus.Subscribe(GameEventType.PlayerEvent, player);

            tolerance = 0.0001f;
        }

        //Assert that player does not initially move
        [Test]
        public void TestInitMove() {
            
            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this, "", "Move", ""));

            float diffX = Math.Abs(player.GetPosition().X - beforeX);
            float diffY = Math.Abs(player.GetPosition().Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }        

        [Test]
        public void TestMoveRight() {

            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this, "true", "SetMoveRight", ""));
            
            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this, "", "Move", ""));

            eventBus.ProcessEvents();

            float diffX = Math.Abs(player.GetPosition().X - (beforeX + player.GetMoveSpeed()));
            float diffY = Math.Abs(player.GetPosition().Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }

        [Test]
        public void TestMoveLeft() {

            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this, "true", "SetMoveLeft", ""));
            
            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this, "", "Move", ""));

            eventBus.ProcessEvents();

            float diffX = Math.Abs(player.GetPosition().X - (beforeX - player.GetMoveSpeed()));
            float diffY = Math.Abs(player.GetPosition().Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }

        //Assert that the right position bound is respected
        [Test]
        public void TestMoveRightOutOfBounds() {
            
            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this, "true", "SetMoveRight", ""));            

            //peform "too many" right moves
            for(float p = beforeX; p <= 1.0f; p += player.GetMoveSpeed()) {
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "", "Move", ""));

                eventBus.ProcessEvents();
            }

            float diffX = Math.Abs(player.GetPosition().X - player.RightBound);
            float diffY = Math.Abs(player.GetPosition().Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }

        //Assert that the left position bound is respected
        [Test]
        public void TestMoveLeftOutOfBounds() {

            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this, "true", "SetMoveLeft", ""));
                
            //peform "too many" left moves
            for(float p = beforeX; p >= 0.0f; p -= player.GetMoveSpeed()) {
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "", "Move", ""));
                
                eventBus.ProcessEvents();
            }
            
            float diffX = Math.Abs(player.GetPosition().X - player.LeftBound);
            float diffY = Math.Abs(player.GetPosition().Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }

        //Assert simultaneous movement will cancel out 
        [Test]
        public void TestSimulMove() {

            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this, "true", "SetMoveLeft", ""));

            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this, "true", "SetMoveRight", ""));

            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this, "", "Move", ""));

            float diffX = Math.Abs(player.GetPosition().X - beforeX);
            float diffY = Math.Abs(player.GetPosition().Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }

        //Assert that position will not be changed by updates after movement has stopped.
        [Test]
        public void TestStopMove() {

            //first move once right
            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this, "true", "SetMoveRight", ""));

            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this, "", "Move", ""));

            eventBus.ProcessEvents();

            float diffX = Math.Abs(player.GetPosition().X - (beforeX + player.GetMoveSpeed()));
            float diffY = Math.Abs(player.GetPosition().Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);

            //then stop right direction
            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this, "false", "SetMoveRight", ""));

            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this, "", "Move", ""));

            eventBus.ProcessEvents();

            float newdiffX = Math.Abs(player.GetPosition().X - (beforeX + player.GetMoveSpeed()));
            float newdiffY = Math.Abs(player.GetPosition().Y - beforeY);

            Assert.LessOrEqual(newdiffX, tolerance);
            Assert.LessOrEqual(newdiffY, tolerance);
        }
    }
}