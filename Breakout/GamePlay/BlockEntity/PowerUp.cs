using System.IO;

using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;

using Breakout.GamePlay.BlockEntity.PowerUpOrbEntity;

namespace Breakout.GamePlay.BlockEntity {
    public class PowerUp : Block {

        public PowerUp (DynamicShape shape, IBaseImage image) : base(shape, image) {}

        /// <summary>
        /// Randomly choose power up and and spawn it as an entity
        /// </summary>
        private void SpawnPowerUp() {
            PowerUpTypes draw = PowerUpRandom.RandomType();
            
            IBaseImage image;
            Vec2F extent = new Vec2F(0.05f, 0.05f);
            DynamicShape shape = new DynamicShape(this.Shape.Position, extent);

            switch(draw) {
                case PowerUpTypes.ExtraLife:
                    image = new Image(Path.Combine(ProjectPath.getPath(), 
                        "Breakout", "Assets", "Images", "LifePickUp.png"));
                    break;
                case PowerUpTypes.ExtraBall:
                    image = new Image(Path.Combine(ProjectPath.getPath(), 
                        "Breakout", "Assets", "Images", "ExtraBallPowerUp.png"));
                    break;
                case PowerUpTypes.ExtraPoints:
                    image = new Image(Path.Combine(ProjectPath.getPath(), 
                        "Breakout", "Assets", "Images", "pointImage.png"));
                    break;
                case PowerUpTypes.HalfSpeed:
                    image = new Image(Path.Combine(ProjectPath.getPath(), 
                        "Breakout", "Assets", "Images", "HalfSpeedPowerUp.png"));
                    break;
                case PowerUpTypes.DoubleSpeed:
                    image = new Image(Path.Combine(ProjectPath.getPath(), 
                        "Breakout", "Assets", "Images", "DoubleSpeedPowerUp.png"));
                    break;
                default:
                    image = new Image(Path.Combine(ProjectPath.getPath(), 
                        "Breakout", "Assets", "Images", "purple-circle.png"));
                    break;
            }
            PowerUpOrb orb = new PowerUpOrb(shape, image, draw);

            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.ControlEvent, StringArg1 = "ADD_ORB",
                From = orb
            });
        }

        protected override void BlockHit() {
            Damage();
            if (!IsAlive()) {
                // Unsubscribe deleted blocks
                BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, this);
                DeleteEntity();
                SpawnPowerUp();

                // Could add points here
                BreakoutBus.GetBus().RegisterEvent( new GameEvent { 
                    EventType = GameEventType.ControlEvent, StringArg1 = "ADD_SCORE",
                        From = this});
            }
        }
    }
}