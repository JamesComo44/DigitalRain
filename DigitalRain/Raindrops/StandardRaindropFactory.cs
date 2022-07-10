using System;
using Microsoft.Xna.Framework;

namespace DigitalRain.Raindrops
{
    using Columns;

    public class StandardRaindropFactory : IRaindropFactory
    {
        // Static will use the same seed across all class instances.
        private static readonly Random _randomGen = new Random();
        private readonly StandardRaindropFactoryConfig _config;

        public StandardRaindropFactory()
        {
            _config = DigitalRainGame.Config.standardRaindropFactory;
        }
        
        public IRaindrop Create(GridCoordinates coordinates, int columnWidth, float symbolHeight)
        {
            var lifespanRange = (_config.lifespanMax + 1) - _config.lifespanMin;
            var randomLifespan = _config.lifespanMin + (_randomGen.NextDouble() * lifespanRange);
            var symbol = GetSymbolFromPool(SymbolPools.EnglishAlphanumericUpperSymbols());
            var colorCalculator = new ColorCalculator(
                timespan: randomLifespan, startColor: Color.White, endColor: Color.GreenYellow, lerpTime: 400);
            var position = new Vector2(coordinates.ColumnNumber * columnWidth, coordinates.RowNumber * symbolHeight);
            return new StandardRaindrop(position, symbol, randomLifespan, colorCalculator);
        }

        private char GetSymbolFromPool(char[] symbolPool)
        {
            int index = _randomGen.Next(symbolPool.Length);
            return symbolPool[index];
        }
    }
}
