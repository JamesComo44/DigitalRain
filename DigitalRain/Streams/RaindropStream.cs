using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace DigitalRain.Raindrops
{
    using Columns;
    using System.Collections;

    public class RaindropStream : IModelObject, IEnumerable<IRaindrop>
    {
        private readonly IRaindropFactory _raindropFactory;
        public Column Column { get; private set; }
        private readonly float _fontHeight;
        private double _lifeSoFarInSeconds;
        private readonly float _speedInPixelsPerSecond;
        private double _distanceFallenInPixels;
        private int _raindropCount;
        private List<IRaindrop> _raindrops;

        public RaindropStream(Column column, IRaindropFactory raindropFactory, float speedInPixelsPerSecond, float fontHeight)
        {
            Column = column;
            _raindropFactory = raindropFactory;
            _fontHeight = fontHeight;
            _speedInPixelsPerSecond = speedInPixelsPerSecond;
            _raindrops = new List<IRaindrop>();
            _raindropCount = 0;  // This climbs forever, whereas the size of the list above shrinks as we delete things.
            _lifeSoFarInSeconds = 0;
        }

        public void Update(GameTime gameTime)
        {
            _lifeSoFarInSeconds += gameTime.ElapsedGameTime.TotalSeconds;

            Fall();

            foreach (var raindrop in _raindrops)
            {
                raindrop.Update(gameTime);
            }
        }

        private void Fall()
        {
            var unboundDistanceFallenInPixels = _speedInPixelsPerSecond * _lifeSoFarInSeconds;
            _distanceFallenInPixels = System.Math.Min(Column.Height, unboundDistanceFallenInPixels);

            AddNewRaindrops();
            RemoveDeadRaindrops();
        }

        private void AddNewRaindrops()
        {
            var nextSymbolPositionY = StreamHeight;
            while (nextSymbolPositionY < _distanceFallenInPixels)
            {
                var raindrop = _raindropFactory.Create(new GridCoordinates(_raindropCount, Column.Number));
                _raindrops.Add(raindrop);
                _raindropCount++;
                nextSymbolPositionY += SymbolHeight;
            }
        }

        private void RemoveDeadRaindrops()
        {
            _raindrops = _raindrops
                .Where(raindrop => !raindrop.IsDead)
                .ToList();
        }

        public bool IsDead
        {
            get
            {
                var noExistingRaindrops = _raindrops.Count == 0;
                var noNewRaindrops = _distanceFallenInPixels >= Column.Height;
                return noExistingRaindrops && noNewRaindrops;
            }
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
        public float SymbolHeight { get { return _fontHeight; } }
        private float StreamHeight { get { return _raindropCount * SymbolHeight; } }
    }
}