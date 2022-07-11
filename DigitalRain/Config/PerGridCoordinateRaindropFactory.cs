using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DigitalRain.Config
{
    using Grid;
    using Raindrop.Raindrops;
    using Raindrop;

    public class PerGridCoordinateRaindropFactory : IRaindropFactory
    {
        private static readonly Random _randomGen = new Random();
        private readonly StandardRaindropFactoryConfig _config;
        private readonly Dictionary<GridCoordinates, char[]> _symbolPools;
        private readonly Dictionary<GridCoordinates, double> _lifespans;

        public ColorCalculatorFactory ColorCalculatorFactory { get; private set; }

        public PerGridCoordinateRaindropFactory()
        {
            _config = DigitalRainGame.Config.standardRaindropFactory;

            _symbolPools = new Dictionary<GridCoordinates, char[]>();
            _lifespans = new Dictionary<GridCoordinates, double>();
            InitializeSymbolPoolsAndLifespans(rowNumber: 7, fixedText: "HELLO WORLD!", fixedLifespan: 120000); // 2 minutes

            ColorCalculatorFactory = new ColorCalculatorFactory(
                startColor: Color.White, endColor: Color.GreenYellow, lerpTime: 400);
        }

        public IRaindrop Create(GridCoordinates coordinates)
        {
            var lifespan = GetLifespan(coordinates);
            var symbolPool = GetSymbolPool(coordinates);
            var symbol = GetSymbolFromPool(symbolPool);
            var colorCalculator = ColorCalculatorFactory.Create(lifespan);

            var glitchChance = 0.07;
            if (_randomGen.NextDouble() < glitchChance)
            {
                // TODO: Needs to implement IRaindrop if we want to get it working again.
                // return new GlitchedRaindrop(coordinates, symbol, lifespan, colorCalculator);
            }
            return new StandardRaindrop(coordinates, symbol, lifespan, colorCalculator);
        }

        private void InitializeSymbolPoolsAndLifespans(int rowNumber, string fixedText, double fixedLifespan)
        {
            var columnCount = 50;
            var columnNumber = (columnCount / 2) - (fixedText.Length / 2);
            foreach (var character in fixedText)
            {
                _symbolPools[new GridCoordinates(rowNumber, columnNumber)] = new char[] { character };
                _lifespans[new GridCoordinates(rowNumber, columnNumber)] = fixedLifespan;
                columnNumber++;
            }
        }

        private double GetLifespan(GridCoordinates coordinates)
        {
            if (_lifespans.ContainsKey(coordinates))
            {
                return _lifespans[coordinates];
            }

            var lifespanRange = (_config.lifespanMax + 1) - _config.lifespanMin;
            var randomLifespan = _config.lifespanMin + (_randomGen.NextDouble() * lifespanRange);
            return _config.lifespanMax;  // TODO: Put this back
        }

        private char[] GetSymbolPool(GridCoordinates coordinates)
        {
            if (_symbolPools.ContainsKey(coordinates))
            {
                return _symbolPools[coordinates];
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
