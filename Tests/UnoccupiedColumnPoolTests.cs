﻿using System;
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

        [TestCase(10, 10, ExpectedResult = 1f)]
        [TestCase(100, 10, ExpectedResult = 10f)]
        [TestCase(420, 36, ExpectedResult = 11f)]
        public float PickOne_ReturnsColumnsWithCorrectWidth(int screenWidth, int columnCount)
        {
            // Modify screen width for test case, but copy everything else.
            Rectangle bounds = _screenBounds;
            bounds.Width = screenWidth;

            var picker = new RandomColumnNumberPicker(columnCount);
            var pool = new UnoccupiedColumnPool(picker, bounds);

            Column column = pool.PickOne();
            return column.Width;
        }

        [Test]
        public void PickOne_ThrowsException_IfAllColumnsArePicked([Range(1, 20)] int columnCount)
        {
            var picker = new RandomColumnNumberPicker(columnCount);
            var pool = new UnoccupiedColumnPool(picker, _screenBounds);

            for (int i = 0; i < columnCount; i++)
            {
                pool.PickOne();
            }

            // I'm testing two things here and you can't stop me!
            Assert.AreEqual(pool.IsLow, true);
            Assert.Throws<InvalidOperationException>(delegate { pool.PickOne(); });
        }

        [Test]
        public void PickOne_ThrowsException_IfZeroColumns()
        {
            var picker = new RandomColumnNumberPicker(columnCount: 0);
            Assert.Throws<DivideByZeroException>(delegate { new UnoccupiedColumnPool(picker, _screenBounds); });
        }
    }
}