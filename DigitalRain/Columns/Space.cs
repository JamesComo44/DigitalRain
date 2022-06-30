﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Columns
{
    /** A space in a column.
     */
    public class Space
    {
        private Column _column;
        private float _positionY;

        public Space(Column column, float positionY)
        {
            _column = column;
            _positionY = positionY;
        }

        public void DrawString(SpriteBatch spriteBatch, SpriteFont spriteFont, string str, Color color)
        {
            _column.DrawString(spriteBatch, spriteFont, str, _positionY, color);
        }
    }
}