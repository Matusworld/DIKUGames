using System.Collections.Generic;

using NUnit.Framework;

using Breakout.States;
using DIKUArcade.GUI;
using DIKUArcade.Events;

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

        // Testing the initial state is Main Menu
        [Test]
        public void TestInitialState() {
            Assert.That(machine.ActiveState, Is.InstanceOf<MainMenu>());
        }

        // Testing when pressing new game its change state from mainmenu to state gamerunning
        [Test]
        public void TestGameRunning() {
            eventBus.RegisterEvent( new GameEvent {
                    EventType = GameEventType.GameStateEvent, 
                    Message = "CHANGE_STATE",
                    StringArg1 = "GAME_NEWGAME" });

            eventBus.ProcessEvents();

            Assert.That(machine.ActiveState, Is.InstanceOf<GameRunning>());
        }

        // Testing changeing to state game paused 
        [Test]
        public void TestGamePaused() {
            eventBus.RegisterEvent( new GameEvent { 
                EventType = GameEventType.GameStateEvent,
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_PAUSED"});

            eventBus.ProcessEvents();

            Assert.That(machine.ActiveState, Is.InstanceOf<GamePaused>());
        }

        // Testing changeing to state game running after pausing the game.
        [Test]
        public void TestGameContinue() {
            eventBus.RegisterEvent( new GameEvent { 
                EventType = GameEventType.GameStateEvent,
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_CONTINUE"});

            eventBus.ProcessEvents();

            Assert.That(machine.ActiveState, Is.InstanceOf<GameRunning>());
        }

        // Testing changeing to state Main menu from game pause, since 
        // mainmenu is the starting state, we change it to game paused first, then
        // test for changeing the state to main menu
        [Test]
        public void TestMainMenu() {
            eventBus.RegisterEvent( new GameEvent { 
                EventType = GameEventType.GameStateEvent,
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_PAUSED"});

            eventBus.ProcessEvents();

            Assert.That(machine.ActiveState, Is.InstanceOf<GamePaused>());

            eventBus.RegisterEvent( new GameEvent { 
                EventType = GameEventType.GameStateEvent,
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_MAINMENU"});

            eventBus.ProcessEvents();

            Assert.That(machine.ActiveState, Is.InstanceOf<MainMenu>());
        }
        // Testing changeing to state Game lost
        [Test]
        public void TestGameLost() {
            eventBus.RegisterEvent( new GameEvent { 
                EventType = GameEventType.GameStateEvent,
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_LOST"});

            eventBus.ProcessEvents();

            Assert.That(machine.ActiveState, Is.InstanceOf<GameLost>());
        }

        // Testing changeing to state Game Won
        public void TestGameWon() {
            eventBus.RegisterEvent( new GameEvent { 
                EventType = GameEventType.GameStateEvent,
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_WON"});

            eventBus.ProcessEvents();

            Assert.That(machine.ActiveState, Is.InstanceOf<GameWon>());
        }

    }
}