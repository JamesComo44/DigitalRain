using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DigitalRain.Grid
{
    using ColumnNumberPickers;

    public class UnoccupiedColumnPool
    {
        private readonly IColumnNumberPicker _columnNumberPicker;
        private readonly Rectangle _columnDimensions;
        private List<Column> _occupiedColumns;

        public UnoccupiedColumnPool(IColumnNumberPicker columnNumberPicker, Rectangle screenBounds)
        {
            _columnNumberPicker = columnNumberPicker;
            _columnDimensions = new Rectangle(
                x: 0, y: 0,  // Kinda gross, really just want a Rectangle for its `.Width` and `.Height`
                width: screenBounds.Width / ColumnCount,
                height: screenBounds.Height
            );
            _occupiedColumns = new List<Column>();
        }

        public int ColumnCount
        {
            get { return _columnNumberPicker.ColumnCount; }
        }

        public float ColumnWidth
        {
            get { return _columnDimensions.Width; }
        }

        public bool IsLow { get { return _columnNumberPicker.IsLow; } }

        public Column PickOne()
        {
            var columnNumber = _columnNumberPicker.PickOne();

            var positionX = CalculatePositionX(columnNumber);
            var column = new Column(
                number: columnNumber,
                bounds: new Rectangle(positionX, 0, _columnDimensions.Width, _columnDimensions.Height)
            );
            _occupiedColumns.Add(column);

            return column;
        }

        public void Restore(ISet<Column> columnsToRestore)
        {
            var newOccupiedColumns = new List<Column>();
            foreach (var column in _occupiedColumns)
            {
                if (!columnsToRestore.Contains(column))
                {
                    newOccupiedColumns.Add(column);
                }
                else
                {
                    _columnNumberPicker.RestoreOne(column.Number);
                }
            }
            _occupiedColumns = newOccupiedColumns;
        }

        private int CalculatePositionX(int columnNumber)
        {
            return columnNumber * _columnDimensions.Width;
        }
    }
}