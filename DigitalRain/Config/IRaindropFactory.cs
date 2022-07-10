
namespace DigitalRain.Config
{
    using Grid;
    using Raindrop.Raindrops;

    public interface IRaindropFactory
    {
        public IRaindrop Create(GridCoordinates coordinates);
    }
}
