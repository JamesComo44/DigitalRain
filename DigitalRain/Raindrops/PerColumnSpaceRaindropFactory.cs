using System;
using System.Collections.Generic;

namespace DigitalRain.Raindrops
{
    using Columns;

    public class PerColumnSpaceRaindropFactory : IRaindropFactory
    {
        private static readonly Random _randomGen = new Random();
        private readonly StandardRaindropFactoryConfig _config;
        private readonly Dictionary<(int, int), char[]> _symbolPools;
        private readonly Dictionary<(int, int), double> _lifespans;

        public PerColumnSpaceRaindropFactory()
        {
            _config = DigitalRainGame.Config.standardRaindropFactory;

            _symbolPools = new Dictionary<(int, int), char[]>();
            _lifespans = new Dictionary<(int, int), double>();
            InitializeSymbolPoolsAndLifespans(rowNumber: 7, fixedText: "HELLO WORLD!", fixedLifespan: 120000); // 2 minutes
        }

        public IRaindrop Create(ColumnSpace space)
        {
            var lifespan = GetLifespan(space);
            var symbolPool = GetSymbolPool(space);
            var symbol = GetSymbolFromPool(symbolPool);
            
            return new StandardRaindrop(
                space, lifespan, symbolColor: StandardRaindrop.DefaultColor, symbol
            );
        }

        private void InitializeSymbolPoolsAndLifespans(int rowNumber, string fixedText, double fixedLifespan)
        {
            var columnCount = 50;
            var columnNumber = (columnCount / 2) - (fixedText.Length / 2);
            foreach (var character in fixedText)
            {
                _symbolPools[(rowNumber, columnNumber)] = new char[] { character };
                _lifespans[(rowNumber, columnNumber)] = fixedLifespan;
                columnNumber++;
            }
        }

        private double GetLifespan(ColumnSpace space)
        {
            if (_lifespans.ContainsKey(space.Coordinates))
            {
                return _lifespans[space.Coordinates];
            }

            var lifespanRange = (_config.lifespanMax + 1) - _config.lifespanMin;
            var randomLifespan = _config.lifespanMin + (_randomGen.NextDouble() * lifespanRange);
            return randomLifespan;
        }

        private char[] GetSymbolPool(ColumnSpace space)
        {
            if (_symbolPools.ContainsKey(space.Coordinates))
            {
                return _symbolPools[space.Coordinates];
            }

            return SymbolPools.EnglishAlphanumericUpperSymbols();
        }

        private char GetSymbolFromPool(char[] symbolPool)
        {
            Random randomGen = new Random();
            int index = randomGen.Next(symbolPool.Length);
            return symbolPool[index];
        }
    }
}
