using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Raindrops
{
    using Columns;

    public class RaindropStreamFactory
    {
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private UnoccupiedColumnPool _columnPool;
        private float _speedInPixelsPerSecond;
        private float _streamSpacing;

        public RaindropStreamFactory(SpriteBatch spriteBatch, SpriteFont spriteFont, UnoccupiedColumnPool columnPool, float speedInPixelsPerSecond, int streamSpacing = 0)
        {
            _spriteBatch = spriteBatch;
            _spriteFont = spriteFont;
            _columnPool = columnPool;
            _speedInPixelsPerSecond = speedInPixelsPerSecond;
            _streamSpacing = streamSpacing;
        }

        public RaindropStream Create()
        {
            var column = _columnPool.PickOne();
            return new RaindropStream(column, column.Width - _streamSpacing, _spriteBatch, _spriteFont, _speedInPixelsPerSecond);
        }
    }
}