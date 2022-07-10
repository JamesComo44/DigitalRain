using System;

namespace DigitalRain.Config
{
    public class RandomSymbolFactory
    {
        private static readonly Random _randomGen = new Random();

        private readonly char[] _symbolPool;

        public RandomSymbolFactory(char[] symbolPool)
        {
            _symbolPool = symbolPool;
        }

        public char Create()
        {
            int index = _randomGen.Next(_symbolPool.Length);
            return _symbolPool[index];
        }
    }
}