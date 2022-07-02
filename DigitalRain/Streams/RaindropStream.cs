using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Raindrops
{
    using Columns;
    using GameUtilities;

    public class RaindropStream: IGameObject
    {
        private readonly StandardRaindropFactory _raindropFactory;
        private readonly float _fontHeight;
        private double? _startTimeInSeconds;
        private readonly float _speedInPixelsPerSecond;
        private double _unboundDistanceFallenInPixels;
        private int _raindropCount;
        readonly List<StandardRaindrop> _raindrops;

        public RaindropStream(StandardRaindropFactory raindropFactory, Column column, float speedInPixelsPerSecond, float fontHeight)
        {
            _raindropFactory = raindropFactory;
            Column = column;
            _fontHeight = fontHeight;
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

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            foreach (var raindrop in _raindrops)
            {
                raindrop.Draw(spriteBatch, font);
            }
        }

        private void Fall(GameTime gameTime)
        {
            var timeElapsedInSeconds = gameTime.TotalGameTime.TotalSeconds - _startTimeInSeconds;
            _unboundDistanceFallenInPixels = _speedInPixelsPerSecond * (double)timeElapsedInSeconds;
            AddNewRaindrops();
            RemoveDeadRaindrops();
        }

        private void AddNewRaindrops()
        {
            while (ThereIsRoomLeftToFall)
            {
                var raindrop = _raindropFactory.Create(
                    space: new ColumnSpace(Column, StreamHeight));
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
        private float CharacterHeight { get { return _fontHeight; } }
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