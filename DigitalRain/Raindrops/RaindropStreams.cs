using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Raindrops
{
    /**
     * Just a Composite class.
     */
    public class RaindropStreams
    {
        private List<RaindropStream> _streams;

        public RaindropStreams()
        {
            _streams = new List<RaindropStream>();
        }

        public int Count { get { return _streams.Count; } }

        public void Add(RaindropStream stream)
        {
            _streams.Add(stream);
        }

        public List<RaindropStream> RemoveDeadStreams()
        {
            var indexOfFirstLivingStream = _streams.FindIndex(
                (RaindropStream stream) => { return !stream.IsDead; }
            );

            var deadStreams = new List<RaindropStream>();
            if (indexOfFirstLivingStream > 0)
            {
                deadStreams = _streams.GetRange(0, indexOfFirstLivingStream);
                _streams.RemoveRange(0, indexOfFirstLivingStream);
            }
            return deadStreams;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var stream in _streams)
            {
                stream.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font)
        {
            foreach (var stream in _streams)
            {
                stream.Draw(gameTime, spriteBatch, font);
            }
        }
    }
}