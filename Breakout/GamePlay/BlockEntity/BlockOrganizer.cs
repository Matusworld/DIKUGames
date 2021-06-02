using DIKUArcade.Events;
using DIKUArcade.Timers;

namespace Breakout.GamePlay.BlockEntity {
    public class BlockOrganizer : EntityOrganizer<Block> {
        public int NumberOfUnbreakables = 0;

        public BlockOrganizer() : base() {
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);
        }

        public override void ResetOrganizer()
        {
            base.ResetOrganizer();
            NumberOfUnbreakables = 0;
        }

        /// <summary>
        /// Returns true if the current level has ended, i.e.
        /// when only unbreakable blocks are left
        /// </summary>
        private bool CheckLevelEnded() {
            if (Entities.CountEntities() == NumberOfUnbreakables) {
                return true;
            }
            return false;
        }

        public override void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.ControlEvent) {
                switch (gameEvent.StringArg1) {
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