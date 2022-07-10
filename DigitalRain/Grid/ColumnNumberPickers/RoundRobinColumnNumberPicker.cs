using System;

namespace DigitalRain.Grid.ColumnNumberPickers
{
	public class RoundRobinColumnNumberPicker : IColumnNumberPicker
	{
		private readonly bool[] _columnNumberPool;
		private int _nextColumnNumber;

		public RoundRobinColumnNumberPicker(int columnCount)
        {
			_columnNumberPool = new bool[columnCount];
			Array.Fill(_columnNumberPool, true);
			_nextColumnNumber = 0;
		}

		public int ColumnCount { get { return _columnNumberPool.Length; } }

		public int PickOne()
        {
			if (IsLow)
			{
				throw new InvalidOperationException("RoundRobinColumnNumberPicker is empty!");
			}

			_columnNumberPool[_nextColumnNumber] = false;
			int columnNumberToReturn = _nextColumnNumber;
			Increment();
			return columnNumberToReturn;
		}

		public void RestoreOne(int columnNumber)
        {
			_columnNumberPool[columnNumber] = true;
		}

		public bool IsLow { get { return !_columnNumberPool[_nextColumnNumber]; } }

		private void Increment()
		{
			_nextColumnNumber = (_nextColumnNumber + 1) % ColumnCount;
		}
	}
}
