using System;

namespace DigitalRain.Config
{
    public class RandomLifespanFactory
    {
        private static readonly Random _randomGen = new Random();

        private readonly double _lifespanMin;
        private readonly double _lifespanMax;

        public RandomLifespanFactory(int lifespanMin, int lifespanMax)
        {
            _lifespanMin = lifespanMin;
            _lifespanMax = lifespanMax;
        }

        public double Create()
        {
            var lifespanRange = (_lifespanMax + 1) - _lifespanMin;
            return _lifespanMin + (_randomGen.NextDouble() * lifespanRange);
        }
    }
}
