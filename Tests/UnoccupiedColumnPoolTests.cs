using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DigitalRain.Columns;
using Microsoft.Xna.Framework;

namespace DigitalRain.Tests.DigitalRainTests
{
    [TestFixture]
    public class UnoccupiedColumnPoolTests
    {
        Rectangle _screenBounds;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _screenBounds = new Rectangle(0, 0, 400, 800);
        }

        [Test]
        public void PickOne_ThrowsException_IfColumnCountIsLow([Range(1, 20)] int columnCount)
        {
            var lowWaterCount = columnCount + 1;
            var picker = new RandomColumnNumberPicker(columnCount, lowWaterCount);
            var pool = new UnoccupiedColumnPool(picker, _screenBounds);

            Assert.AreEqual(pool.IsLow, true);
            Assert.Throws<InvalidOperationException>(delegate { pool.PickOne(); });
        }

        [Test]
        public void PickOne_ThrowsException_IfAllColumnsArePicked([Range(2, 20)] int columnCount)
        {
            var picker = new RandomColumnNumberPicker(columnCount, 1);
            var pool = new UnoccupiedColumnPool(picker, _screenBounds);

            for (int i = 0; i < columnCount - 1; i++)
            {
                pool.PickOne();
            }

            Assert.AreEqual(pool.IsLow, true);
            Assert.Throws<InvalidOperationException>(delegate { pool.PickOne(); });
        }

        [Test]
        public void PickOne_ThrowsException_IfZeroColumns()
        {
            var picker = new RandomColumnNumberPicker(columnCount: 0, 1);
            Assert.Throws<DivideByZeroException>(delegate { new UnoccupiedColumnPool(picker, _screenBounds); });
        }
    }
}