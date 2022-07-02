using System;

namespace DigitalRain
{
    using Columns;

    public class ColumnNumberPickerFactory
    {
        private ColumnNumberPickerConfig _config;

        public ColumnNumberPickerFactory(ColumnNumberPickerConfig config)
        {
            _config = config;
        }

        public IColumnNumberPicker Create()
        {
            switch (_config.type)
            {
                case "RoundRobinColumnNumberPicker":
                    return new RoundRobinColumnNumberPicker(_config.columnCount);
                case "RandomColumnNumberPicker":
                    return new RandomColumnNumberPicker(_config.columnCount, _config.lowWaterMark);
                default:
                    throw new Exception("Bad config");
            }
        }
    }
}
