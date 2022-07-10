using Microsoft.Xna.Framework;
namespace DigitalRain.Raindrops
{
    using Columns;

    public interface IModelObject
    {
        public void Update(GameTime gameTime);
    }

    public interface IRaindrop : IModelObject
    {
        public bool IsDead { get; }
        public string Symbol { get; }
        public Color Color { get; }
        public GridCoordinates Coordinates { get; }
    }
}
