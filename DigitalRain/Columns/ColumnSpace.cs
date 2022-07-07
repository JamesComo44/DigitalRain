using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Columns
{
    /** A space in a column.
     */
    public class ColumnSpace
    {
        private Column _column;
        private float _positionY;

        public ColumnSpace(Column column, int number, float positionY)
        {
            _column = column;
            RowNumber = number;
            _positionY = positionY;
        }

        public int RowNumber { get; private set; }
        public int ColumnNumber { get { return _column.Number; } }
        public (int, int) Coordinates { get { return (RowNumber, ColumnNumber); } }

        public void DrawString(SpriteBatch spriteBatch, SpriteFont spriteFont, string str, Color color)
        {
            _column.DrawString(spriteBatch, spriteFont, str, _positionY, color);
        }
    }
}