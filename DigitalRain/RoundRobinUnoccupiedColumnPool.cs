using System;
using System.Collections.Generic;

namespace DigitalRain
{
    public class RoundRobinUnoccupiedColumnPool : IUnoccupiedColumnPool
    {
        public RoundRobinUnoccupiedColumnPool(int columnCount)
        {
            _columnPool = new bool[columnCount];
            Array.Fill(_columnPool, true);
        }

        public ColumnId PickOne()
        {
            if (_columnPool[_nextId])
            {
                _columnPool[_nextId] = false;

                var idToReturn = _nextId;
                _nextId = Increment(_nextId);
                return new ColumnId(idToReturn);
            }
            throw new InvalidOperationException("No unoccupied columns available in the pool");
        }

        public void Restore(ISet<ColumnId> columnsToRestore)
        {
            for (int i = 0; i < _columnPool.Length; i++)
            {
                if (columnsToRestore.Contains(new ColumnId(i)))
                {
                    _columnPool[i] = true;
                }
            }
        }

        private int Increment(int id)
        {
            return (id + 1) % ColumnCount;
        }

        public int ColumnCount
        {
            get { return _columnPool.Length; }
        }

        private readonly bool[] _columnPool;
        private int _nextId = 0;
    }
}