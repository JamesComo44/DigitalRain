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
        public float Width { get { return _bounds.Width; } }
        public float Height { get { return _bounds.Height; } }

        public ColumnSpace CreateSpace(int number, float positionY)
        {
            return new ColumnSpace(this, number, positionY);
        }

        public void DrawString(SpriteBatch spriteBatch, SpriteFont spriteFont, string str, float positionY, Color color)
        {
            spriteBatch.DrawString(spriteFont, str, new Vector2(_bounds.X, positionY), color);
        }
    }
}