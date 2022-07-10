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
        public int Width { get { return _bounds.Width; } }
        public int Height { get { return _bounds.Height; } }
    }
}