using System;
using System.Collections.Generic;

namespace DigitalRain.Columns
{
    public class UnoccupiedColumnPool
    {
        private readonly IColumnNumberPicker _columnNumberPicker;
        private readonly float _columnWidth;
        private Dictionary<Column, int> _columnToColumnNumber;

        public UnoccupiedColumnPool(IColumnNumberPicker columnNumberPicker, float screenWidth)
        {
            _columnNumberPicker = columnNumberPicker;
            _columnWidth = screenWidth / ColumnCount;
            _columnToColumnNumber = new Dictionary<Column, int>();
        }

        public int ColumnCount
        {
            get { return _columnNumberPicker.ColumnCount; }
        }

        public bool IsEmpty { get { return _columnNumberPicker.IsEmpty; } }

        public Column PickOne()
        {
            var columnNumber = _columnNumberPicker.PickOne();

            var positionX = CalculatePositionX(columnNumber);
            var column = new Column(positionX, _columnWidth);
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

        private float CalculatePositionX(int columnNumber)
        {
            return columnNumber * _columnWidth;
        }
    }
}