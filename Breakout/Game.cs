using System.IO;
using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Input;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Breakout.LevelLoading;
using Breakout.Blocks;
using Breakout.States;

namespace Breakout {
    public class Game : DIKUGame, IGameEventProcessor {
        
        //private Player player;

        //private LevelLoader levelloader;

        private StateMachine stateMachine;

        public Game(WindowArgs winArgs) : base(winArgs) {
            window.SetKeyEventHandler(KeyHandler);
            window.SetClearColor(System.Drawing.Color.DarkGray);

            //player = new Player(
            //    new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.02f)),
            //    new Image(Path.Combine("Assets", "Images", "player.png")));


            //levelloader = new LevelLoader(Path.Combine("Assets", "Levels", "level3.txt"));
            //levelloader.LoadLevel();

            stateMachine = new StateMachine();

            BreakoutBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.WindowEvent, GameEventType.PlayerEvent, GameEventType.ControlEvent,
                GameEventType.GameStateEvent } );
            BreakoutBus.GetBus().Subscribe(GameEventType.WindowEvent, this);  
        }

        //KeyHandler sends singal on to ActiveState of StateMachine
        private void KeyHandler(KeyboardAction action, KeyboardKey key) { 
            if (action == KeyboardAction.KeyRelease && key == KeyboardKey.Escape) {
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.WindowEvent, Message = "CLOSE_WINDOW" });
            }
            else {
                stateMachine.ActiveState.HandleKeyEvent(action, key);
            }
        }
        /*
        private void KeyHandler(KeyboardAction action, KeyboardKey key) {
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
                    case KeyboardKey.Escape:
                        BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                            EventType = GameEventType.WindowEvent, Message = "CLOSE_WINDOW"
                        } );
                        break;
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
        */

        /*
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
        */

        public override void Render() {
            stateMachine.ActiveState.RenderState();
            /*
            player.Render();
            renderBlocks(levelloader.Blocks);
            */
        }

        public override void Update() {
            stateMachine.ActiveState.UpdateState();

            BreakoutBus.GetBus().ProcessEvents();

            /*
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });
            */
        }

        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.WindowEvent) {
                if (gameEvent.Message == "CLOSE_WINDOW") {
                    window.CloseWindow();
                }
            }
        }
    }
}