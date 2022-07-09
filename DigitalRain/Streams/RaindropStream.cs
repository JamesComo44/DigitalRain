using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Raindrops
{
    using Columns;
    using GameUtilities;
    using System.Collections;

    public class RaindropStream : IModelObject, IEnumerable<IRaindrop>
    {
        private readonly IRaindropFactory _raindropFactory;
        private readonly Column _column;
        private readonly float _fontHeight;
        private double? _startTimeInSeconds;
        private readonly float _speedInPixelsPerSecond;
        private double _unboundDistanceFallenInPixels;
        private int _raindropCount;
        readonly List<IRaindrop> _raindrops;

        public RaindropStream(IRaindropFactory raindropFactory, Column column, float speedInPixelsPerSecond, float fontHeight)
        {
            _raindropFactory = raindropFactory;
            _column = column;
            _fontHeight = fontHeight;
            _speedInPixelsPerSecond = speedInPixelsPerSecond;
            _raindrops = new List<IRaindrop>();
            _raindropCount = 0;  // This climbs forever, whereas the size of the list above shrinks as we delete things.
            IsDead = false;
        }

        public bool IsDead { get; private set; }

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

        private void Fall(GameTime gameTime)
        {
            var timeElapsedInSeconds = gameTime.TotalGameTime.TotalSeconds - _startTimeInSeconds;
            _unboundDistanceFallenInPixels = _speedInPixelsPerSecond * (double)timeElapsedInSeconds;
            AddNewRaindrops();
            //RemoveDeadRaindrops();
            UpdateIsDead();
        }

        private void AddNewRaindrops()
        {
            while (ThereIsRoomLeftToFall)
            {
                var raindrop = _raindropFactory.Create();
                _raindrops.Add(raindrop);
                _raindropCount++;
            }
        }

        private void UpdateIsDead()
        {
            if (IsDead)
            {
                return;
            }

            var noNewRaindrops = DistanceFallenInPixels == _column.Height && !ThereIsRoomLeftToFall;
            var allRaindropsDead = _raindrops.TrueForAll(raindrop => raindrop.IsDead);
            IsDead = noNewRaindrops && allRaindropsDead;
        }

        private void RemoveDeadRaindrops()
        {
            // ASSUMPTION: Raindrops at the tail of the raindrop die before raindrops at the head.
            // Even if this isn't true, they'll *eventually* be cleaned up, but performance would be sub-optimal.
            // I think this assumption will remain mostly true, with "glitched raindrops" being the only exception.
            var indexOfFirstLivingRaindrop = _raindrops.FindIndex(
                (raindrop) => { return !raindrop.IsDead; }
            );

            if (indexOfFirstLivingRaindrop == -1)
            {
                indexOfFirstLivingRaindrop = _raindrops.Count; 
            }
            _raindrops.RemoveRange(0, indexOfFirstLivingRaindrop);
        }

        public IEnumerator<IRaindrop> GetEnumerator()
        {
            return ((IEnumerable<IRaindrop>)_raindrops).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_raindrops).GetEnumerator();
        }

        // ASSUMPTION: Monospace font (all characters same height)
        public float CharacterHeight { get { return _fontHeight; } }
        private float StreamHeight { get { return _raindropCount * CharacterHeight; } }

        private float DistanceFallenInPixels
        {
            get
            {
                // Note this value is clamped to the bottom of the column.
                return System.Math.Min(_column.Height, (float)_unboundDistanceFallenInPixels);
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