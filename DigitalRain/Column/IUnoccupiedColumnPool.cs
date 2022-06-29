
using System.Collections.Generic;

namespace DigitalRain
{
	public interface IUnoccupiedColumnPool
	{
		ColumnId PickOne();
		void Restore(ISet<ColumnId> columnsToRestore);
	}
}
