using System;
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

using Breakout.LevelLoading;
using Breakout.Blocks;
using Breakout.PlayerEntity;

namespace Breakout.States {
    public class GameRunning : IGameState {
        private static GameRunning instance;
        private Entity backGroundImage;
        private Player player;
        private BallOrganizer ballOrganizer;
        private PowerUpOrganizer PUorganizer;
        public LevelLoader LevelLoader { get; private set; }
        private List<string> levelSequence;
        public int LevelIndex { get; private set; }
        private Score score;
        private Timer gameTimer;
        
        public GameRunning() {
            Init();
        }

        private void Init() {
            //Initialize backGroundImage
            Vec2F imagePos = new Vec2F(0f,0f);
            Vec2F imageExtent = new Vec2F(1f, 1f);
            StationaryShape shape = new StationaryShape(imagePos, imageExtent);
            IBaseImage image = new Image(Path.Combine( ProjectPath.getPath(),
                "Breakout", "Assets", "Images", "SpaceBackground.png"));
            backGroundImage = new Entity(shape, image);
                        
            levelSequence = new List<string> { "level1.txt", "level2.txt", "level3.txt",
                "central-mass.txt", "columns.txt", "wall.txt" };
            LevelIndex = 0;

            LevelLoader = new LevelLoader();
            LevelLoader.LoadLevel(Path.Combine(ProjectPath.getPath(),
                "Breakout", "Assets", "Levels", 
                levelSequence[LevelIndex]));

            PUorganizer = new PowerUpOrganizer();

            ballOrganizer = new BallOrganizer();
            //BreakoutBus.GetBus().RegisterEvent ( new GameEvent {
            //    EventType = GameEventType.ControlEvent, StringArg1 = "BallGained" });
            ballOrganizer.AddBall();

            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.02f)),
                new Image(Path.Combine(ProjectPath.getPath(),
                "Breakout", "Assets", "Images", "player.png")));

            score = new Score(new Vec2F(0.00f, -0.26f), new Vec2F(0.3f, 0.3f));

            gameTimer = new Timer(LevelLoader.Meta.Time, new Vec2F(0.33f, -0.26f), new Vec2F(0.3f, 0.3f));
        }

        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }

        /// <summary>
        /// Render blocks from a EntityContainer with blocks, to fix problem of blocks still being
        /// render even though they are deleted.
        /// </summary>
        /// <param name="blocks"></param>
        private void renderEntityContainer(EntityContainer<Entity> entities) {
            entities.Iterate(entity => {
                entity.RenderEntity();
            });
        }

        private void BallPlayerCollision(Ball ball) {
            CollisionData check = CollisionDetection.Aabb(ball.Shape.AsDynamicShape(),
                player.Shape.AsDynamicShape());
            if (check.Collision) {
                float hitPosition = PlayerHitPositionTruncator(PlayerHitPosition(ball));
                
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.MovementEvent, Message = hitPosition.ToString(), 
                    StringArg1 = "PlayerCollision", To = ball
                });
            }
        }

        private void BallBlockCollision(Block block, Ball ball) {
            CollisionData check = CollisionDetection.Aabb(ball.Shape.AsDynamicShape(), block.Shape);
            if (check.Collision) {
                //to block
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.ControlEvent, 
                    StringArg1 = "BlockCollision", To = block
                });
                switch(check.CollisionDir) {
                    case CollisionDirection.CollisionDirDown:
                    case CollisionDirection.CollisionDirUp:
                        BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                            EventType = GameEventType.MovementEvent, 
                            StringArg1 = "BlockCollision", Message = "UpDown", To = ball
                        });
                        break;
                    case CollisionDirection.CollisionDirLeft:
                    case CollisionDirection.CollisionDirRight:
                        BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                            EventType = GameEventType.MovementEvent, 
                            StringArg1 = "BlockCollision", Message = "LeftRight", To = ball
                        });
                        break;
                }
            }
        }

        private void OrbPlayerCollision(PowerUpOrb orb) {
            CollisionData check = CollisionDetection.Aabb(orb.Shape.AsDynamicShape(), player.Shape);

            if(check.Collision) {
                switch(orb.Type) {
                    case PowerUpTypes.ExtraLife:
                        BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                            EventType = GameEventType.ControlEvent, StringArg1 = "HealthGained"
                        });
                        break;
                    case PowerUpTypes.ExtraBall:
                        ballOrganizer.AddBall();
                        break;
                    case PowerUpTypes.ExtraPoints:
                        BreakoutBus.GetBus().RegisterEvent ( new GameEvent {
                            EventType = GameEventType.ControlEvent, StringArg1 = "PowerUpScore"
                        });
                        break;
                }
                orb.DeleteEntity();
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
        public void ResetState() {
            Init();
        }

        /// <summary>
        /// Load next level if legal, else go to main menu
        /// </summary>
        private void NextLevel() {
            LevelIndex++;
            if (LevelIndex < levelSequence.Count) {
                LevelLoader.LoadLevel(Path.Combine( new string[] { ProjectPath.getPath(),
                    "Breakout", "Assets", "Levels", levelSequence[LevelIndex] }));
                
                StaticTimer.RestartTimer();
                if (LevelLoader.Meta.Time != 0) {
                    gameTimer.NewTimer(LevelLoader.Meta.Time);
                }
            }
            else {
                BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                    EventType = GameEventType.GameStateEvent,
                    Message = "CHANGE_STATE",
                    StringArg1 = "GAME_WON" });
            }
        }
        //Name is up for debate
        public void CheckLifeLost() {
            if (ballOrganizer.Balls.CountEntities() == 0 ) {
                /* BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                        EventType = GameEventType.ControlEvent,
                        StringArg1 = "HealthLost" }); */
                player.Hpbar.HealthLost();
                //damage via event will never happen instantaneously
                if (player.Hpbar.Lives > 0) {
                    ballOrganizer.AddBall();
                } else {
                    BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                        EventType = GameEventType.GameStateEvent,
                        Message = "CHANGE_STATE",
                        StringArg1 = "GAME_LOST"
                    });
                }
            }
        }

        public void UpdateState() {
            //player move
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });
            
            //ball move
            ballOrganizer.Balls.Iterate(ball => {
                BallPlayerCollision(ball);
                BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                    EventType = GameEventType.MovementEvent, StringArg1 = "Move" , To = ball });
            });

            //PowerUpOrb move
            PUorganizer.MoveOrbs();

            PUorganizer.Orbs.Iterate(orb => {
                OrbPlayerCollision(orb);
            });

            LevelLoader.Blocks.Iterate(block => {
                ballOrganizer.Balls.Iterate(ball => {
                    BallBlockCollision(block, ball);
                });
            });
            
            CheckLifeLost();

            if (LevelLoader.Meta.Time != 0) {
                gameTimer.UpdateTimer();

                if (gameTimer.TimeRunOut()) {
                    BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                        EventType = GameEventType.GameStateEvent, Message = "CHANGE_STATE",
                        StringArg1 = "GAME_LOST"
                    });
                }
            }

            if(LevelLoader.LevelEnded()) {
                NextLevel();
            }
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            player.Render();
            player.Hpbar.Render();
            ballOrganizer.RenderBalls();
            LevelLoader.Blocks.Iterate(block => {
                block.RenderEntity();
            });
            PUorganizer.RenderOrbs();
            score.Render();

            // if time is not 0 render Timer else do not render
            if (LevelLoader.Meta.Time != 0) {
                gameTimer.Render();
            }
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
                    default:
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
                        NextLevel();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}