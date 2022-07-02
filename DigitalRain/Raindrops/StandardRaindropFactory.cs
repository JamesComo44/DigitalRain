using System;

namespace DigitalRain.Raindrops
{
    using Columns;

    public class StandardRaindropFactory
    {
        // Static will use the same seed across all class instances.
        private static readonly Random _randomGen = new Random();
        private readonly StandardRaindropFactoryConfig _config;

        public StandardRaindropFactory(StandardRaindropFactoryConfig config)
        {
            _config = config;
        }

        public StandardRaindrop Create(ColumnSpace space)
        {
            var lifespanRange = (_config.lifespanMax + 1) - _config.lifespanMin;
            var randomLifespan = _config.lifespanMin + (_randomGen.NextDouble() * lifespanRange);
            return new StandardRaindrop(
                space, randomLifespan, symbolColor: StandardRaindrop.DefaultColor
            );
        }
    }
}
