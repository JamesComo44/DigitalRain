using System;
using System.Collections.Generic;

namespace DigitalRain
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

        public Column PickOne()
        {
            var columnNumber = _columnNumberPicker.PickOne();

            var xPosition = CalculateXPosition(columnNumber);
            var column = new Column(xPosition, _columnWidth);
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

        private float CalculateXPosition(int columnNumber)
        {
            return columnNumber * _columnWidth;
        }
    }
}