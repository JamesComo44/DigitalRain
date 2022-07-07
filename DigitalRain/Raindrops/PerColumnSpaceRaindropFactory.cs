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
            _symbolPools[(7, 19)] = new char[] { 'H' };
            _symbolPools[(7, 20)] = new char[] { 'E' };
            _symbolPools[(7, 21)] = new char[] { 'L' };
            _symbolPools[(7, 22)] = new char[] { 'L' };
            _symbolPools[(7, 23)] = new char[] { 'O' };
            _symbolPools[(7, 24)] = new char[] { ' ' };
            _symbolPools[(7, 25)] = new char[] { 'W' };
            _symbolPools[(7, 26)] = new char[] { 'O' };
            _symbolPools[(7, 27)] = new char[] { 'R' };
            _symbolPools[(7, 28)] = new char[] { 'L' };
            _symbolPools[(7, 29)] = new char[] { 'D' };
            _symbolPools[(7, 30)] = new char[] { '!' };

            _lifespans = new Dictionary<(int, int), double>();
            for (int i = 19; i < 31; i++)
            {
                _lifespans[(7, i)] = 120000;  // 2 mins
            }
        }

        public IRaindrop Create(ColumnSpace space)
        {
            var lifespan = GetLifespan(space);
            var symbolPool = GetSymbolPool(space);
            
            return new StandardRaindrop(
                space, lifespan, symbolColor: StandardRaindrop.DefaultColor, symbolPool
            );
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
    }
}
