using Microsoft.Xna.Framework;

namespace DigitalRain.Config
{
    using Raindrop.Raindrops;
    using Grid;

    public class StandardRaindropFactory : IRaindropFactory
    {
        private RandomSymbolFactory _symbolFactory;
        private RandomLifespanFactory _lifespanFactory;
        public ColorCalculatorFactory ColorCalculatorFactory { get; private set; }

        public StandardRaindropFactory(RandomSymbolFactory symbolFactory, RandomLifespanFactory lifespanFactory, ColorCalculatorFactory colorCalculatorFactory)
        {
            _symbolFactory = symbolFactory;
            _lifespanFactory = lifespanFactory;
            ColorCalculatorFactory = colorCalculatorFactory;
        }
        
        public IRaindrop Create(GridCoordinates coordinates)
        {
            var symbol = _symbolFactory.Create();
            var lifespan = _lifespanFactory.Create();
            var colorCalculator = ColorCalculatorFactory.Create(timespan: lifespan);
            return new StandardRaindrop(coordinates, symbol, lifespan, colorCalculator);
        }
    }
}
