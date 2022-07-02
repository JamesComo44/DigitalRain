using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Raindrops
{
    using Columns;
    using GameUtilities;

    class StreamSpawner: IGameObject
    {
        private StreamSpawnerConfig _config;
        private RaindropStreamPool _streamPool;
        private List<RaindropStream> _raindropStreams;
        private double _lastRaindropStreamCreationTimeInSeconds;

        public StreamSpawner(StreamSpawnerConfig config, RaindropStreamPool streamPool, Rectangle screenBounds)
        {
            _config = config;
            _streamPool = streamPool;
            _raindropStreams = new List<RaindropStream>();
            _lastRaindropStreamCreationTimeInSeconds = 0;
        }

        public void Update(GameTime gameTime)
        {
            AddNewRaindropStreams(gameTime);
            RemoveDeadRaindropStreams();
            foreach (var stream in _raindropStreams)
            {
                stream.Update(gameTime);
            }
        }

        private void AddNewRaindropStreams(GameTime gameTime, float currentFontHeight)
        {
            var timeElapsedSinceLastNewRaindropStream = gameTime.TotalGameTime.TotalSeconds - _lastRaindropStreamCreationTimeInSeconds;
            if (timeElapsedSinceLastNewRaindropStream > _config.minSecondsPerNewRaindropStream)
            {
                if (!_streamPool.IsLow)
                {
                    _lastRaindropStreamCreationTimeInSeconds = gameTime.TotalGameTime.TotalSeconds;
                    var raindropStream = _streamPool.Create(currentFontHeight);
                    _raindropStreams.Add(raindropStream);
                }
            }
        }

        private void RemoveDeadRaindropStreams()
        {
            var deadStreams = _raindropStreams.Where((stream) => stream.IsDead).ToHashSet();
            _raindropStreams = _raindropStreams.Where((stream) => !stream.IsDead).ToList();
            _streamPool.Restore(deadStreams);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            foreach (var stream in _raindropStreams)
            {
                stream.Draw(spriteBatch, font);
            }
        }
    }
}
