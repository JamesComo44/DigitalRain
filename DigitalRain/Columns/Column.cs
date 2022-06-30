using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Columns
{
    public class Column
    {
        private float _positionX;

        public Column(float positionX, float width)
        {
            _positionX = positionX;
            Width = width;
        }

        public float Width { get; private set; }

        public void DrawString(SpriteBatch spriteBatch, SpriteFont spriteFont, string str, float positionY, Color color)
        {
            spriteBatch.DrawString(spriteFont, str, new Vector2(_positionX, positionY), color);
        }
    }
}