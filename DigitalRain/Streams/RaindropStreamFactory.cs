using System.Collections.Generic;
using System.Linq;

namespace DigitalRain.Raindrops
{
    using Columns;
    using System;

    public class RaindropStreamFactory
    {
        private static readonly Random _randomGen = new Random();

        private readonly RaindropStreamFactoryConfig _config;
        private readonly IRaindropFactory _raindropFactory;

        public RaindropStreamFactory(IRaindropFactory raindropFactory)
        {
            _config = DigitalRainGame.Config.raindropStreamFactory;
            _raindropFactory = raindropFactory;
        }

        public RaindropStream Create(float fontHeight, Column column)
        {
            var speedInPixelsPerSecond = _randomGen.Next(
                _config.streamFallSpeedMin, _config.streamFallSpeedMax + 1);

            return new RaindropStream(_raindropFactory, column, speedInPixelsPerSecond, fontHeight);
        }
    }
}