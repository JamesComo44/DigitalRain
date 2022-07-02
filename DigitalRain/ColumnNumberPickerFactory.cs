using System;

namespace DigitalRain
{
    using Columns;

    public class ColumnNumberPickerFactory
    {
        private readonly ColumnNumberPickerConfig _config;

        public ColumnNumberPickerFactory()
        {
            _config = DigitalRainGame.config.columnNumberPicker;
        }

        public IColumnNumberPicker Create()
        {
            return _config.type switch
            {
                "RoundRobinColumnNumberPicker" => new RoundRobinColumnNumberPicker(_config.columnCount),
                "RandomColumnNumberPicker" => new RandomColumnNumberPicker(_config.columnCount, _config.lowWaterMark),
                _ => throw new NotSupportedException("Invalid Column Number Picker Type"),
            };
        }
    }
}
