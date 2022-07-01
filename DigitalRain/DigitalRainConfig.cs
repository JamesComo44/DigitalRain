using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

namespace DigitalRain
{
    public class DigitalRainConfig
    {
        public int columnCount;
        public int columnLowWaterMark;
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