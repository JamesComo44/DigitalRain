using System.Collections.Generic;
using System.Linq;

namespace DigitalRain.Raindrops
{
    using Columns;
    using System;

    public class RaindropStreamPool
    {
        private static readonly Random _randomGen = new Random();

        private RaindropStreamPoolConfig _config;
        private StandardRaindropFactory _raindropFactory;
        private UnoccupiedColumnPool _columnPool;
        private float _streamSpacing;

        public RaindropStreamPool(RaindropStreamPoolConfig config, UnoccupiedColumnPool columnPool, StandardRaindropFactory raindropFactory, int streamSpacing = 0)
        {
            _config = config;
            _raindropFactory = raindropFactory;
            _columnPool = columnPool;
            _streamSpacing = streamSpacing;
        }

        public RaindropStream Create(float fontHeight)
        {
            var speedInPixelsPerSecond = _randomGen.Next(
                _config.streamFallSpeedMin, _config.streamFallSpeedMax + 1);

            var column = _columnPool.PickOne();

            return new RaindropStream(_raindropFactory, column, column.Width - _streamSpacing, speedInPixelsPerSecond, fontHeight);
        }

        public bool IsLow { get { return _columnPool.IsLow; } }

        public void Restore(ISet<RaindropStream> streams)
        {
            var columnsToRestore = streams.Select((stream) => stream.Column).ToHashSet();
            _columnPool.Restore(columnsToRestore);
        }
    }
}