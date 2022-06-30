using System;
using System.Collections.Generic;

namespace DigitalRain.Columns
{
	public class RandomColumnNumberPicker : IColumnNumberPicker
	{
		private int _columnCount;
		private List<int> _columnNumberPool;
		private Random _random;

		public RandomColumnNumberPicker(int columnCount)
        {
			_columnCount = columnCount;
			_columnNumberPool = new List<int>(_columnCount);
			for (int i = 0; i < _columnCount; i++)
            {
				_columnNumberPool.Add(i);
            }
			_random = new Random();
		}

		public int ColumnCount { get { return _columnCount; } }

		public int PickOne()
        {
			if (IsEmpty)
			{
				throw new InvalidOperationException("RandomColumnNumberPicker is empty!");
			}

			var randomIndex = PickRandomIndex();
			var columnNumber = _columnNumberPool[randomIndex];
			_columnNumberPool.RemoveAt(randomIndex);
			return columnNumber;
		}

		public void RestoreOne(int columnNumber)
        {
			// WARNING: We're trusting the user gave us a columnNumber we don't already have!
			_columnNumberPool.Add(columnNumber);
		}

		public bool IsEmpty { get { return _columnNumberPool.Count == 0; } }

		private int PickRandomIndex()
		{
			return _random.Next(_columnNumberPool.Count);
		}
	}
}
