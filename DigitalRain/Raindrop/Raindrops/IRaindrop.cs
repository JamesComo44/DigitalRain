using Microsoft.Xna.Framework;

namespace DigitalRain.Raindrop.Raindrops
{
    using GameUtilities;
    using Grid;

    public interface IRaindrop : IModelObject
    {
        public bool IsDead { get; }
        public string Symbol { get; set;  }
        public Color Color { get; }
        public GridCoordinates Coordinates { get; }
        public double Lifespan { get; }
        public ColorCalculator ColorCalculator { get; set; }
    }
}
