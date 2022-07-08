using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

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
            var colorCalculator = new ColorCalculator(
                timespan: lifespan, startColor: Color.White, endColor: Color.GreenYellow, lerpTime: 400);

            var glitchChance = 0.07;
            if (_randomGen.NextDouble() < glitchChance)
            {
                return new GlitchedRaindrop(space, symbol, lifespan, colorCalculator);
            }
            return new StandardRaindrop(space, symbol, lifespan, colorCalculator);
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
            return _config.lifespanMax;  // TODO: Put this back
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
            int index = _randomGen.Next(symbolPool.Length);
            return symbolPool[index];
        }
    }
}
