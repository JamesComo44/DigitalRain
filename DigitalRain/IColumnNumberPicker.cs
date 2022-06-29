
namespace DigitalRain
{
	public interface IColumnNumberPicker
	{
		int PickOne();
		void RestoreOne(int columnNumber);
		public int ColumnCount { get; }
	}
}
