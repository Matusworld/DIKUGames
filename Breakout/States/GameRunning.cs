using DIKUArcade.State;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Input;
using System.IO;
using System.Collections.Generic;
using Breakout.LevelLoading;
using Breakout.Blocks;

namespace Breakout.States {
    public class GameRunning : IGameState {
        private static GameRunning instance;
        private Entity backGroundImage;
        private Player player;
        private LevelLoader levelloader;
        private List<string> levelSequence;
        private int levelIndex;
        
        public GameRunning() {
            //Initialize backGroundImage
            Vec2F imagePos = new Vec2F(0f,0f);
            Vec2F imageExtent = new Vec2F(1f, 1f);
            StationaryShape shape = new StationaryShape(imagePos, imageExtent);
            IBaseImage image = new Image(Path.Combine( ProjectPath.getPath(),
                "Assets", "Images", "SpaceBackground.png"));
            backGroundImage = new Entity(shape, image);

            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.02f)),
                new Image(Path.Combine("Assets", "Images", "player.png")));

            levelloader = new LevelLoader(Path.Combine("Assets", "Levels", "level1.txt"));
            levelloader.LoadLevel();

            levelSequence = new List<string> { "level1.txt", "level2.txt", "level3.txt",
                "central-mass.txt", "columns.txt", "wall.txt" };
            levelIndex = 0;

            BreakoutBus.GetBus().Subscribe(GameEventType.PlayerEvent, player); 
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

        public void ResetState() {
            GameRunning.instance = new GameRunning();
        }

        public void UpdateState() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            player.Render();
            renderBlocks(levelloader.Blocks);
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
                    case KeyboardKey.Space:
                        levelloader.Blocks.Iterate(block => {
                            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                                EventType = GameEventType.ControlEvent, StringArg1 = "Damage",
                                To = block});
                        });
                        break;
                    case KeyboardKey.P:
                        BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                            EventType = GameEventType.GameStateEvent,
                            Message = "CHANGE_STATE",
                            StringArg1 = "GAME_PAUSED" });
                            break;
                    case KeyboardKey.Plus:
                        levelIndex++;
                        if (levelIndex < levelSequence.Count) {
                            levelloader = new LevelLoader(Path.Combine(
                                "Assets", "Levels", levelSequence[levelIndex]));
                            levelloader.LoadLevel();
                        }
                        else {
                            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                                EventType = GameEventType.GameStateEvent,
                                Message = "CHANGE_STATE",
                                StringArg1 = "GAME_MAINMENU" });
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}