
using System.Collections.Generic;

namespace DigitalRain
{
	public interface IUnoccupiedColumnPool
	{
		int PickOne();
		void Restore(ISet<int> columnsToRestore);
		public int ColumnCount { get; }
	}
}
