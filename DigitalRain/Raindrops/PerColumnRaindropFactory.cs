using System;

namespace DigitalRain.Raindrops
{
    using Columns;

    public class PerColumnRaindropFactory
    {
        // Static will use the same seed across all class instances.
        private static readonly Random _randomGen = new Random();
        private readonly StandardRaindropFactoryConfig _config;

        public PerColumnRaindropFactory()
        {
            _config = DigitalRainGame.Config.standardRaindropFactory;
        }

        public IRaindrop Create(ColumnSpace space)
        {
            var lifespanRange = (_config.lifespanMax + 1) - _config.lifespanMin;
            var randomLifespan = _config.lifespanMin + (_randomGen.NextDouble() * lifespanRange);
            return new StandardRaindrop(
                space, randomLifespan, symbolColor: StandardRaindrop.DefaultColor
            );
        }
    }
}
