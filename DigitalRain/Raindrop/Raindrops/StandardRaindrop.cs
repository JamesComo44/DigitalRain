using Microsoft.Xna.Framework;
using System;

namespace DigitalRain.Raindrop.Raindrops
{
    using Grid;
    using Config;

    public class StandardRaindrop : IRaindrop
    {
        public GridCoordinates Coordinates { get; private set; }
        public double Lifespan { get; private set; }
        private double _lifeRemaining;

        public ColorCalculator ColorCalculator { get; set; }

        public Color Color { get; private set; }
        public string Symbol { get; set; }

        public StandardRaindrop(GridCoordinates coordinates, char symbol, double lifespan, ColorCalculator colorCalculator)
        {
            Coordinates = coordinates;
            Symbol = symbol.ToString();
            Lifespan = lifespan;
            _lifeRemaining = Lifespan;
            ColorCalculator = colorCalculator;
            Color = ColorCalculator.StartColor;
        }

        public bool IsDead
        {
            get
            {
                return _lifeRemaining <= 0;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!IsDead)
            {
                _lifeRemaining = (float)Math.Max(_lifeRemaining - gameTime.ElapsedGameTime.TotalMilliseconds, 0);
                Color = ColorCalculator.Calculate(_lifeRemaining);
            }
        }
    }
}
