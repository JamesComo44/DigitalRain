using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain
{
    public class Column
    {
        private float _xPosition;

        public Column(float xPosition, float width)
        {
            _xPosition = xPosition;
            Width = width;
        }

        public float Width { get; private set; }

        public void DrawString(SpriteBatch spriteBatch, SpriteFont spriteFont, string str, float yPosition, Color color)
        {
            spriteBatch.DrawString(spriteFont, str, new Vector2(_xPosition, yPosition), color);
        }
    }
}