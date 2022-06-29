using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain
{
    public class RaindropStream
    {
        Column _column;
        float _width;
        SpriteBatch _spriteBatch;
        SpriteFont _spriteFont;

        public RaindropStream(Column column, float width, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            _column = column;
            _width = width;
            _spriteBatch = spriteBatch;
            _spriteFont = spriteFont;
        }

        public void Update()
        { }

        public void Draw(GameTime gameTime)
        {
            _column.DrawString(_spriteBatch, _spriteFont, "A", 0, Color.GreenYellow);
        }
    }
}