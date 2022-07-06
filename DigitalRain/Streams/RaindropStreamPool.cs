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
        private readonly UnoccupiedColumnPool _columnPool;

        public RaindropStreamPool(UnoccupiedColumnPool columnPool)
        {
            _config = DigitalRainGame.Config.raindropStreamPool;
            _columnPool = columnPool;
        }

        public RaindropStream Create(float fontHeight)
        {
            var speedInPixelsPerSecond = _randomGen.Next(
                _config.streamFallSpeedMin, _config.streamFallSpeedMax + 1);

            var column = _columnPool.PickOne();

            return new RaindropStream(column, speedInPixelsPerSecond, fontHeight);
        }

        public bool IsLow { get { return _columnPool.IsLow; } }

        public void Restore(ISet<RaindropStream> streams)
        {
            var columnsToRestore = streams.Select((stream) => stream.Column).ToHashSet();
            _columnPool.Restore(columnsToRestore);
        }
    }
}