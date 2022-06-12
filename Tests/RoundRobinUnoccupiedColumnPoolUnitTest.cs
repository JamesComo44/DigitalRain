using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using DigitalRain;

namespace DigitalRainTests
{
    [TestClass]
    public class RoundRobinUnoccupiedColumnPoolUnitTest
    {
        [TestMethod]
        public void PickOne_ReturnsColumnsInSequence()
        {
            var pool = new RoundRobinUnoccupiedColumnPool(columnCount: 10);
            for (int i = 0; i < 10; i++)
            {
                var expectedId = new ColumnId(i);
                var actualId = pool.PickOne();
                Assert.AreEqual(actualId, expectedId);
            }
        }

        [TestMethod]
        public void PickOne_WhenNoColumnsUnoccupied_ThrowsInvalidOperationException()
        {
            var pool = new RoundRobinUnoccupiedColumnPool(columnCount: 10);
            PickAll(pool);
            Assert.ThrowsException<InvalidOperationException>(() => pool.PickOne());
        }

        [TestMethod]
        public void PickOne_WhenSomeColumnsUnoccupied_ContinuesReturningColumnsInSequence()
        {
            var pool = new RoundRobinUnoccupiedColumnPool(columnCount: 10);
            var columns = PickSome(pool, numberToPick: 5);
            pool.Restore(columns);

            var nextColumn = pool.PickOne();
            Assert.AreEqual(nextColumn, new ColumnId(5));
        }

        [TestMethod]
        public void PickOne_WhenAtEndOfSequence_ReturnsFirstColumn()
        {
            var pool = new RoundRobinUnoccupiedColumnPool(columnCount: 10);
            var columns = PickAll(pool);
            pool.Restore(columns);

            var nextColumn = pool.PickOne();
            Assert.AreEqual(nextColumn, new ColumnId(0));
        }

        private static HashSet<ColumnId> PickAll(RoundRobinUnoccupiedColumnPool pool)
        {
            return PickSome(pool, pool.ColumnCount);
        }

        private static HashSet<ColumnId> PickSome(RoundRobinUnoccupiedColumnPool pool, int numberToPick)
        {
            var columns = new HashSet<ColumnId>();
            for (int i = 0; i < numberToPick; i++)
            {
                var id = pool.PickOne();
                columns.Add(id);
            }
            return columns;
        }
    }
}
