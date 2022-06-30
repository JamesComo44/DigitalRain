using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Columns
{
    public class Column
    {
        private Rectangle _bounds;

        public Column(Rectangle bounds)
        {
            _bounds = bounds;
        }

        public float Width { get { return _bounds.Width; } }
        public float Height { get { return _bounds.Height; } }

        public void DrawString(SpriteBatch spriteBatch, SpriteFont spriteFont, string str, float positionY, Color color)
        {
            spriteBatch.DrawString(spriteFont, str, new Vector2(_bounds.X, positionY), color);
        }
    }
}