using DIKUArcade.Entities;
using DIKUArcade.Events;

namespace Breakout.Blocks {
    public class PowerUpOrganizer : IGameEventProcessor {
        public EntityContainer<PowerUpOrb> Orbs { get; private set; } 
            

        public PowerUpOrganizer() {
            Orbs = new EntityContainer<PowerUpOrb>();

            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);
        }

        private void AddOrb(PowerUpOrb orb) {
            Orbs.AddEntity(orb);
        }
        

        public void MoveOrbs() {
            Orbs.Iterate(orb => {
               orb.Move();
           });
        }

        public void RenderOrbs() {
            Orbs.Iterate(orb => {
               orb.RenderEntity();
           });
        }


        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.ControlEvent) {
                switch(gameEvent.StringArg1) {
                    case "ADD_ORB":
                        AddOrb((PowerUpOrb) gameEvent.From);
                        break;
                }
            }
        }
    }
}