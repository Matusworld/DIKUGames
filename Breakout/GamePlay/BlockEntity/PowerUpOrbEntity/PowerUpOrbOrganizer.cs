using DIKUArcade.Entities;
using DIKUArcade.Events;

namespace Breakout.GamePlay.BlockEntity.PowerUpOrbEntity {
    public class PowerUpOrbOrganizer : EntityOrganizer<PowerUpOrb> {
        //public EntityContainer<PowerUpOrb> Orbs { get; private set; } 
            
        public int PowerUpDuration { get; private set; } = 5000; //ms
        public PowerUpOrbOrganizer() : base() {
            //Entities = new EntityContainer<PowerUpOrb>();

            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);
        }

        /*
        private void AddOrb(PowerUpOrb orb) {
            Entities.AddEntity(orb);
        }*/
        

        public override void MoveEntities() {
            Entities.Iterate(orb => {
               orb.Move();
           });
        }

        /*
        public void RenderOrbs() {
            Entities.Iterate(orb => {
               orb.RenderEntity();
           });
        }*/


        public override void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.ControlEvent) {
                switch(gameEvent.StringArg1) {
                    case "ADD_ORB":
                        AddEntity((PowerUpOrb) gameEvent.From);
                        break;
                }
            }
        }
    }
}