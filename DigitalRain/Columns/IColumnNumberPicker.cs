﻿
namespace DigitalRain.Columns
{
	public interface IColumnNumberPicker
	{
		int PickOne();
		void RestoreOne(int columnNumber);
		public int ColumnCount { get; }
		public bool IsEmpty { get; }
	}
}
