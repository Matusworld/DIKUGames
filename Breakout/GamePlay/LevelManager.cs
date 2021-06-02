using System.IO;
using System.Collections.Generic;

using DIKUArcade.Events;
using DIKUArcade.Math;

using Breakout.LevelLoading;

namespace Breakout.GamePlay {
    /// <summary>
    /// Manages the levels of the games by keeping them sequentially ordered and only allowing 
    /// one active level at a time. Facilitates navigation between levels.
    /// Manages both the LevelLoader and the LevelTimer.
    /// </summary>
    public class LevelManager : IGameEventProcessor {
        private LinkedList<string> levelPathSequence;
        private LinkedListNode<string> activeLevelPath;

        public LevelLoader LevelLoader { get; private set; }
        public LevelTimer LevelTimer { get; private set; }


        public LevelManager(LinkedList<string> levelNameSequence) { 
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);

            //Initialize sequence of paths to level files
            levelPathSequence = new LinkedList<string>();
            foreach (string name in levelNameSequence) {
                string path = Path.Combine(new string[] {
                    ProjectPath.getPath(), "Breakout", "Assets", "Levels", name});
                levelPathSequence.AddLast(path);
            }
            activeLevelPath = levelPathSequence.First;

            LevelLoader = new LevelLoader();   
            LevelLoader.LoadLevel(activeLevelPath.Value);

            LevelTimer = new LevelTimer(
                LevelLoader.Meta.Time, new Vec2F(0.33f, -0.26f), 
                new Vec2F(0.3f, 0.3f));
        }

        /// <summary>
        /// Update this LevelManagers state by Loading its active level and setting its 
        /// LevelTimer to the time of the loaded level.
        /// </summary>
        private void UpdateActiveLevel() {
            LevelLoader.LoadLevel(activeLevelPath.Value);

            LevelTimer.SetNewLevelTime(LevelLoader.Meta.Time);
        }

        /// <summary>
        /// Navigate to the previous level as dictated by the sequential level ordering and set this
        /// level to the active of this LevelManager.
        /// If the active level is already first, then do nothing.
        /// </summary>
        public void PreviousLevel() {
            if (activeLevelPath != levelPathSequence.First) {
                activeLevelPath = activeLevelPath.Previous;

                UpdateActiveLevel();
            }
        }

        /// <summary>
        /// Navigate to the next level as dictated by the sequential level ordering and set this
        /// level to the active of this LevelManager.
        /// If the active level is already last, broadcast that the game has been won.
        /// </summary>
        public void NextLevel() {
            if (activeLevelPath != levelPathSequence.Last) {
                activeLevelPath = activeLevelPath.Next;

                UpdateActiveLevel();
            } else {
                BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                    EventType = GameEventType.GameStateEvent,
                    Message = "CHANGE_STATE",
                    StringArg1 = "GAME_WON" });
            }
        }

        /// <summary>
        /// Navigate to the first level as dictated by the sequential level ordering and set this
        /// level to the active of this LevelManager.
        /// </summary>
        public void ResetToFirst() {
            activeLevelPath = levelPathSequence.First;

            UpdateActiveLevel();
        }

        /// <summary>
        /// Update the LevelTimer of this LevelManager if its active level has a time amount given.
        /// </summary>
        public void UpdateLevelTimer() {
            if (LevelLoader.Meta.Time != 0) {
                LevelTimer.UpdateTimer();
            }
        }

        /// <summary>
        /// Render all Blocks that has been loaded the active level of this LevelManager
        /// </summary>
        public void RenderLevel() {
            LevelLoader.BlockOrganizer.RenderEntities();

            // if time is not 0 render Timer, else do not render
            if (LevelLoader.Meta.Time != 0) {
                LevelTimer.Render();
            }
        }

        /// <summary>
        /// Process Events relevant to Level progress and management.
        /// </summary>
        /// <param name="gameEvent"></param>
        public void ProcessEvent(GameEvent gameEvent) {
            switch (gameEvent.StringArg1) {
                case "LEVEL_ENDED":
                    NextLevel();
                    break;
                case "LEVEL_BACK":
                    PreviousLevel();
                    break;
            }
        }
        
    }
}