using System;
using Microsoft.Xna.Framework;

namespace DigitalRain.Raindrop.Raindrops
{
    using Grid;
    using Config;

    public class GlitchedRaindrop // TODO : IRaindrop
    {
        private static readonly Random _randomGen = new Random();

        private readonly GridCoordinates _coordinates;
        private readonly char _symbol;

        private readonly double _lifespan;
        public double LifeRemaining { get; private set; }

        private readonly ColorCalculator _colorCalculator;
        private Color _currentColor;

        private readonly int _glitchSpeedFactor;

        public GlitchedRaindrop(GridCoordinates coordinates, char symbol, double lifespan, ColorCalculator colorCalculator)
        {
            _coordinates = coordinates;
            _symbol = symbol;
            _lifespan = lifespan;
            LifeRemaining = lifespan;
            _colorCalculator = colorCalculator;
            _currentColor = _colorCalculator.StartColor;
            _glitchSpeedFactor = _randomGen.Next(5, 15);
        }

        public bool IsDead()
        {
            return LifeRemaining <= 0;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsDead())
            {
                LifeRemaining = (float)Math.Max(LifeRemaining - gameTime.ElapsedGameTime.TotalMilliseconds, 0);

                var totalElapsedTime = _lifespan - LifeRemaining;
                var glitchedElapsedTime = (totalElapsedTime * _glitchSpeedFactor) % (_lifespan + 1);
                var glitchedTimeRemaining = _lifespan - glitchedElapsedTime;
                _currentColor = _colorCalculator.Calculate(glitchedTimeRemaining);
            }
        }
    }
}
