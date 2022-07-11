﻿using System.IO;
using Newtonsoft.Json;

namespace DigitalRain.Config
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
    public class RaindropStreamFactoryConfig
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
        public int profile;
        public ColumnNumberPickerConfig columnNumberPicker;
        public StreamSpawnerConfig streamSpawner;
        public RaindropStreamFactoryConfig raindropStreamFactory;
        public StandardRaindropFactoryConfig standardRaindropFactory;
    }

    public static class ConfigReader
    {
        public static DigitalRainConfig FromJson(string filename)
        {
            var json = File.ReadAllText(filename);
            var config = JsonConvert.DeserializeObject<DigitalRainConfig>(json);
            return config;
        }
    }
}