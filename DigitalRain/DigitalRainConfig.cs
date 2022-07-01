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

    public class StreamSpawnerConfig
    {
        [JsonProperty(Required = Required.Always)]
        public double minSecondsPerNewRaindropStream;
    }
    public class RaindropStreamPoolConfig
    {
        [JsonProperty(Required = Required.Always)]
        public int streamFallSpeedMin;        
        [JsonProperty(Required = Required.Always)]
        public int streamFallSpeedMax;
    }

    public class StandardRaindropFactoryConfig
    {
        [JsonProperty(Required = Required.Always)]
        public int lifespanMin;
        [JsonProperty(Required = Required.Always)]
        public int lifespanMax;
    }

    public class DigitalRainConfig
    {
        public ColumnNumberPickerConfig columnNumberPicker;
        public StreamSpawnerConfig streamSpawner;
        public RaindropStreamPoolConfig raindropStreamPool;
        public StandardRaindropFactoryConfig standardRaindropFactory;
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