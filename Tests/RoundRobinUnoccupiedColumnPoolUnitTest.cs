using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalRain;

namespace DigitalRainTests
{
    [TestClass]
    public class RoundRobinUnoccupiedColumnPoolUnitTest
    {
        [TestMethod]
        public void PickOne_WhenAllColumnsUnoccupied_ReturnsColumnsInSequence()
        {
            
        }

        [TestMethod]
        public void PickOne_WhenSomeColumnsUnoccupied_ReturnsColumnsInSequence()
        {

        }

        [TestMethod]
        public void PickOne_WhenAtEndOfSequence_ReturnsFirstColumn()
        {

        }

        [TestMethod]
        public void PickOne_WhenNoColumnsUnoccupied_ThrowsInvalidOperationException()
        {

        }

        [TestMethod]
        public void Restore_RestoredColumnsCanBeReturnedByPickOne()
        {

        }

        [TestMethod]
        public void Restore_WhenOneColumnIsAlreadyUnoccupied_ThrowsInvalidOperationException()
        {

        }

        [TestMethod]
        public void Restore_WhenOneColumnIsAlreadyUnoccupied_NoColumnsAreRestored()
        {

        }
    }
}
