using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

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
}