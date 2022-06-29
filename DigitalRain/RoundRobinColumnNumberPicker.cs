using System;

namespace DigitalRain
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
			if (!_columnNumberPool[_nextColumnNumber])
			{
				throw new InvalidOperationException("The next column number is not available in the RoundRobinColumnNumberPicker's pool");
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

		private void Increment()
		{
			_nextColumnNumber = (_nextColumnNumber + 1) % ColumnCount;
		}
	}
}
