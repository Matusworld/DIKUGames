using System.IO;
using System.Collections.Generic;

using DIKUArcade.State;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Input;
using DIKUArcade.Physics;

using Breakout.GamePlay;
using Breakout.GamePlay.PlayerEntity;
using Breakout.GamePlay.BallEntity;
using Breakout.GamePlay.BlockEntity;
using Breakout.GamePlay.PowerUpOrbEntity;

namespace Breakout.States {
    public class GameRunning : IGameState {
        private static GameRunning instance;
        private Entity backGroundImage;
        private Player player;
        private BallOrganizer ballOrganizer;
        private PowerUpOrbOrganizer PowerUpOrbOrganizer;
        public LevelManager LevelManager { get; private set; }
        private Score score;
        
        public GameRunning() {
            //Initialize backGroundImage
            Vec2F imagePos = new Vec2F(0f,0f);
            Vec2F imageExtent = new Vec2F(1f, 1f);
            StationaryShape shape = new StationaryShape(imagePos, imageExtent);
            IBaseImage image = new Image(Path.Combine( ProjectPath.getPath(),
                "Breakout", "Assets", "Images", "SpaceBackground.png"));
            backGroundImage = new Entity(shape, image);
              
            LinkedList<string> levelSequence = new LinkedList<string> (new List<string> { "level1.txt", "level2.txt", "level3.txt",
                "central-mass.txt", "columns.txt", "wall.txt" });
            LevelManager = new LevelManager(levelSequence);

            PowerUpOrbOrganizer = new PowerUpOrbOrganizer();

            ballOrganizer = new BallOrganizer();
            ballOrganizer.ResetOrganizer();

            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.02f)),
                new Image(Path.Combine(ProjectPath.getPath(),
                "Breakout", "Assets", "Images", "player.png")));

            score = new Score(new Vec2F(0.00f, -0.26f), new Vec2F(0.3f, 0.3f));
        }

        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }

        private void BallBlockCollisionIterate() {
            ballOrganizer.Entities.Iterate(ball => {
                bool hit = false;
                LevelManager.LevelLoader.BlockOrganizer.Entities.Iterate(block => {
                    BallBlockCollision(block, ball, ref hit);
                });
            });
        }

        private void BallBlockCollision(Block block, Ball ball, ref bool priorHit) {
            CollisionData check = CollisionDetection.Aabb(ball.Shape.AsDynamicShape(), block.Shape);
            if (check.Collision) {
                //to block
                block.BlockHit();
                
                //only send to ball if not priorly hit this iteration
                if (!priorHit) {
                    priorHit = true;

                    ball.DirectionBlockSetter(check.CollisionDir);
                }
            }
        }

        private void BallPlayerCollision(Ball ball) {
            CollisionData check = CollisionDetection.Aabb(ball.Shape.AsDynamicShape(),
                player.Shape.AsDynamicShape());

            if (check.Collision) {
                float hitPosition = PlayerHitPositionTruncator(PlayerHitPosition(ball));
                ball.DirectionPlayerSetter(hitPosition);
            }
        }

        private float PlayerHitPosition(Ball ball){
            float numerator = ball.Shape.Position.X + ball.Shape.Extent.X - player.Shape.Position.X;
            float denominator = player.Shape.Extent.X + ball.Shape.Extent.X; 

            return numerator / denominator;
        }

        private float PlayerHitPositionTruncator(float hitPosition) {
            if (hitPosition < 0.0f) {
                return 0.0f;
            } 
            else if (hitPosition > 1.0f) {
                return 1.0f;
            }
            else {
                return hitPosition;
            }
        }
        private void OrbPlayerCollision(PowerUpOrb orb) {
            CollisionData check = CollisionDetection.Aabb(orb.Shape.AsDynamicShape(), player.Shape);

            if(check.Collision) {
                orb.PlayerCollision();
            }
        }

        public void ResetState() {
            LevelManager.ResetToFirst();

            PowerUpOrbOrganizer.ResetOrganizer();

            ballOrganizer.ResetOrganizer();

            player.Reset();

            player.Healthbar.Reset();

            score.Reset();  
        }

        public void UpdateState() {
            player.Move();

            //balls move
            ballOrganizer.MoveEntities();

            //PowerUpOrb move
            PowerUpOrbOrganizer.MoveEntities();

            PowerUpOrbOrganizer.Entities.Iterate(orb => {
                OrbPlayerCollision(orb);
            });

            ballOrganizer.Entities.Iterate(ball => {
                BallPlayerCollision(ball);
            });

            BallBlockCollisionIterate();

            LevelManager.UpdateLevelTimer();
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            
            player.Render();
            player.Healthbar.Render();

            ballOrganizer.RenderEntities();

            PowerUpOrbOrganizer.RenderEntities();

            score.Render();

            LevelManager.RenderLevel();
        }

        public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
            if (action == KeyboardAction.KeyPress) {
                switch (key) {
                    case KeyboardKey.A:
                    case KeyboardKey.Left:
                        player.SetMoveLeft(true);
                        break;
                    case KeyboardKey.D:
                    case KeyboardKey.Right:
                        player.SetMoveRight(true);
                        break;
                }
            }
            else if (action == KeyboardAction.KeyRelease) {
                switch (key) {
                    case KeyboardKey.A:
                    case KeyboardKey.Left:
                        player.SetMoveLeft(false);
                        break;
                    case KeyboardKey.D:
                    case KeyboardKey.Right:
                        player.SetMoveRight(false);
                        break;
                    case KeyboardKey.P:
                        BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                            EventType = GameEventType.GameStateEvent,
                            Message = "CHANGE_STATE",
                            StringArg1 = "GAME_PAUSED" });
                            break;
                    case KeyboardKey.Plus:
                        BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                            EventType = GameEventType.ControlEvent,
                            StringArg1 = "LEVEL_ENDED"});
                        break;
                    case KeyboardKey.Minus:
                        BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                            EventType = GameEventType.ControlEvent,
                            StringArg1 = "LEVEL_BACK"});
                        break;
                }
            }
        }
    }
}