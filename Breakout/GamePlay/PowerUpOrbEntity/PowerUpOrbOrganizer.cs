using System.IO;

using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Graphics;

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

        public PowerUpOrb GenerateRandomOrb(DynamicShape shape) {
            PowerUpTypes draw = PowerUpRandom.RandomType();
            IBaseImage image;
            PowerUpOrb orb;

            switch(draw) {
                case PowerUpTypes.ExtraLife:
                    image = new Image(Path.Combine(ProjectPath.getPath(), 
                        "Breakout", "Assets", "Images", "LifePickUp.png"));
                    orb = new ExtraLifeOrb(shape, image);
                    break;
                case PowerUpTypes.ExtraBall:
                    image = new Image(Path.Combine(ProjectPath.getPath(), 
                        "Breakout", "Assets", "Images", "ExtraBallPowerUp.png"));
                    orb = new ExtraBallOrb(shape, image);
                    break;
                case PowerUpTypes.ExtraPoints:
                    image = new Image(Path.Combine(ProjectPath.getPath(), 
                        "Breakout", "Assets", "Images", "pointImage.png"));
                    orb = new ExtraPointsOrb(shape, image);
                    break;
                case PowerUpTypes.HalfSpeed:
                    image = new Image(Path.Combine(ProjectPath.getPath(), 
                        "Breakout", "Assets", "Images", "HalfSpeedPowerUp.png"));
                    orb = new HalfSpeedOrb(shape, image, PowerUpDuration);
                    break;
                case PowerUpTypes.DoubleSpeed:
                    image = new Image(Path.Combine(ProjectPath.getPath(), 
                        "Breakout", "Assets", "Images", "DoubleSpeedPowerUp.png"));
                    orb = new DoubleSpeedOrb(shape, image, PowerUpDuration);
                    break;
                default: //default is extra life
                    image = new Image(Path.Combine(ProjectPath.getPath(), 
                        "Breakout", "Assets", "Images", "LifePickUp.png"));
                    orb = new ExtraLifeOrb(shape, image);
                    break;
            }
            return orb;
        } 

        public override void ProcessEvent(GameEvent gameEvent) {
            switch(gameEvent.StringArg1) {
                case "ADD_ORB":
                    DynamicShape shape = (DynamicShape) gameEvent.ObjectArg1;
                    PowerUpOrb orb = GenerateRandomOrb(shape);
                    Entities.AddEntity(orb);
                    break;
            }
        }
    }
}