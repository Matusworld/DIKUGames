using DIKUArcade.State;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Input;
using System.IO;
using Breakout.LevelLoading;
using Breakout.Blocks;

namespace Breakout.States {
    public class GameRunning : IGameState {
        private Player player;
        private LevelLoader levelloader;

        public GameRunning() {
            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.02f)),
                new Image(Path.Combine("Assets", "Images", "player.png")));

            levelloader = new LevelLoader(Path.Combine("Assets", "Levels", "level3.txt"));
            levelloader.LoadLevel();

            BreakoutBus.GetBus().Subscribe(GameEventType.PlayerEvent, player); 
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
        }

        public void UpdateState() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });
        }

        public void RenderState() {
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
                    default:
                        break;
                }
            }
        }
    }
}