using NUnit.Framework;
using Galaga.GalagaStates;
using System;
using Galaga;
using DIKUArcade.EventBus;

namespace GalagaTests {
    public class TestGameStateType {
        string validString;
        string invalidString;
        GameStateType validState;
        GameStateType invalidState;

        StateMachine stateMachine;


        [SetUp]
        public void setup() {
            validString = "GAME_PAUSE";
            invalidString = "DO_A_BARREL_ROLL";
            validState = GameStateType.MainMenu;
            stateMachine = new StateMachine();
        }

        [Test]
        public void TestTransformStringToStateValid() {
            GameStateType state = StateTransformer.TransformStringToState(validString);
            
            Assert.AreEqual(GameStateType.GamePause, state);
        }

        [Test]
        public void TestTransformStringToStateInvalid() {
            try {
                StateTransformer.TransformStringToState(invalidString);
                Assert.Fail();
            }
            catch(ArgumentException) {

            }
        }
        
        [Test]
        public void TestTransformStateToStringValid() {
            string state = StateTransformer.TransformStateToString(validState);

            Assert.AreEqual("MAIN_MENU", state);  
        }

        //No invalid GameStateTypes that can give runtime error. 
        //[Test]
        //public void TestTransformStateToStringInvalid() {
        //
        //}

        [Test]
        public void TestInitialState() {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
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

        [Test]
        public void TestEventPaused() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_PAUSED", ""));

            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());
        }
        
    }
}