using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Raindrops
{
    using Columns;

    class StreamSpawner
    {
        private UnoccupiedColumnPool _columnPool;
        private RaindropStreamFactory _streamFactory;
        private List<RaindropStream> _raindropStreams;
        private double _lastRaindropStreamCreationTimeInSeconds;
        private int StreamCount { get { return _raindropStreams.Count; } }

        public StreamSpawner(Rectangle screenBounds)
        {
            var columnNumberPicker = new RandomColumnNumberPicker(columnCount: 50, lowWaterMark:10);
            _columnPool = new UnoccupiedColumnPool(columnNumberPicker, screenBounds);

            _streamFactory = new RaindropStreamFactory(_columnPool);
            _raindropStreams = new List<RaindropStream>();
            _lastRaindropStreamCreationTimeInSeconds = 0;
        }

        public void Update(GameTime gameTime, float currentFontHeight)
        {
            AddNewRaindropStreams(gameTime, currentFontHeight);
            RemoveDeadRaindropStreams();
            foreach (var stream in _raindropStreams)
            {
                stream.Update(gameTime);
            }
        }

        private void AddNewRaindropStreams(GameTime gameTime, float currentFontHeight)
        {
            double minSecondsPerNewRaindropStream = 0.33;
            var timeElapsedSinceLastNewRaindropStream = gameTime.TotalGameTime.TotalSeconds - _lastRaindropStreamCreationTimeInSeconds;
            if (timeElapsedSinceLastNewRaindropStream > minSecondsPerNewRaindropStream)
            {
                if (!_columnPool.IsLow)
                {
                    _lastRaindropStreamCreationTimeInSeconds = gameTime.TotalGameTime.TotalSeconds;
                    var raindropStream = _streamFactory.Create(currentFontHeight);
                    _raindropStreams.Add(raindropStream);
                }
            }
        }

        private void RemoveDeadRaindropStreams()
        {
            var deadStreams = _raindropStreams.Where((stream) => stream.IsDead).ToList();
            _raindropStreams = _raindropStreams.Where((stream) => !stream.IsDead).ToList();
            var columnsToRestore = deadStreams.Select((stream) => stream.Column).ToHashSet();
            _columnPool.Restore(columnsToRestore);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font)
        {
            foreach (var stream in _raindropStreams)
            {
                stream.Draw(gameTime, spriteBatch, font);
            }
        }
    }
}
