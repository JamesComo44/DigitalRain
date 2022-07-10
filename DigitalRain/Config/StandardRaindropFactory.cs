using Microsoft.Xna.Framework;

namespace DigitalRain.Config
{
    using Raindrop.Raindrops;
    using Grid;

    public class StandardRaindropFactory : IRaindropFactory
    {
        RandomSymbolFactory _symbolFactory;
        RandomLifespanFactory _lifespanFactory;
        ColorCalculatorFactory _colorCalculatorFactory;

        public StandardRaindropFactory(RandomSymbolFactory symbolFactory, RandomLifespanFactory lifespanFactory, ColorCalculatorFactory colorCalculatorFactory)
        {
            _symbolFactory = symbolFactory;
            _lifespanFactory = lifespanFactory;
            _colorCalculatorFactory = colorCalculatorFactory;
        }
        
        public IRaindrop Create(GridCoordinates coordinates)
        {
            var symbol = _symbolFactory.Create();
            var lifespan = _lifespanFactory.Create();
            var colorCalculator = _colorCalculatorFactory.Create(timespan: lifespan);
            return new StandardRaindrop(coordinates, symbol, lifespan, colorCalculator);
        }
    }
}
