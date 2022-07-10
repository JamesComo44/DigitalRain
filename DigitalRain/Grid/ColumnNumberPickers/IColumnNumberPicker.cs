
namespace DigitalRain.Grid.ColumnNumberPickers
{
	public interface IColumnNumberPicker
	{
		int PickOne();
		void RestoreOne(int columnNumber);
		public int ColumnCount { get; }
		public bool IsLow { get; }
	}
}
