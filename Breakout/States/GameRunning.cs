using System.IO;
using System.Collections.Generic;

using DIKUArcade.State;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Input;
using DIKUArcade.Physics;
using DIKUArcade.Timers;

using Breakout.GamePlay;
using Breakout.GamePlay.PlayerEntity;
using Breakout.GamePlay.BallEntity;
using Breakout.GamePlay.BlockEntity;
using Breakout.GamePlay.PowerUpOrbEntity;
using Breakout.LevelLoading;

namespace Breakout.States {
    public class GameRunning : IGameState {
        private static GameRunning instance;
        private Entity backGroundImage;
        private Player player;
        private BallOrganizer ballOrganizer;
        private PowerUpOrbOrganizer PUOOrganizer;
        //public LevelLoader LevelLoader { get; private set; }
        //private List<string> levelSequence;
        //public int LevelIndex { get; private set; }
        public LevelManager LevelManager { get; private set; }
        private Score score;
        //private BreakoutTimer gameTimer;
        
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
            //LevelIndex = 0;

            //LevelLoader = new LevelLoader();
            //LevelLoader.LoadLevel(Path.Combine(ProjectPath.getPath(),
            //    "Breakout", "Assets", "Levels", 
            //    levelSequence[LevelIndex]));

            PUOOrganizer = new PowerUpOrbOrganizer();

            ballOrganizer = new BallOrganizer();
            ballOrganizer.ResetOrganizer();

            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.02f)),
                new Image(Path.Combine(ProjectPath.getPath(),
                "Breakout", "Assets", "Images", "player.png")));

            score = new Score(new Vec2F(0.00f, -0.26f), new Vec2F(0.3f, 0.3f));

            //gameTimer = new BreakoutTimer(
            //    LevelManager.LevelLoader.Meta.Time, new Vec2F(0.33f, -0.26f), 
            //    new Vec2F(0.3f, 0.3f));
        }

        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }

        public void ResetState() {
            //LevelIndex = 0;

            //LevelLoader.LoadLevel(Path.Combine(ProjectPath.getPath(),
            //    "Breakout", "Assets", "Levels", 
            //    levelSequence[LevelIndex]));
            LevelManager.ResetToFirst();

            PUOOrganizer.ResetOrganizer();

            ballOrganizer.ResetOrganizer();

            player.Reset();

            player.Healthbar.Reset();

            score.Reset();  

            //gameTimer.SetNewLevelTime(LevelManager.LevelLoader.Meta.Time);
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
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.ControlEvent, 
                    StringArg1 = "BlockCollision", To = LevelManager.LevelLoader.BlockOrganizer, 
                    ObjectArg1 = block
                });
                //only send to ball if not priorly hit this iteration
                if (!priorHit) {
                    priorHit = true;

                    ball.DirectionBlockSetter(check.CollisionDir);
                }
            }
        }

        private void OrbPlayerCollision(PowerUpOrb orb) {
            CollisionData check = CollisionDetection.Aabb(orb.Shape.AsDynamicShape(), player.Shape);

            if(check.Collision) {
                orb.ApplyEffect();
                orb.DeleteEntity();
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

        /*
        /// <summary>
        /// Load next level if legal, else go to main menu
        /// </summary>
        private void NextLevel() {
            LevelIndex++;
            if (LevelIndex < levelSequence.Count) {
                LevelLoader.LoadLevel(Path.Combine( new string[] { ProjectPath.getPath(),
                    "Breakout", "Assets", "Levels", levelSequence[LevelIndex] }));
                
                ballOrganizer.ResetOrganizer();
                player.Reset();

                //StaticTimer.RestartTimer();
                if (LevelLoader.Meta.Time != 0) {
                    gameTimer.SetNewLevelTime(LevelLoader.Meta.Time);
                }
            }
            else {
                BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                    EventType = GameEventType.GameStateEvent,
                    Message = "CHANGE_STATE",
                    StringArg1 = "GAME_WON" });
            }
        } */

        public void UpdateState() {
            //player move
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });
            
            //ball move
            ballOrganizer.MoveEntities();

            //PowerUpOrb move
            PUOOrganizer.MoveEntities();

            PUOOrganizer.Entities.Iterate(orb => {
                OrbPlayerCollision(orb);
            });

            ballOrganizer.Entities.Iterate(ball => {
                BallPlayerCollision(ball);
            });

            BallBlockCollisionIterate();

            LevelManager.UpdateLevelTimer();

            //if(LevelLoader.LevelEnded()) {
            //    NextLevel();
            //}
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            
            player.Render();
            player.Healthbar.Render();

            ballOrganizer.RenderEntities();
            //LevelLoader.BlockOrganizer.RenderEntities();
            PUOOrganizer.RenderEntities();

            score.Render();

            LevelManager.RenderLevel();
            // if time is not 0 render Timer, else do not render
            //if (LevelManager.LevelLoader.Meta.Time != 0) {
            //    LevelManager.LevelTimer.Render();
            //}
        }

        public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
            if (action == KeyboardAction.KeyPress) {
                switch (key) {
                    case KeyboardKey.A:
                    case KeyboardKey.Left:
                        BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                            EventType = GameEventType.PlayerEvent, Message = "true",
                            StringArg1 = "SetMoveLeft" });
                        break;
                    case KeyboardKey.D:
                    case KeyboardKey.Right:
                        BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                            EventType = GameEventType.PlayerEvent, Message = "true",
                            StringArg1 = "SetMoveRight" });
                        break;
                }
            }
            else if (action == KeyboardAction.KeyRelease) {
                switch (key) {
                    case KeyboardKey.A:
                    case KeyboardKey.Left:
                        BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                            EventType = GameEventType.PlayerEvent, Message = "false",
                            StringArg1 = "SetMoveLeft" });
                        break;
                    case KeyboardKey.D:
                    case KeyboardKey.Right:
                        BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                            EventType = GameEventType.PlayerEvent, Message = "false",
                            StringArg1 = "SetMoveRight" });
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
                    case KeyboardKey.Comma:
                        BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                            EventType = GameEventType.GameStateEvent,
                            Message = "CHANGE_STATE",
                            StringArg1 = "GAME_WON" });
                        break;
                }
            }
        }
    }
}