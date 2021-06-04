using DIKUArcade.Events;
using DIKUArcade.Timers;


namespace Breakout.GamePlay.BlockEntity {
    /// <summary>
    /// Organizing class for containing and giving mass functionality to Blocks.
    /// Processes Events on behalf of contained Blocks.
    /// </summary>
    public class BlockOrganizer : EntityOrganizer<Block> {
        //Number of Unbreakable Blocks contained in this BlockOrganizer
        public int NumberOfUnbreakables = 0;

        public BlockOrganizer() : base() {
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);
        }

        /// <summary>
        /// Returns true if the current level has ended, i.e.
        /// when only Unbreakable Blocks remain in this BlockOrganizer.
        /// </summary>
        private bool CheckLevelEnded() {
            if (Entities.CountEntities() == NumberOfUnbreakables) {
                return true;
            }
            return false;
        }

        public override void ResetOrganizer() {
            base.ResetOrganizer();
            NumberOfUnbreakables = 0;
        }

        /// <summary>
        /// Process Events related to Blocks.
        /// </summary>
        /// <param name="gameEvent">gameEvent is a string that describes what event happened
        ///  and that is being sent.</param>
        public override void ProcessEvent(GameEvent gameEvent) {
            switch (gameEvent.StringArg1) {
                case "BLOCK_DELETED":
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