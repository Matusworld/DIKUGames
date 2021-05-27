using System;

namespace Breakout.GamePlay.PowerUpOrbEntity {
    public enum PowerUpTypes {
        ExtraLife,
        ExtraBall,
        ExtraPoints,
        HalfSpeed,
        DoubleSpeed
    }

    public static class PowerUpRandom {
        private static Array types = Enum.GetValues(typeof(PowerUpTypes));

        private static Random rand = new Random();

        public static PowerUpTypes RandomType() {
            return (PowerUpTypes) types.GetValue(rand.Next(types.Length));
        }
    }
}