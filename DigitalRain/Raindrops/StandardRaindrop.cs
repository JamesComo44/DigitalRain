using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DigitalRain.Raindrops
{
    using Columns;

    public class StandardRaindrop : IRaindrop
    {
        // IRaindrop
        public char[] SymbolPool { get; private set; }
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

        public StandardRaindrop(ColumnSpace space, double lifeSpan, Color symbolColor)
            : this(space, lifeSpan, symbolColor, SymbolPools.EnglishAlphanumericUpperSymbols())
        { }

        public StandardRaindrop(ColumnSpace space, double lifeSpan, Color symbolColor, char[] symbolPool)
        {
            _columnSpace = space;

            SymbolPool = symbolPool;
            Symbol = GetSymbolFromPool();

            _colorAlphaLsbWeight = lifeSpan / _maxColorAlpha;
            LifeRemaining = lifeSpan;

            _initialSymbolColor = symbolColor;
            _symbolColor = symbolColor;
            _startColor = Color.White;
        }

        // IRaindrop
        public bool IsDead()
        {
            return LifeRemaining <= 0;
        }

        // IRaindrop
        public string SymbolAsStr()
        {
            return Symbol.ToString();
        }

        // IRaindrop
        public char GetSymbolFromPool()
        {
            Random randomGen = new Random();
            int index = randomGen.Next(SymbolPool.Length);
            return SymbolPool[index];
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
            _columnSpace.DrawString(spriteBatch, font, SymbolAsStr(), _symbolColor);
        }
    }
}
