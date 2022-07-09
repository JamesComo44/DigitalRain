using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.Columns
{
    public class GridCoordinates : IEquatable<GridCoordinates>
    {
        private Point _point;

        public GridCoordinates(int row, int column)
        {
            _point = new Point(row, column);
        }

        public int RowNumber { get { return _point.X; } }
        public int ColumnNumber { get { return _point.Y; } }
        public override int GetHashCode() { return _point.GetHashCode(); }
        public bool Equals([AllowNull] GridCoordinates other)
        {
            return other != null && _point == other._point;
        }        
    }

    /** A space in a column.
     */
    public class ColumnSpace
    {
        private Column _column;
        private float _positionY;
        private int _rowNumber;

        /**
         * A ColumnSpace is a location in the grid that can be drawn to.
         * For example, in a 10x10 grid on a 600x800 pixel window, ColumnSpace (4, 5) will draw to pixel (240, 400)
         */
        public ColumnSpace(Column column, int rowNumber, float positionY)
        {
            _column = column;
            _rowNumber = rowNumber;
            _positionY = positionY;
        }

        public GridCoordinates Coordinates { get { return new GridCoordinates(_rowNumber, _column.Number); } }

        public void DrawString(SpriteBatch spriteBatch, SpriteFont spriteFont, string str, Color color)
        {
            _column.DrawString(spriteBatch, spriteFont, str, _positionY, color);
        }
    }
}