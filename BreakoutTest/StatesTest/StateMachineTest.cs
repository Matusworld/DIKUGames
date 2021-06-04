using NUnit.Framework;

using DIKUArcade.GUI;
using DIKUArcade.Events;

using Breakout;
using Breakout.States;

namespace BreakoutTest.StatesTest {
    public class StateMachineTest {
        StateMachine machine;

        public StateMachineTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void Setup() {
            machine = new StateMachine();
        }

        // Testing the initial state is Main Menu
        [Test]
        public void TestInitialState() {
            Assert.That(machine.ActiveState, Is.InstanceOf<MainMenu>());

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.GameStateEvent, machine);
        }

        // Testing when pressing new game its change state from mainmenu to state gamerunning
        [Test]
        public void TestGameRunning() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                    EventType = GameEventType.GameStateEvent, 
                    Message = "CHANGE_STATE",
                    StringArg1 = "GAME_NEWGAME" });

            BreakoutBus.GetBus().ProcessEventsSequentially();

            Assert.That(machine.ActiveState, Is.InstanceOf<GameRunning>());

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.GameStateEvent, machine);
            ((GameRunning) machine.ActiveState).UnsubscribeAll();
        }

        // Testing changeing to state game paused 
        [Test]
        public void TestGamePaused() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent { 
                EventType = GameEventType.GameStateEvent,
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_PAUSED"});

            BreakoutBus.GetBus().ProcessEventsSequentially();

            Assert.That(machine.ActiveState, Is.InstanceOf<GamePaused>());

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.GameStateEvent, machine);
        }

        // Testing changing to state game running after pausing the game.
        [Test]
        public void TestGameContinue() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent { 
                EventType = GameEventType.GameStateEvent,
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_CONTINUE"});

            BreakoutBus.GetBus().ProcessEventsSequentially();

            Assert.That(machine.ActiveState, Is.InstanceOf<GameRunning>());

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.GameStateEvent, machine);
            ((GameRunning) machine.ActiveState).UnsubscribeAll();
        }

        // Testing changeing to state Main menu from game pause, since 
        // mainmenu is the starting state, we change it to game paused first, then
        // test for changeing the state to main menu
        [Test]
        public void TestMainMenu() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent { 
                EventType = GameEventType.GameStateEvent,
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_PAUSED"});

            BreakoutBus.GetBus().ProcessEventsSequentially();

            Assert.That(machine.ActiveState, Is.InstanceOf<GamePaused>());

            BreakoutBus.GetBus().RegisterEvent( new GameEvent { 
                EventType = GameEventType.GameStateEvent,
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_MAINMENU"});

            BreakoutBus.GetBus().ProcessEventsSequentially();

            Assert.That(machine.ActiveState, Is.InstanceOf<MainMenu>());

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.GameStateEvent, machine);
        }

        // Testing changeing to state Game lost
        [Test]
        public void TestGameLost() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent { 
                EventType = GameEventType.GameStateEvent,
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_LOST"});

            BreakoutBus.GetBus().ProcessEventsSequentially();

            Assert.That(machine.ActiveState, Is.InstanceOf<GameLost>());

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.GameStateEvent, machine);
        }

        // Testing changeing to state Game Won
        public void TestGameWon() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent { 
                EventType = GameEventType.GameStateEvent,
                Message = "CHANGE_STATE",
                StringArg1 = "GAME_WON"});

            BreakoutBus.GetBus().ProcessEventsSequentially();

            Assert.That(machine.ActiveState, Is.InstanceOf<GameWon>());

            //unsubscribe to prevent corrupt memory
            BreakoutBus.GetBus().Unsubscribe(GameEventType.GameStateEvent, machine);
        }

    }
}