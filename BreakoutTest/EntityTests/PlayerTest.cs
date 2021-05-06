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
    //Move() must have 100% statement coverage
    //Unit tests that cover a statement of Move is marked with: *Move statement covered*
    public class PlayerTest {
        Player player;
        GameEventBus eventBus;
        float beforeX;
        float beforeY;

        float tolerance;


        [SetUp]
        public void Setup() {
            Window.CreateOpenGLContext();

            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.02f)),
                new Image(Path.Combine(TestProjectPath.getPath() ,"Assets", "Images", "player.png")));

            beforeX = player.GetPosition().X;
            beforeY = player.GetPosition().Y;

            //BreakoutBus.GetBus().InitializeEventBus(new List<GameEventType> {
            //    GameEventType.PlayerEvent } );

            //BreakoutBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
            
            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.PlayerEvent });
            eventBus.Subscribe(GameEventType.PlayerEvent, player);

            tolerance = 0.0001f;
        }

        //Assert that player starts in center and bottom half 
        //which is the precondition of other tests
        [Test]
        public void TestPrecondition() {
            
            float diffx = (player.GetPosition().X + player.GetExtent().X / 2.0f - 0.5f);

            Assert.LessOrEqual(diffx, tolerance);

            Assert.LessOrEqual(0.0f, player.GetPosition().Y);
            Assert.LessOrEqual(player.GetPosition().Y, 0.5f-player.GetExtent().Y);
        }

        //Assert that player does not initially move
        //*Move statement covered*
        [Test]
        public void TestInitMove() {
            
            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });
                
            float diffX = Math.Abs(player.GetPosition().X - beforeX);
            float diffY = Math.Abs(player.GetPosition().Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }        
        
        //*Move statement covered*
        [Test]
        public void TestMoveRight() {

            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent,  Message = "true",
                StringArg1 = "SetMoveRight" });

            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });

            eventBus.ProcessEvents();

            float diffX = Math.Abs(player.GetPosition().X - (beforeX + player.GetMoveSpeed()));
            float diffY = Math.Abs(player.GetPosition().Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }
        
        //*Move statement covered*
        [Test]
        public void TestMoveLeft() {

            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent,  Message = "true",
                StringArg1 = "SetMoveLeft" });

            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });

            eventBus.ProcessEvents();

            float diffX = Math.Abs(player.GetPosition().X - (beforeX - player.GetMoveSpeed()));
            float diffY = Math.Abs(player.GetPosition().Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }
        
        //Assert that the right position bound is respected
        //*Move statement covered*
        [Test]
        public void TestMoveRightOutOfBounds() {
            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent,  Message = "true",
                StringArg1 = "SetMoveRight" });

            //peform "too many" right moves
            for(float p = beforeX; p <= 1.0f; p += player.GetMoveSpeed()) {
                eventBus.RegisterEvent( new GameEvent {
                    EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });

                eventBus.ProcessEvents();
            }

            float diffX = Math.Abs(player.GetPosition().X - player.RightBound);
            float diffY = Math.Abs(player.GetPosition().Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }

        //Assert that the left position bound is respected
        //*Move statement covered*
        [Test]
        public void TestMoveLeftOutOfBounds() {

            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent,  Message = "true",
                StringArg1 = "SetMoveLeft" });
                
            //peform "too many" left moves
            for(float p = beforeX; p >= 0.0f; p -= player.GetMoveSpeed()) {
                eventBus.RegisterEvent( new GameEvent {
                    EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });
                
                eventBus.ProcessEvents();
            }
            
            float diffX = Math.Abs(player.GetPosition().X - player.LeftBound);
            float diffY = Math.Abs(player.GetPosition().Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }
        
        //Assert no stutter on right bound
        [Test]
        public void TestMoveRightNoStutter() {
            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent,  Message = "true",
                StringArg1 = "SetMoveRight" });

            //peform "too many" right moves
            for(float p = beforeX; p <= 1.0f; p += player.GetMoveSpeed()) {
                eventBus.RegisterEvent( new GameEvent {
                    EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });

                eventBus.ProcessEvents();
            }

            //try repeatedly to move out of bounds and assert it keeps same position
            int repeats = 100;

            for(int i = 0; i < repeats; i++) {
                eventBus.RegisterEvent( new GameEvent {
                    EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });

                eventBus.ProcessEvents();

                float diffX = Math.Abs(player.GetPosition().X - player.RightBound);
                float diffY = Math.Abs(player.GetPosition().Y - beforeY);

                Assert.LessOrEqual(diffX, tolerance);
                Assert.LessOrEqual(diffY, tolerance);
            }
        }

        //Assert no stutter on left bound
        [Test]
        public void TestMoveLeftNoStutter() {
            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent,  Message = "true",
                StringArg1 = "SetMoveLeft" });

            //peform "too many" right moves
            for(float p = beforeX; p > 0.0f; p -= player.GetMoveSpeed()) {
                eventBus.RegisterEvent( new GameEvent {
                    EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });

                eventBus.ProcessEvents();
            }

            //try repeatedly to move out of bounds and assert it keeps same position
            int repeats = 100;

            for(int i = 0; i < repeats; i++) {
                eventBus.RegisterEvent( new GameEvent {
                    EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });

                eventBus.ProcessEvents();

                float diffX = Math.Abs(player.GetPosition().X - player.LeftBound);
                float diffY = Math.Abs(player.GetPosition().Y - beforeY);

                Assert.LessOrEqual(diffX, tolerance);
                Assert.LessOrEqual(diffY, tolerance);
            }
        }

        //Assert simultaneous movement will cancel out 
        [Test]
        public void TestSimulMove() {

            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent,  Message = "true",
                StringArg1 = "SetMoveLeft" });

            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent,  Message = "true",
                StringArg1 = "SetMoveRight" });

            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });

            float diffX = Math.Abs(player.GetPosition().X - beforeX);
            float diffY = Math.Abs(player.GetPosition().Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);
        }
        
        //Assert that position will not be changed by updates after movement has stopped.
        [Test]
        public void TestStopMove() {

            //first move once right
            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent,  Message = "true",
                StringArg1 = "SetMoveRight" });

            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });

            eventBus.ProcessEvents();

            float diffX = Math.Abs(player.GetPosition().X - (beforeX + player.GetMoveSpeed()));
            float diffY = Math.Abs(player.GetPosition().Y - beforeY);

            Assert.LessOrEqual(diffX, tolerance);
            Assert.LessOrEqual(diffY, tolerance);

            //then stop right direction
            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent,  Message = "false",
                StringArg1 = "SetMoveRight" });

            eventBus.RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });

            eventBus.ProcessEvents();

            float newdiffX = Math.Abs(player.GetPosition().X - (beforeX + player.GetMoveSpeed()));
            float newdiffY = Math.Abs(player.GetPosition().Y - beforeY);

            Assert.LessOrEqual(newdiffX, tolerance);
            Assert.LessOrEqual(newdiffY, tolerance);
        }
    }
}