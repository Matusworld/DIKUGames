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

using Breakout.LevelLoading;
using Breakout.Blocks;

namespace Breakout.States {
    public class GameRunning : IGameState {
        private static GameRunning instance;
        private Entity backGroundImage;
        private Player player;
        private Ball ball;
        private LevelLoader levelLoader;
        private List<string> levelSequence;
        private int levelIndex;

        private Score score;
        
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
            
            ball = new Ball(
                new DynamicShape (new Vec2F(0.45f, 0.5f), new Vec2F(0.025f,0.025f)),
                new Image (Path.Combine(ProjectPath.getPath(),  
                "Breakout", "Assets", "Images", "ball.png")),
                (float) Math.PI /4f);
            

            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.02f)),
                new Image(Path.Combine(ProjectPath.getPath(),
                "Breakout", "Assets", "Images", "player.png")));

            score = new Score(new Vec2F(0.06f, -0.25f), new Vec2F(0.3f, 0.3f));
            
            levelSequence = new List<string> { "level1.txt", "level2.txt", "level3.txt",
                "central-mass.txt", "columns.txt", "wall.txt" };
            levelIndex = 0;

            levelLoader = new LevelLoader();
            levelLoader.LoadLevel(Path.Combine(ProjectPath.getPath(),
                "Breakout", "Assets", "Levels", 
                levelSequence[levelIndex]));

            BreakoutBus.GetBus().Subscribe(GameEventType.PlayerEvent, player); 
            BreakoutBus.GetBus().Subscribe(GameEventType.MovementEvent, ball);
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, score);
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, ball);
        }

        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }

        /// <summary>
        /// Render blocks from a EntityContainer with blocks, to fix problem of blocks still being
        /// render even though they are deleted.
        /// </summary>
        /// <param name="blocks"></param>
        private void renderBlocks(EntityContainer<Block> blocks) {
            blocks.Iterate(block => {
                block.RenderEntity();
            });
        }

        private void BallPlayerCollision() {
            CollisionData check = CollisionDetection.Aabb(ball.Shape.AsDynamicShape(),
                player.Shape.AsDynamicShape());
            if (check.Collision) {
                float hitPosition = PlayerHitPositionTruncator(PlayerHitPosition());
                
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.MovementEvent, Message = hitPosition.ToString(), 
                    StringArg1 = "PlayerCollision"
                });
            }
        }

        private void BallBlockCollision(Block block) {
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
                        StringArg1 = "BlockCollision", Message = "UpDown"
                        });
                        break;
                    case CollisionDirection.CollisionDirLeft:
                    case CollisionDirection.CollisionDirRight:
                        BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                        EventType = GameEventType.MovementEvent, 
                        StringArg1 = "BlockCollision", Message = "LeftRight"
                        });
                        break;
                }
            }
        }

        private float PlayerHitPosition(){
            float numerator = ball.Shape.Position.X + ball.Shape.Extent.X - player.Shape.Position.X;
            float denominator = player.Shape.Extent.X + ball.Shape.Extent.X; 

            return numerator / denominator;
            //return (ball.Shape.Position.X + ball.Shape.Extent.X/2f - player.Shape.Position.X)/
            //    (player.Shape.Extent.X);
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
            levelIndex++;
            if (levelIndex < levelSequence.Count) {
                levelLoader.LoadLevel(Path.Combine( new string[] { ProjectPath.getPath(),
                    "Breakout", "Assets", "Levels", levelSequence[levelIndex] }));
            }
            else {
                BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                    EventType = GameEventType.GameStateEvent,
                    Message = "CHANGE_STATE",
                    StringArg1 = "GAME_MAINMENU" });
            }
        }

        public void UpdateState() {
            //player move
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });
            
            //ball move
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.MovementEvent, StringArg1 = "Move" });
            BallPlayerCollision();
            levelLoader.Blocks.Iterate(block => {
                BallBlockCollision(block);
            });

            if(levelLoader.LevelEnded()) {
                NextLevel();
            }
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            player.Render();
            ball.RenderEntity();
            renderBlocks(levelLoader.Blocks);
            score.RenderScore();
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
                    // To visually test that blocks get deleted when they are 'hit'
                    /*case KeyboardKey.Space:
                        levelLoader.Blocks.Iterate(block => {
                            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                                EventType = GameEventType.ControlEvent, StringArg1 = "Damage",
                                To = block});
                        });
                        break;*/
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