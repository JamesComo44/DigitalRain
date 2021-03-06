using System;
using System.Collections.Generic;

namespace DigitalRain.Columns
{
	public class RandomColumnNumberPicker : IColumnNumberPicker
	{
		private static readonly Random _randomGen = new Random();

		private readonly int _columnCount;
		private readonly int _lowWaterMark;
		private List<int> _columnNumberPool;

		public int ColumnCount { get { return _columnCount; } }
		public bool IsLow { get { return _columnNumberPool.Count <= _lowWaterMark; } }

		public RandomColumnNumberPicker(int columnCount, int lowWaterMark)
        {
			_columnCount = columnCount;
			_lowWaterMark = lowWaterMark;

			_columnNumberPool = new List<int>(_columnCount);
			for (int i = 0; i < _columnCount; i++)
            {
				_columnNumberPool.Add(i);
            }
		}

		public int PickOne()
        {
			if (IsLow)
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

		private int PickRandomIndex()
		{
			return _randomGen.Next(_columnNumberPool.Count);
		}
	}
}
