using System;

namespace Breakout.Blocks {
    public enum PowerUpTypes {
        ExtraLife,
        ExtraBall,
        ExtraPoints
    }

    public static class PowerUpRandom {
        private static Array types = Enum.GetValues(typeof(PowerUpTypes));

        private static Random rand = new Random();

        public static PowerUpTypes RandomType() {
            return (PowerUpTypes) types.GetValue(rand.Next(types.Length));
        }
    }
}