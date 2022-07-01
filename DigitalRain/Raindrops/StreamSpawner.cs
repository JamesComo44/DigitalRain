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
        private RaindropStreams _raindropStreams;

        public StreamSpawner(Rectangle screenBounds)
        {
            var columnNumberPicker = new RandomColumnNumberPicker(columnCount: 50);
            _columnPool = new UnoccupiedColumnPool(columnNumberPicker, screenBounds);

            _streamFactory = new RaindropStreamFactory(_columnPool);
            _raindropStreams = new RaindropStreams();
        }

        public void Update(GameTime gameTime, float currentFontHeight)
        {
            AddNewRaindropStreams(gameTime, currentFontHeight);
            RemoveDeadRaindropStreams();
            _raindropStreams.Update(gameTime);
        }


        private void AddNewRaindropStreams(GameTime gameTime, float currentFontHeight)
        {
            float newRaindropStreamsPerSecond = 3;
            float secondsPerNewRaindropStream = 1 / newRaindropStreamsPerSecond;
            var numRaindropStreams = (int)(gameTime.TotalGameTime.TotalSeconds / secondsPerNewRaindropStream);
            while (!_columnPool.IsEmpty && _raindropStreams.Count < numRaindropStreams)
            {
                var raindropStream = _streamFactory.Create(currentFontHeight);
                _raindropStreams.Add(raindropStream);
            }
        }

        private void RemoveDeadRaindropStreams()
        {
            var deadStreams = _raindropStreams.RemoveDeadStreams();
            var columnsToRestore = from stream in deadStreams select stream.Column;
            _columnPool.Restore(new HashSet<Column>(columnsToRestore));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font)
        {
            _raindropStreams.Draw(gameTime, spriteBatch, font);
        }
    }
}
