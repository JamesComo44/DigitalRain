
namespace DigitalRain.Raindrops
{
    using Columns;

    public interface IRaindropFactory
    {
        public IRaindrop Create(GridCoordinates coordinates);
    }
}
