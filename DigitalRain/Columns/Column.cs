using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Columns
{
    public class Column
    {
        private Rectangle _bounds;

        public Column(int number, Rectangle bounds)
        {
            Number = number;
            _bounds = bounds;
        }

        public int Number { get; private set; }
        public float Height { get { return _bounds.Height; } }
    }
}