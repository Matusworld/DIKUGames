using NUnit.Framework;
using Galaga.GameStates;
using System;

namespace GalagaTests {
    public class TestGameStateType {
        string validString;
        string invalidString;
        GameStateType validState;
        GameStateType invalidState;


        [SetUp]
        public void setup() {
            validString = "GAME_PAUSE";
            invalidString = "DO_A_BARREL_ROLL";
            validState = GameStateType.MainMenu;
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