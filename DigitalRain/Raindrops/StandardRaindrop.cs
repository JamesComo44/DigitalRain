using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DigitalRain.Raindrops
{
    using Columns;

    public class StandardRaindrop : IRaindrop
    {
        public char Symbol
        {
            get { return _symbol; }
            set {_symbol = value; }
        }
        public double LifeRemaining { get; private set; }
        public static Color DefaultColor { get { return Color.GreenYellow; } }

        private const int _maxColorAlpha = 255;
        private readonly double _colorAlphaLsbWeight;

        private readonly ColumnSpace _columnSpace;
        private char _symbol;
        private Color _symbolColor;
        private Color _initialSymbolColor;
        private Color _startColor;

        public StandardRaindrop(ColumnSpace space, double lifespan, Color symbolColor, char symbol)
        {
            _columnSpace = space;

            Symbol = symbol;

            _colorAlphaLsbWeight = lifespan / _maxColorAlpha;
            LifeRemaining = lifespan;

            _initialSymbolColor = symbolColor;
            _symbolColor = symbolColor;
            _startColor = Color.White;
        }

        public bool IsDead()
        {
            return LifeRemaining <= 0;
        }

        float lerpScaleFactor = 350;
        float lerpAmount = 400;
        public void Update(GameTime gameTime)
        {
            if (LifeRemaining > 0)
            {
                if (lerpAmount > 0)
                    lerpAmount -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                double updatedLife = LifeRemaining - gameTime.ElapsedGameTime.TotalMilliseconds;
                LifeRemaining = System.Math.Max(updatedLife, 0);
                _symbolColor = CalculateColor();
            }
        }

        private Color CalculateColor()
        {
            float t = lerpAmount / lerpScaleFactor;
            Color lerpColor = Color.Lerp(_initialSymbolColor, _startColor, t);

            lerpColor.A = (byte)(LifeRemaining / _colorAlphaLsbWeight);
            return lerpColor;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            _columnSpace.DrawString(spriteBatch, font, Symbol.ToString(), _symbolColor);
        }
    }
}
