using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Raindrops
{
    using Columns;

    public class RaindropStream
    {
        private Column _column;
        private float _width;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private double? _startTimeInSeconds;
        private float _speedInPixelsPerSecond;
        List<StandardRaindrop> _raindrops;

        public RaindropStream(Column column, float width, SpriteBatch spriteBatch, SpriteFont spriteFont, float speedInPixelsPerSecond)
        {
            _column = column;
            _width = width;
            _spriteBatch = spriteBatch;
            _spriteFont = spriteFont;
            _speedInPixelsPerSecond = speedInPixelsPerSecond;
            _raindrops = new List<StandardRaindrop>();
        }

        public void Update(GameTime gameTime)
        {
            if (_startTimeInSeconds == null)
            {
                _startTimeInSeconds = gameTime.TotalGameTime.TotalSeconds;
            }

            var timeElapsedInSeconds = gameTime.TotalGameTime.TotalSeconds - _startTimeInSeconds;
            var distanceFallenInPixels = _speedInPixelsPerSecond * timeElapsedInSeconds;
 
            while (ThereIsRoomLeftToFall((double)distanceFallenInPixels))
            {
                double lifeSpan = 10000.0;
                Color color = StandardRaindrop.DefaultColor;
                var position = new Vector2(_column.PositionX, StreamHeight);
                _raindrops.Add(new StandardRaindrop(lifeSpan, position, color));
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var raindrop in _raindrops)
            {
                raindrop.Draw(_spriteBatch, _spriteFont);
            }
        }

        private float RaindropCount { get { return _raindrops.Count; } }
        // ASSUMPTION: Monospace font (all characters same height)
        private float CharacterHeight { get { return _spriteFont.MeasureString("A").Y; } }
        private float StreamHeight { get { return RaindropCount * CharacterHeight; } }
        private bool ThereIsRoomLeftToFall(double distanceFallenInPixels)
        {
            return StreamHeight + CharacterHeight < distanceFallenInPixels;
        }
    }
}