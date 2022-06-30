using System.Collections.Generic;
using Microsoft.Xna.Framework;

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

        public void Update(GameTime gameTime)
        {
            foreach (var stream in _streams)
            {
                stream.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var stream in _streams)
            {
                stream.Draw(gameTime);
            }
        }
    }
}