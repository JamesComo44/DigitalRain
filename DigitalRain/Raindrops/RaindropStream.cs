using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Raindrops
{
    using Columns;
    using System;

    public class RaindropStream
    {
        // Static will use the same seed across all class instances.
        private static readonly Random _randomGen = new Random();

        private float _width;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private double? _startTimeInSeconds;
        private float _speedInPixelsPerSecond;
        private double _unboundDistanceFallenInPixels;
        private int _raindropCount;
        List<StandardRaindrop> _raindrops;

        public RaindropStream(Column column, float width, SpriteBatch spriteBatch, SpriteFont spriteFont, float speedInPixelsPerSecond)
        {
            Column = column;
            _width = width;
            _spriteBatch = spriteBatch;
            _spriteFont = spriteFont;
            _speedInPixelsPerSecond = speedInPixelsPerSecond;
            _raindrops = new List<StandardRaindrop>();
            _raindropCount = 0;  // This climbs forever, whereas the size of the list above shrinks as we delete things.
        }

        public Column Column { get; private set;}

        public bool IsDead
        {
            get
            {
                var noNewRaindrops = DistanceFallenInPixels == Column.Height && !ThereIsRoomLeftToFall;
                var allRaindropsDeadAndRemoved = _raindrops.Count == 0;
                return noNewRaindrops && allRaindropsDeadAndRemoved;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_startTimeInSeconds == null)
            {
                _startTimeInSeconds = gameTime.TotalGameTime.TotalSeconds;
            }

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

        private void Fall(GameTime gameTime)
        {
            var timeElapsedInSeconds = gameTime.TotalGameTime.TotalSeconds - _startTimeInSeconds;
            _unboundDistanceFallenInPixels = _speedInPixelsPerSecond * (double)timeElapsedInSeconds;
            AddNewRaindrops(gameTime);
            RemoveDeadRaindrops();
        }

        private void AddNewRaindrops(GameTime gameTime)
        {
            int lowRange = 2400;
            int highRange = 4000;
            //double precision = _randomGen.NextDouble();

            while (ThereIsRoomLeftToFall)
            {
                //double randomLifespan = (double)_randomGen.Next(lowRange, highRange + 1) + precision;
                double randomLifespan = (double)_randomGen.Next(lowRange, highRange + 1);

                var raindrop = new StandardRaindrop(
                    space: new ColumnSpace(Column, StreamHeight),
                    lifeSpan: randomLifespan,
                    symbolColor: StandardRaindrop.DefaultColor
                );
                _raindrops.Add(raindrop);
                _raindropCount++;
            }
        }

        private void RemoveDeadRaindrops()
        {
            // ASSUMPTION: Raindrops at the tail of the raindrop die before raindrops at the head.
            // Even if this isn't true, they'll *eventually* be cleaned up, but performance would be sub-optimal.
            // I think this assumption will remain mostly true, with "glitched raindrops" being the only exception.
            var indexOfFirstLivingRaindrop = _raindrops.FindIndex(
                (StandardRaindrop raindrop) => { return !raindrop.IsDead(); }
            );

            if (indexOfFirstLivingRaindrop == -1)
            {
                indexOfFirstLivingRaindrop = _raindrops.Count; 
            }
            _raindrops.RemoveRange(0, indexOfFirstLivingRaindrop);
        }

        // ASSUMPTION: Monospace font (all characters same height)
        private float CharacterHeight { get { return _spriteFont.MeasureString("A").Y; } }
        private float StreamHeight { get { return _raindropCount * CharacterHeight; } }

        private float DistanceFallenInPixels
        {
            get
            {
                // Note this value is clamped to the bottom of the column.
                return System.Math.Min(Column.Height, (float)_unboundDistanceFallenInPixels);
            }
        }

        private bool ThereIsRoomLeftToFall
        {
            get
            {
                var nextDrawPositionY = StreamHeight + CharacterHeight;
                return nextDrawPositionY < DistanceFallenInPixels;
            }
        }
    }
}