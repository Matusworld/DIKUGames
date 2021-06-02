using System;

namespace Breakout.GamePlay.PowerUpOrbEntity {
        /// <summary>
        /// Enables random draws from PowerUpTypes.
        /// </summary>
        public static class PowerUpRandom {
        private static Array types = Enum.GetValues(typeof(PowerUpTypes));

        private static Random rand = new Random();

        /// <summary>
        /// Draw one random type from PowerUpTypes with uniform probability.
        /// </summary>
        /// <returns>The drawn PowerUpType.</returns>
        public static PowerUpTypes RandomType() {
            return (PowerUpTypes) types.GetValue(rand.Next(types.Length));
        }
    }
}