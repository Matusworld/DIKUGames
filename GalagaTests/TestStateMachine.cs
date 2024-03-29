using System.Collections.Generic;
using NUnit.Framework;
using Galaga;
using Galaga.GalagaStates;
using DIKUArcade.EventBus;

namespace GalagaTests {
    [TestFixture]
    public class StateMachineTesting {
        
        private StateMachine stateMachine;

        [SetUp]
        public void InitiateStateMachine() {
            DIKUArcade.Window.CreateOpenGLContext();

            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> { GameEventType.InputEvent,
                GameEventType.PlayerEvent, GameEventType.ControlEvent, GameEventType.GraphicsEvent,
                GameEventType.GameStateEvent});

            stateMachine = new StateMachine();        
        }

        [Test]
        public void TestInitialState() {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        } 

        [Test]
        public void TestEventGamePaused() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this, 
                    "CHANGE_STATE",
                    "GAME_PAUSED", ""));
            
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());
        }

        [Test]
        public void TestEventGameRunning() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this, 
                    "CHANGE_STATE",
                    "GAME_RUNNING", ""));
            
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameRunning>());
        }
    }
}