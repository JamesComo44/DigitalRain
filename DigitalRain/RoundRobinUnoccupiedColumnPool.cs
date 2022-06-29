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

        public int PickOne()
        {
            if (_columnPool[_nextId])
            {
                _columnPool[_nextId] = false;

                var idToReturn = _nextId;
                _nextId = Increment(_nextId);
                return idToReturn;
            }
            throw new InvalidOperationException("No unoccupied columns available in the pool");
        }

        public void Restore(ISet<int> columnsToRestore)
        {
            foreach (var columnNumber in columnsToRestore)
            {
                _columnPool[columnNumber] = true;
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