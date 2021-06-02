using System.IO;

using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout.GamePlay.PowerUpOrbEntity {
    /// <summary>
    /// Organizing class for containing and giving mass functionality to PowerUpOrbs.
    /// Processes Events on behalf of contained PowerUpOrbs.
    /// </summary>
    public class PowerUpOrbOrganizer : EntityOrganizer<PowerUpOrb> {
        public int PowerUpDuration { get; private set; } = 5000; //ms
        public Vec2F PowerUpExtent { get; private set; } = new Vec2F(0.05f, 0.05f);

        public PowerUpOrbOrganizer() : base() {
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);
        }

        public override void MoveEntities() {
            Entities.Iterate(orb => {
               orb.Move();
           });
        }

        public PowerUpOrb GenerateRandomOrb(Vec2F position) {
            DynamicShape shape = new DynamicShape(position, PowerUpExtent);
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
                case "SPAWN_ORB":
                    Vec2F position = (Vec2F) gameEvent.ObjectArg1;
                    PowerUpOrb orb = GenerateRandomOrb(position);
                    Entities.AddEntity(orb);
                    break;
            }
        }
    }
}