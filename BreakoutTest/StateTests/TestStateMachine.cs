using System.Collections.Generic;
using System.IO;
using System;
using NUnit.Framework;
using Breakout;
using Breakout.States;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

namespace BreakoutTest {
    public class StateMachineTest {
        StateMachine machine;
        GameEventBus eventBus;
        [SetUp]
        public void Setup() {
            Window.CreateOpenGLContext();

            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.GameStateEvent });

            machine = new StateMachine();
            
            eventBus.Subscribe(GameEventType.GameStateEvent, machine);
        }

        [Test]
        public void TestInitialState() {
            Assert.That(machine.ActiveState, Is.InstanceOf<MainMenu>());
        }

        [Test]
        public void TestGameRunning() {
            eventBus.RegisterEvent( new GameEvent {
                    EventType = GameEventType.GameStateEvent, 
                    Message = "CHANGE_STATE",
                    StringArg1 = "GAME_RUNNING" });

            eventBus.ProcessEvents();

            Assert.That(machine.ActiveState, Is.InstanceOf<GameRunning>());
        }

        [Test]
        public void TestGamePaused() {
            eventBus.RegisterEvent( new GameEvent { 
                EventType = GameEventType.GameStateEvent,
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_PAUSED"});

            eventBus.ProcessEvents();

            Assert.That(machine.ActiveState, Is.InstanceOf<GamePaused>());
        }
    }
}