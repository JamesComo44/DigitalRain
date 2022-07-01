using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Raindrops
{
    using Columns;
    using System;

    public class RaindropStreamFactory
    {
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private static readonly Random _randomGen = new Random();

        private UnoccupiedColumnPool _columnPool;
        private float _streamSpacing;

        public RaindropStreamFactory(SpriteBatch spriteBatch, SpriteFont spriteFont, UnoccupiedColumnPool columnPool, float speedInPixelsPerSecond, int streamSpacing = 0)
        {
            _spriteBatch = spriteBatch;
            _spriteFont = spriteFont;
            _columnPool = columnPool;
            _streamSpacing = streamSpacing;
        }

        public RaindropStream Create()
        {
            int lowSpeed = 100;
            int highSpeed = 400;
            int speedInPixelsPerSecond = _randomGen.Next(lowSpeed, highSpeed + 1);

            var column = _columnPool.PickOne();

            return new RaindropStream(column, column.Width - _streamSpacing, _spriteBatch, _spriteFont, speedInPixelsPerSecond);
        }
    }
}