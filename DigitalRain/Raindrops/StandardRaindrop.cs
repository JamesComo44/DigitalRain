using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DigitalRain.Raindrops
{
    using Columns;
    public class StandardRaindrop
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
  
        public StandardRaindrop(ColumnSpace space, double lifeSpan, Color symbolColor)
        {
            _columnSpace = space;

            SymbolPool = SymbolPools.EnglishAlphanumericUpperSymbols();
            Symbol = GetSymbolFromPool();

            _colorAlphaLsbWeight = lifeSpan / _maxColorAlpha;
            LifeRemaining = lifeSpan;

            _symbolColor = symbolColor;
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

        // IRaindrop
        public void Update(GameTime gameTime)
        {
            if (LifeRemaining > 0)
            {
                double updatedLife = LifeRemaining - gameTime.ElapsedGameTime.TotalMilliseconds;
                LifeRemaining = System.Math.Max(updatedLife, 0);
                _symbolColor.A = CalculateColorAlpha(LifeRemaining);
            }
        }

        private byte CalculateColorAlpha(double lifeRemaining)
        {
            return (byte)(lifeRemaining / _colorAlphaLsbWeight);
        }

        // IRaindrop
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            _columnSpace.DrawString(spriteBatch, font, SymbolAsStr(), _symbolColor);

            // DEBUG
            //spriteBatch.DrawString(font, _symbolColor.A.ToString(), new Vector2(0, 25), Color.White);
            //spriteBatch.DrawString(font, LifeRemaining.ToString(), new Vector2(0, 50), Color.White);
        }
    }
}
