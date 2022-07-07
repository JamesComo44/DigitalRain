using System;
using System.Collections.Generic;

namespace DigitalRain.Raindrops
{
    using Columns;

    public class PerColumnSpaceRaindropFactory : IRaindropFactory
    {
        private static readonly Random _randomGen = new Random();
        private readonly StandardRaindropFactoryConfig _config;
        private readonly Dictionary<int, char[]> _symbolPools;

        public PerColumnSpaceRaindropFactory()
        {
            _config = DigitalRainGame.Config.standardRaindropFactory;

            _symbolPools = new Dictionary<int, char[]>();
            _symbolPools[19] = new char[] { 'H' };
            _symbolPools[20] = new char[] { 'E' };
            _symbolPools[21] = new char[] { 'L' };
            _symbolPools[22] = new char[] { 'L' };
            _symbolPools[23] = new char[] { 'O' };
            _symbolPools[24] = new char[] { ' ' };
            _symbolPools[25] = new char[] { 'W' };
            _symbolPools[26] = new char[] { 'O' };
            _symbolPools[27] = new char[] { 'R' };
            _symbolPools[28] = new char[] { 'L' };
            _symbolPools[29] = new char[] { 'D' };
            _symbolPools[30] = new char[] { '!' };
        }

        public IRaindrop Create(ColumnSpace space)
        {
            var lifespanRange = (_config.lifespanMax + 1) - _config.lifespanMin;
            var randomLifespan = _config.lifespanMin + (_randomGen.NextDouble() * lifespanRange);
            var symbolPool = GetSymbolPool(space);
            
            return new StandardRaindrop(
                space, randomLifespan, symbolColor: StandardRaindrop.DefaultColor, symbolPool
            );
        }

        private char[] GetSymbolPool(ColumnSpace space)
        {
            if (_symbolPools.ContainsKey(space.ColumnNumber))
            {
                return _symbolPools[space.ColumnNumber];
            }

            return SymbolPools.EnglishAlphanumericUpperSymbols();
        }
    }
}
