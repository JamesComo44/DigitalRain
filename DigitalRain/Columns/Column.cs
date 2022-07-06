using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Columns
{
    using Raindrops;

    public class Column
    {
        private static readonly Random _randomGen = new Random();

        private Rectangle _bounds;
        private readonly ColumnSpaceFactoryConfig _config;

        public Column(Rectangle bounds)
        {
            _bounds = bounds;
            _config = DigitalRainGame.Config.columnSpaceFactory;
        }

        public float Width { get { return _bounds.Width; } }
        public float Height { get { return _bounds.Height; } }

        public StandardRaindrop CreateRaindrop(float streamHeight)
        {
            var space = new ColumnSpace(this, streamHeight);
            var lifespanRange = (_config.lifespanMax + 1) - _config.lifespanMin;
            var randomLifespan = _config.lifespanMin + (_randomGen.NextDouble() * lifespanRange);
            return new StandardRaindrop(
                space, randomLifespan, symbolColor: StandardRaindrop.DefaultColor
            );
        }

        public void DrawString(SpriteBatch spriteBatch, SpriteFont spriteFont, string str, float positionY, Color color)
        {
            spriteBatch.DrawString(spriteFont, str, new Vector2(_bounds.X, positionY), color);
        }
    }
}