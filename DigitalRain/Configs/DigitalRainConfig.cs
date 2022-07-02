using Newtonsoft.Json;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;

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

    public class DebugConfigEditor
    {
        public bool IsActive { get; private set; }
        public bool IsEditing { get; private set; }
        private string _editKey;
        private string _editValue;
        private Dictionary<string, object> _editableConfigs;

        private List<object> _gameConfigs;
        private int _currentIndex;
        private string _currentName;

        public DebugConfigEditor()
        {
            IsActive = false;
            IsEditing = false;
            _editKey = "EDIT MODE";
            _editValue = "VALUE 123";

            //var fieldValues = foo.GetType()
            //         .GetFields()
            //         .Select(field => field.GetValue(foo))
            //         .ToList();

            Type digitalRainConfigType = DigitalRainGame.Config.GetType();
            FieldInfo[] configFields = digitalRainConfigType.GetFields(BindingFlags.Instance | BindingFlags.Public);

            _gameConfigs = new List<object>();
            foreach (FieldInfo field in configFields)
                _gameConfigs.Add(field.GetValue(DigitalRainGame.Config));

            for (int i = 0; i < _gameConfigs.Count; i++)
            {
                var fieldValues = _gameConfigs[i].GetType().GetFields();
                //string key = // WTF
                //_editableConfigs.Add()
            }

            _currentIndex = 0;
            _currentName = GetConfigName();
        }

        public void ToggleActiveMode()
        {
            IsActive = !IsActive;
        }

        public void ToggleEditingMode()
        {
            if (IsActive)
                IsEditing = !IsEditing;
        }

        public void IncrementIndex()
        {
            if (!IsActive)
                return;

            if (!IsEditing)
                _currentIndex++;

            if (_currentIndex >= _gameConfigs.Count)
                _currentIndex = 0;

            //TODO: If is editing, what then?
            _currentName = GetConfigName();
        }

        public void DecrementIndex()
        {
            if (!IsActive)
                return;

            if (!IsEditing)
                _currentIndex--;

            if (_currentIndex < 0)
                _currentIndex = _gameConfigs.Count - 1;

            //TODO: If is editing, what then?
            _currentName = GetConfigName();
        }

        private string GetConfigName()
        {
            return _gameConfigs[_currentIndex].GetType().Name;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (!IsActive)
                return;

            if (!IsEditing)
            {
                Vector2 namePos = new Vector2(0, font.MeasureString(_currentName).Y);
                spriteBatch.DrawString(font, "Select A Config To Edit", Vector2.Zero, Color.White);
                spriteBatch.DrawString(font, _currentName, namePos, Color.White);
            }

            if (IsEditing)
            {
                Vector2 valuePos = new Vector2(0, font.MeasureString(_editKey).Y);
                spriteBatch.DrawString(font, _editKey, Vector2.Zero, Color.White);
                spriteBatch.DrawString(font, _editValue, valuePos, Color.White);
            }
        }
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