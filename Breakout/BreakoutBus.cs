using DIKUArcade.Events;

namespace Breakout {
    public static class BreakoutBus {
        private static GameEventBus eventBus;
        
        /// <summary>
        /// Number of ms to wait before checking an EntityContainer.
        /// Time delay gives deleted item time to be swept.
        /// </summary>
        public static int CountDelay = 5;

        public static GameEventBus GetBus() {
            return BreakoutBus.eventBus ?? (BreakoutBus.eventBus = new GameEventBus());
        }
    }
}