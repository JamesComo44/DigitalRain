using System;   

namespace DigitalRain.Config
{
    using Grid;
    using Raindrop;

    public class RaindropStreamFactory
    {
        private static readonly Random _randomGen = new Random();

        private readonly RaindropStreamFactoryConfig _config;
        private IRaindropFactory _raindropFactory;

        public RaindropStreamFactory(IRaindropFactory raindropFactory)
        {
            _config = DigitalRainGame.Config.raindropStreamFactory;
            SetRaindropFactory(raindropFactory);
        }

        public void SetRaindropFactory(IRaindropFactory raindropFactory)
        {
            _raindropFactory = raindropFactory;
        }

        public RaindropStream Create(Column column, float fontHeight)
        {
            var speedInPixelsPerSecond = _randomGen.Next(
                _config.streamFallSpeedMin, _config.streamFallSpeedMax + 1);

            return new RaindropStream(column, _raindropFactory, speedInPixelsPerSecond, fontHeight);
        }
    }
}