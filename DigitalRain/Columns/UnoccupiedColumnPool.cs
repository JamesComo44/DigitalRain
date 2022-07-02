using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DigitalRain.Columns
{
    public class UnoccupiedColumnPool
    {
        private readonly IColumnNumberPicker _columnNumberPicker;
        private readonly Rectangle _columnDimensions;
        private readonly Dictionary<Column, int> _columnToColumnNumber;

        public UnoccupiedColumnPool(IColumnNumberPicker columnNumberPicker, Rectangle screenBounds)
        {
            _columnNumberPicker = columnNumberPicker;
            _columnDimensions = new Rectangle(
                x: 0, y: 0,  // Kinda gross, really just want a Rectangle for its `.Width` and `.Height`
                width: screenBounds.Width / ColumnCount,
                height: screenBounds.Height
            );
            _columnToColumnNumber = new Dictionary<Column, int>();
        }

        public int ColumnCount
        {
            get { return _columnNumberPicker.ColumnCount; }
        }

        public bool IsLow { get { return _columnNumberPicker.IsLow; } }

        public Column PickOne()
        {
            var columnNumber = _columnNumberPicker.PickOne();

            var positionX = CalculatePositionX(columnNumber);
            var column = new Column(
                bounds: new Rectangle(positionX, 0, _columnDimensions.Width, _columnDimensions.Height)
            );
            _columnToColumnNumber[column] = columnNumber;

            return column;
        }

        public void Restore(ISet<Column> columnsToRestore)
        {
            foreach (var column in columnsToRestore)
            {
                _columnToColumnNumber.Remove(column, out int columnNumber);
                _columnNumberPicker.RestoreOne(columnNumber);
            }
        }

        private int CalculatePositionX(int columnNumber)
        {
            return columnNumber * _columnDimensions.Width;
        }
    }
}