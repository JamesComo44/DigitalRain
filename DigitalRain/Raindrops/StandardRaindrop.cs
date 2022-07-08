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
        private readonly Color _startColor;
        private readonly Color _endColor;
        private Color _currentColor;
        private char _symbol;
        float _lerpTimeElapsed;

        public StandardRaindrop(ColumnSpace space, double lifespan, Color symbolColor, char symbol)
        {
            _columnSpace = space;

            _colorAlphaLsbWeight = lifespan / _maxColorAlpha;
            LifeRemaining = lifespan;

            _startColor = Color.White;
            _endColor = symbolColor;
            _currentColor = _startColor;
            _lerpTimeElapsed = 0;

            Symbol = symbol;
        }

        public bool IsDead()
        {
            return LifeRemaining <= 0;
        }

        readonly float LerpTime = 400;
        public void Update(GameTime gameTime)
        {
            if (!IsDead())
            { 
                _lerpTimeElapsed = (float)Math.Min(_lerpTimeElapsed + gameTime.ElapsedGameTime.TotalMilliseconds, LerpTime);
                LifeRemaining = (float)Math.Max(LifeRemaining - gameTime.ElapsedGameTime.TotalMilliseconds, 0);
                _currentColor = CalculateColor();
            }
        }

        private Color CalculateColor()
        {
            var lerpPercentage = _lerpTimeElapsed / LerpTime;
            Color color = Color.Lerp(_startColor, _endColor, lerpPercentage);

            color.A = (byte)(LifeRemaining / _colorAlphaLsbWeight);
            return color;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            _columnSpace.DrawString(spriteBatch, font, Symbol.ToString(), _currentColor);
        }
    }
}
