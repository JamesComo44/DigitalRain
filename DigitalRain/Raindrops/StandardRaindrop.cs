using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalRain.Raindrops
{
    class StandardRaindrop : IRaindrop
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
        public int Lifespan { get { return 255; } }
        public double LifeRemaining { get; private set; }
        public static Color DefaultColor { get { return Color.GreenYellow; } }

        private char _symbol;
        private readonly double _decayRate;
        private readonly Vector2 _position;
        private Color _symbolColor;
        private Vector2 _symbolCenter;
        private bool _isDrawingNewSymbol;

        public StandardRaindrop(double decayRate, Vector2 initialPosition, Color symbolColor)
        {
            SymbolPool = SymbolPools.EnglishAlphanumericUppercase();
            Symbol = GetSymbolFromPool();
            LifeRemaining = (double)Lifespan;

            _decayRate = decayRate;
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
                LifeRemaining -= _decayRate * gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        private Color CalculateSymbolColor()
        {
            if (LifeRemaining >= 0)
                _symbolColor.A = (byte)LifeRemaining;
            return _symbolColor;
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

            spriteBatch.DrawString(font, SymbolAsStr(), _position, CalculateSymbolColor(), 0f, _symbolCenter, 1.0f, SpriteEffects.None, 0.5f);

            // DEBUG
            spriteBatch.DrawString(font, _symbolColor.A.ToString(), new Vector2(0, 0), Color.White);
        }
    }
}
