using System.Collections.Generic;
using NUnit.Framework;
using Galaga;
using Galaga.GameStates;
using DIKUArcade.EventBus;

namespace GalagaTests {
    [TestFixture]
    public class StateMachineTesting {
        
        private StateMachine stateMachine;

        [SetUp]
        public void InitiateStateMachine() {
            DIKUArcade.Window.CreateOpenGLContext();

            GalagaBus.GetBus().InitializeEventBus(
                new List<GameEventType> { GameEventType.GameStateEvent, GameEventType.InputEvent });

            stateMachine = new StateMachine();

            //subscribe          
        }

        [Test]
        public void TestInitialState() {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>()) //<- class
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
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>()) //<- class
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
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameRunning>()) //<- class
        }
    }
}