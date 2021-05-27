using DIKUArcade.Events;
using DIKUArcade.Entities;

namespace Breakout.GamePlay.BlockEntity {
    public class BlockOrganizer : EntityOrganizer<Block> {

        public BlockOrganizer() : base() {
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);
        }
        public override void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.To == this) {
                ProcessEventValidator(gameEvent);

                Block block = (Block) gameEvent.ObjectArg1;
                block.ReceiveEvent(gameEvent);
            }
        }
    }
}