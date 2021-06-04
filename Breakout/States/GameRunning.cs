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
    /// <summary>
    /// GameRunning is the GameState where the actual game mechanis are running.
    /// Holds all gamerelated objects though not necessarily directly.
    /// Responsible for Move- and Time Updates plus CollisionChecks between Entities.
    /// </summary>
    public class GameRunning : IGameState {
        private static GameRunning instance;

        private Entity backGroundImage;
        public Player player { get; private set; }
        public BallOrganizer ballOrganizer { get; private set; }
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

            //Initialize LevelManager
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

        /// <summary>
        /// Run CollisionCheck on all Blocks for all balls
        /// </summary>
        public void BallBlockCollisionIterate() {
            ballOrganizer.Entities.Iterate(ball => {
                LevelManager.LevelLoader.BlockOrganizer.Entities.Iterate(block => {
                    CollisionData check = CollisionDetection.Aabb(ball.Shape.AsDynamicShape(), block.Shape);
                    if (check.Collision) {
                        //to block
                        block.BlockHit();
                
                        ball.DirectionBlockSetter(check.CollisionDir);
                    }
                });
            });
        }

        /// <summary>
        /// Collision check between a single Ball and the Player.
        /// The relative hitposition on the player is computed and passed on the Ball.
        /// </summary>
        /// <param name="ball">Ball is checked for collision with the Player</param>
        private void BallPlayerCollision(Ball ball) {
            CollisionData check = CollisionDetection.Aabb(ball.Shape.AsDynamicShape(),
                player.Shape.AsDynamicShape());

            if (check.Collision) {
                float hitPosition = PlayerHitPositionTruncator(PlayerHitPosition(ball));
                ball.DirectionPlayerSetter(hitPosition);
            }
        }

        /// <summary>
        /// Compute the relative hitPosition on the Player for its collision with the given Ball.
        /// Designed to return [0.0, 1.0], but due to Collision boundary cases, this amount might
        /// be exceeded.
        /// </summary>
        /// <param name="ball">Ball that is colliding with the Player</param>
        /// <returns></returns>
        public float PlayerHitPosition(Ball ball){
            float numerator = ball.Shape.Position.X + ball.Shape.Extent.X - player.Shape.Position.X;
            float denominator = player.Shape.Extent.X + ball.Shape.Extent.X; 

            return numerator / denominator;
        }

        /// <summary>
        /// Truncate the given hitPosition to the interval [0.0, 1.0]
        /// </summary>
        /// <param name="hitPosition">relative Player hitPosition</param>
        /// <returns></returns>
        public float PlayerHitPositionTruncator(float hitPosition) {
            if (hitPosition < 0.0f) {
                return 0.0f;
            } else if (hitPosition > 1.0f) {
                return 1.0f;
            } else {
                return hitPosition;
            }
        }

        /// <summary>
        /// Collision check between a single PowerUpOrbs and the Player.
        /// Orbhit is always recorded.
        /// </summary>
        /// <param name="orb"> The orb that hit the player</param>
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
            //Movement
            player.Move();
            ballOrganizer.MoveEntities();
            PowerUpOrbOrganizer.MoveEntities();

            //Collision
            PowerUpOrbOrganizer.Entities.Iterate(orb => {
                OrbPlayerCollision(orb);
            });
            ballOrganizer.Entities.Iterate(ball => {
                BallPlayerCollision(ball);
                ball.BoundaryCollision();
            });
            BallBlockCollisionIterate();

            LevelManager.UpdateLevelTimer();
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            
            player.Render();
            player.Healthbar.Render();

            ballOrganizer.RenderEntities();

            score.Render();

            LevelManager.RenderLevel();

            PowerUpOrbOrganizer.RenderEntities();
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
            } else if (action == KeyboardAction.KeyRelease) {
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