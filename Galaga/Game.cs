using DIKUArcade;
using DIKUArcade.Timers;
using System.Collections.Generic;
using DIKUArcade.EventBus;
using Galaga.GalagaStates;

namespace Galaga
{
    public class Game : IGameEventProcessor<object> {
        private Window window;
        private GameTimer gameTimer;
        private StateMachine stateMachine;
        public Game() {
            window = new Window("Galaga", 500, 500);
            gameTimer = new GameTimer(60, 60);

            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> { GameEventType.InputEvent,
                GameEventType.PlayerEvent, GameEventType.ControlEvent, GameEventType.GraphicsEvent,
                GameEventType.GameStateEvent});

            window.RegisterEventBus(GalagaBus.GetBus());
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.GraphicsEvent, this);

            stateMachine = new StateMachine();
        }

        public void KeyRelease(string key) {
            switch (key) {
                case "KEY_ESCAPE":
                    window.CloseWindow();
                    break;
                default:
                    break;
            }
        }

        public void ProcessEvent(GameEventType type, GameEvent<object> gameEvent) {
            if (type == GameEventType.InputEvent) {
                switch (gameEvent.Parameter1) {
                    case "KEY_RELEASE":
                        KeyRelease(gameEvent.Message);
                        break;
                    default:
                        break;
                }
            }
        }

        public void Run() {
            while(window.IsRunning()) {
                gameTimer.MeasureTime();

                while (gameTimer.ShouldUpdate()) {

                    window.PollEvents();
                    GalagaBus.GetBus().ProcessEvents();
                    stateMachine.ActiveState.UpdateGameLogic();
                }
                if (gameTimer.ShouldRender()) {

                    window.Clear();
                    stateMachine.ActiveState.RenderState();
                    window.SwapBuffers();

                }
                if (gameTimer.ShouldReset()) {
                    // this update happens once every second
                    window.Title = $"Galaga | (UPS,FPS): ({gameTimer.CapturedUpdates},{gameTimer.CapturedFrames})";  
                }
            }
        }
    }
}