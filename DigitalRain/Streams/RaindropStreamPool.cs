using System.Collections.Generic;
using System.Linq;

namespace DigitalRain.Raindrops
{
    using Columns;
    using System;

    public class RaindropStreamPool
    {
        private static readonly Random _randomGen = new Random();

        private readonly RaindropStreamPoolConfig _config;
        private readonly IRaindropFactory _raindropFactory;

        public RaindropStreamPool(IRaindropFactory raindropFactory)
        {
            _config = DigitalRainGame.Config.raindropStreamPool;
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