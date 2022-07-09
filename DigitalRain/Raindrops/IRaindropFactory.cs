using System;

namespace DigitalRain.Raindrops
{
    using Columns;

    public interface IRaindropFactory
    {
        public IRaindrop Create(ColumnSpace space);
    }
}
