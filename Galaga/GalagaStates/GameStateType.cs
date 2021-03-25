using System;

namespace Galaga.GameStates {
    public enum GameStateType {
        GameRunning,
        GamePause,
        MainMenu
    }

    public static class StateTransformer {
        public static GameStateType TransformStringToState(string state) {
            switch(state) {
                case "GAME_RUNNING":
                    return GameStateType.GameRunning;
                case "GAME_PAUSE":
                    return GameStateType.GamePause;
                case "MAIN_MENU":
                    return GameStateType.MainMenu;
                default:
                    throw new ArgumentException("Invalid GameStateType");
            }
        }

        public static string TransformStateToString(GameStateType state) {
            switch(state) {
                case GameStateType.GameRunning:
                    return "GAME_RUNNING";
                case GameStateType.GamePause:
                    return "GAME_PAUSE";
                case GameStateType.MainMenu:
                    return "MAIN_MENU";
                default:
                    throw new ArgumentException("Invalid GameStateType");
            }
        }
    }
}