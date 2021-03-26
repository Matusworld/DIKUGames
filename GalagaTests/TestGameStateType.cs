using NUnit.Framework;
using Galaga.GalagaStates;
using System;
namespace GalagaTests {
    public class TestGameStateType {
        string validString;
        string invalidString;
        GameStateType validState;
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
    }
}