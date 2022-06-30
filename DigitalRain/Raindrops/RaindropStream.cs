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
        private int _raindropCount;
        List<StandardRaindrop> _raindrops;

        public RaindropStream(Column column, float width, SpriteBatch spriteBatch, SpriteFont spriteFont, float speedInPixelsPerSecond)
        {
            _column = column;
            _width = width;
            _spriteBatch = spriteBatch;
            _spriteFont = spriteFont;
            _speedInPixelsPerSecond = speedInPixelsPerSecond;
            _raindrops = new List<StandardRaindrop>();
            _raindropCount = 0;  // This climbs forever, whereas the size of the list above shrinks as we delete things.
        }

        public void Update(GameTime gameTime)
        {
            RecordStartTime(gameTime);
            Fall(gameTime);

            foreach (var raindrop in _raindrops)
            {
                raindrop.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var raindrop in _raindrops)
            {
                raindrop.Draw(_spriteBatch, _spriteFont);
            }
        }

        private void RecordStartTime(GameTime gameTime)
        {
            if (_startTimeInSeconds == null)
            {
                _startTimeInSeconds = gameTime.TotalGameTime.TotalSeconds;
            }
        }

        private void Fall(GameTime gameTime)
        {
            AddNewRaindrops(gameTime);
            RemoveExpiredRaindrops();
        }

        private void AddNewRaindrops(GameTime gameTime)
        {
            var timeElapsedInSeconds = gameTime.TotalGameTime.TotalSeconds - _startTimeInSeconds;
            var distanceFallenInPixels = _speedInPixelsPerSecond * timeElapsedInSeconds;

            while (ThereIsRoomLeftToFall((double)distanceFallenInPixels))
            {
                var raindrop = new StandardRaindrop(
                    space: new Space(_column, StreamHeight),
                    lifeSpan: 10000.0,
                    symbolColor: StandardRaindrop.DefaultColor
                );
                _raindrops.Add(raindrop);
                _raindropCount++;
            }
        }

        private void RemoveExpiredRaindrops()
        {
            // ASSUMPTION: Raindrops at the tail of the raindrop die before raindrops at the head.
            // Even if this isn't true, they'll *eventually* be cleaned up, but performance would be sub-optimal.
            // I think this assumption will remain mostly true, with "glitched raindrops" being the only exception.
            var indexOfFirstLivingRaindrop = _raindrops.FindIndex(
                (StandardRaindrop raindrop) => { return !raindrop.IsDead(); }
            );
            if (indexOfFirstLivingRaindrop > 0)
            {
                _raindrops.RemoveRange(0, indexOfFirstLivingRaindrop);
            }
        }

        // ASSUMPTION: Monospace font (all characters same height)
        private float CharacterHeight { get { return _spriteFont.MeasureString("A").Y; } }
        private float StreamHeight { get { return _raindropCount * CharacterHeight; } }
        private bool ThereIsRoomLeftToFall(double distanceFallenInPixels)
        {
            var nextDrawPositionY = StreamHeight + CharacterHeight;
            return nextDrawPositionY < System.Math.Min(_column.Height, distanceFallenInPixels);
        }
    }
}