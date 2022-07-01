using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Raindrops
{
    using Columns;
    using Raindrops;

    class StreamSpawner
    {
        private UnoccupiedColumnPool _columnPool;
        private RaindropStreamFactory _streamFactory;
        private List<RaindropStream> _raindropStreams;
        private double _lastRaindropStreamCreationTimeInSeconds;
        private int StreamCount { get { return _raindropStreams.Count; } }

        public StreamSpawner(Rectangle screenBounds)
        {
            var columnNumberPicker = new RandomColumnNumberPicker(columnCount: 50);
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
                if (!_columnPool.IsEmpty)
                {
                    _lastRaindropStreamCreationTimeInSeconds = gameTime.TotalGameTime.TotalSeconds;
                    var raindropStream = _streamFactory.Create(currentFontHeight);
                    _raindropStreams.Add(raindropStream);
                }
            }
        }

        private void RemoveDeadRaindropStreams()
        {
            var indexOfFirstLivingStream = _raindropStreams.FindIndex(
                (RaindropStream stream) => { return !stream.IsDead; }
            );

            var deadStreams = new List<RaindropStream>();
            if (indexOfFirstLivingStream > 0)
            {
                deadStreams = _raindropStreams.GetRange(0, indexOfFirstLivingStream);
                _raindropStreams.RemoveRange(0, indexOfFirstLivingStream);
            }
            var columnsToRestore = from stream in deadStreams select stream.Column;
            _columnPool.Restore(new HashSet<Column>(columnsToRestore));
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
