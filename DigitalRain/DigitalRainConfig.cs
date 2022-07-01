using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

namespace DigitalRain
{
    public class ColumnNumberPickerConfig
    {
        [JsonProperty(Required = Required.Always)]
        public string type;

        [JsonProperty(Required = Required.Always)]
        public int columnCount;

        public int lowWaterMark = 1;
    }

    public class DigitalRainConfig
    {
        public ColumnNumberPickerConfig columnNumberPicker;
    }

    public class ConfigReader
    {
        public DigitalRainConfig ReadConfig(string filename)
        {
            var json = File.ReadAllText(filename);
            var config = JsonConvert.DeserializeObject<DigitalRainConfig>(json);
            return config;
        }
    }
}