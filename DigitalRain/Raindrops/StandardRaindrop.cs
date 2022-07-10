using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DigitalRain.Raindrops
{
    using Columns;

    public class StandardRaindrop : IRaindrop
    {
        public GridCoordinates Coordinates { get; private set; }
        public double LifeRemaining { get; private set; }

        private readonly ColorCalculator _colorCalculator;

        public Color Color { get; private set; }
        public string Symbol { get; private set; }

        public StandardRaindrop(GridCoordinates coordinates, char symbol, double lifespan, ColorCalculator colorCalculator)
        {
            Coordinates = coordinates;
            Symbol = symbol.ToString();
            LifeRemaining = lifespan;
            _colorCalculator = colorCalculator;
            Color = _colorCalculator.StartColor;
        }

        public bool IsDead
        {
            get
            {
                return LifeRemaining <= 0;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!IsDead)
            {
                LifeRemaining = (float)Math.Max(LifeRemaining - gameTime.ElapsedGameTime.TotalMilliseconds, 0);
                Color = _colorCalculator.Calculate(LifeRemaining);
            }
        }
    }
}
