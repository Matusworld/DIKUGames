using System.IO;
using System.Collections.Generic;

using DIKUArcade.Events;
using DIKUArcade.Math;
using DIKUArcade.Timers;

using Breakout.LevelLoading;

namespace Breakout.GamePlay {
    public class LevelManager : IGameEventProcessor {
        public LevelLoader LevelLoader { get; private set; }
        public LevelTimer LevelTimer { get; private set; }
        private LinkedList<string> levelPathSequence;
        private LinkedListNode<string> activeLevelPath;

        public LevelManager(LinkedList<string> levelNameSequence) { 
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);

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

        private void UpdateActiveLevel() {
            LevelLoader.LoadLevel(activeLevelPath.Value);

            LevelTimer.SetNewLevelTime(LevelLoader.Meta.Time);
        }

        public void PreviousLevel() {
            if (activeLevelPath != levelPathSequence.First) {
                activeLevelPath = activeLevelPath.Previous;

                UpdateActiveLevel();
            }
        }

        public void NextLevel() {
            if (activeLevelPath != levelPathSequence.Last) {
                activeLevelPath = activeLevelPath.Next;

                UpdateActiveLevel();
            }
            else {
                BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                    EventType = GameEventType.GameStateEvent,
                    Message = "CHANGE_STATE",
                    StringArg1 = "GAME_WON" });
            }
        }

        public void ResetToFirst() {
            activeLevelPath = levelPathSequence.First;

            UpdateActiveLevel();
        }

        /// <summary>
        /// Returns true if the current level has ended, i.e.
        /// when only unbreakable blocks are left
        /// </summary>
        private bool CheckLevelEnded() {
            if (LevelLoader.BlockOrganizer.Entities.CountEntities() 
                == LevelLoader.NumberOfUnbreakables) {
                return true;
            }
            return false;
        }

        public void UpdateLevelTimer() {
            if (LevelLoader.Meta.Time != 0) {
                LevelTimer.UpdateTimer();
            }
        }

        public void RenderLevel() {
            LevelLoader.BlockOrganizer.RenderEntities();

            // if time is not 0 render Timer, else do not render
            if (LevelLoader.Meta.Time != 0) {
                LevelTimer.Render();
            }
        }

        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.ControlEvent) {
                switch(gameEvent.StringArg1) {
                    case "LEVEL_ENDED":
                        NextLevel();
                        break;
                    case "LEVEL_BACK":
                        PreviousLevel();
                        break;
                    case "BLOCK_DELETED":
                        //Add small delay so that EntityContainer will have cleaned up
                        //the block marked for deletion by the time of block counting
                        BreakoutBus.GetBus().RegisterTimedEvent(
                            new GameEvent { EventType = GameEventType.ControlEvent,
                                StringArg1 = "BLOCK_DELETED_DELAY"},
                            TimePeriod.NewMilliseconds(5));
                        break;
                    case "BLOCK_DELETED_DELAY":
                        if (CheckLevelEnded()) {
                            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                                EventType = GameEventType.ControlEvent,
                                StringArg1 = "LEVEL_ENDED"});
                        }
                        break;
                }
            }
        }
        
    }
}