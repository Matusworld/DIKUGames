using System.IO;
using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Input;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout {
    public class Game : DIKUGame, IGameEventProcessor {
        
        private Player player;
        private Block block1;

        private LevelLoader levelloader;

        private EntityContainer<Block> blocks;

        public Game(WindowArgs winArgs) : base(winArgs) {
            window.SetKeyEventHandler(KeyHandler);
            window.SetClearColor(System.Drawing.Color.DarkGray);

            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.02f)),
                new Image(Path.Combine("Assets", "Images", "player.png")));


            levelloader = new LevelLoader(Path.Combine("Assets", "Levels", "level3.txt"));

            blocks = levelloader.ReadFile();

            BreakoutBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.WindowEvent, GameEventType.PlayerEvent } );
            BreakoutBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
            BreakoutBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
            
        }

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
                    default:
                        break;
                }
            }
        }

        public override void Render() {
            player.Render();
            blocks.RenderEntities();
        }

        public override void Update() {
            BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                EventType = GameEventType.PlayerEvent, StringArg1 = "Move" });

            BreakoutBus.GetBus().ProcessEvents();
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