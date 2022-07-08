using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DigitalRain.Raindrops
{
    using Columns;

    public class StandardRaindrop : IRaindrop
    {
        private readonly ColumnSpace _columnSpace;
        private readonly char _symbol;
        public double LifeRemaining { get; private set; }

        private readonly ColorCalculator _colorCalculator;
        private Color _currentColor;

        public StandardRaindrop(ColumnSpace space, char symbol, double lifespan, ColorCalculator colorCalculator)
        {
            _columnSpace = space;
            _symbol = symbol;
            LifeRemaining = lifespan;
            _colorCalculator = colorCalculator;
            _currentColor = _colorCalculator.StartColor;
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
                _currentColor = _colorCalculator.Calculate(LifeRemaining);
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            _columnSpace.DrawString(spriteBatch, font, _symbol.ToString(), _currentColor);
        }
    }
}
