using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalRain.Raindrops
{
    class StandardRaindrop
    {
        // IRaindrop
        public char[] SymbolPool { get; private set; }
        public char Symbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;
                _isDrawingNewSymbol = true;
            }
        }
        public double LifeRemaining { get; private set; }
        public static Color DefaultColor { get { return Color.GreenYellow; } }

        private const int _maxColorAlpha = 255;
        private readonly double _colorAlphaLsbWeight;

        private char _symbol;
        private readonly Vector2 _position;
        private Color _symbolColor;
        private Vector2 _symbolCenter;
        private bool _isDrawingNewSymbol;

        public StandardRaindrop(double lifeSpan, Vector2 initialPosition, Color symbolColor)
        {
            SymbolPool = SymbolPools.EnglishAlphanumericUppercase();
            Symbol = GetSymbolFromPool();

            _colorAlphaLsbWeight = lifeSpan / _maxColorAlpha;
            LifeRemaining = lifeSpan;

            _position = initialPosition;

            _symbolColor = symbolColor;
            _symbolCenter = Vector2.Zero;
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
            if (_isDrawingNewSymbol)
            {
                Vector2 newCenter = font.MeasureString(SymbolAsStr());
                _symbolCenter = new Vector2(newCenter.X / 2, 0);
                _isDrawingNewSymbol = false;
            }

            spriteBatch.DrawString(font, SymbolAsStr(), _position, _symbolColor, 0f, _symbolCenter, 1.0f, SpriteEffects.None, 0.5f);

            // DEBUG
            spriteBatch.DrawString(font, _symbolColor.A.ToString(), new Vector2(0, 25), Color.White);
            spriteBatch.DrawString(font, LifeRemaining.ToString(), new Vector2(0, 50), Color.White);
        }
    }
}
