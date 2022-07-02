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
        private readonly StreamSpawnerConfig _config;
        private readonly RaindropStreamPool _streamPool;
        private List<RaindropStream> _raindropStreams;
        private double _lastRaindropStreamCreationTimeInSeconds;
        private float _currentFontHeight;

        public StreamSpawner(RaindropStreamPool streamPool)
        {
            _config = DigitalRainGame.Config.streamSpawner;
            _streamPool = streamPool;
            _raindropStreams = new List<RaindropStream>();
            _lastRaindropStreamCreationTimeInSeconds = 0;
            _currentFontHeight = 0;
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

        public void SetFontHeight(float height)
        {
            _currentFontHeight = height;
        }

        private void AddNewRaindropStreams(GameTime gameTime)
        {
            var timeElapsedSinceLastNewRaindropStream = gameTime.TotalGameTime.TotalSeconds - _lastRaindropStreamCreationTimeInSeconds;
            if (timeElapsedSinceLastNewRaindropStream > _config.minSecondsPerNewRaindropStream)
            {
                if (!_streamPool.IsLow)
                {
                    _lastRaindropStreamCreationTimeInSeconds = gameTime.TotalGameTime.TotalSeconds;
                    var raindropStream = _streamPool.Create(_currentFontHeight);
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
