using DIKUArcade.Entities;
using DIKUArcade.Events;

namespace Breakout.GamePlay.PowerUpOrbEntity {
    public class PowerUpOrbOrganizer : EntityOrganizer<PowerUpOrb> {
        public int PowerUpDuration { get; private set; } = 5000; //ms
        public PowerUpOrbOrganizer() : base() {
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);
        }

        public override void MoveEntities() {
            Entities.Iterate(orb => {
               orb.Move();
           });
        }

        public override void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.ControlEvent) {
                switch(gameEvent.StringArg1) {
                    case "ADD_ORB":
                        ProcessEventValidator(gameEvent);
                        AddEntity((PowerUpOrb) gameEvent.ObjectArg1);
                        break;
                }
            }
        }
    }
}