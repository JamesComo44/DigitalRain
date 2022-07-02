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

        private readonly List<List<FieldInfo>> _editableConfigs;
        private readonly List<object> _gameConfigs;

        private int _currentIndex;
        private string _currentName;

        private int _editIndex;
        private string _editName;
        private object _editValue;

        private GraphicsDevice _graphicsDevice;
        private Texture2D _backdropTexture;

        public DebugConfigEditor(GraphicsDevice graphics)
        {
            _graphicsDevice = graphics;

            IsActive = false;
            IsEditing = false;

            Type digitalRainConfigType = DigitalRainGame.Config.GetType();
            FieldInfo[] configFields = digitalRainConfigType.GetFields(BindingFlags.Instance | BindingFlags.Public);

            _gameConfigs = new List<object>();
            _editableConfigs = new List<List<FieldInfo>>();

            foreach (FieldInfo field in configFields)
                _gameConfigs.Add(field.GetValue(DigitalRainGame.Config));

            for (int i = 0; i < _gameConfigs.Count; i++)
            {
                List<FieldInfo> configProperties = new List<FieldInfo>();

                var fieldValues = _gameConfigs[i].GetType().GetRuntimeFields();
                foreach (var field in fieldValues)
                    configProperties.Add(field);

                _editableConfigs.Add(configProperties);
            }

            _currentIndex = 0;
            _currentName = GetConfigName();

            _editIndex = 0;
            _editName = GetEditName();
            _editValue = GetEditValue();
        }

        public void ToggleActiveMode()
        {
            IsActive = !IsActive;

            if (IsActive)
                LoadTextures();
            else
                UnloadTextures();
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

            if (IsEditing)
                _editIndex++;
            else
            {
                _currentIndex++;
                // By setting this here, we remember edit index unless changing config.
                _editIndex = 0;
            }

            ClampIndexes();
            _currentName = GetConfigName();
            _editName = GetEditName();
            _editValue = GetEditValue();
        }

        public void DecrementIndex()
        {
            if (!IsActive)
                return;

            if (IsEditing)
                _editIndex--;
            else
            {
                _currentIndex--;
                // By setting this here, we remember edit index unless changing config.
                _editIndex = 0;
            }

            ClampIndexes();
            _currentName = GetConfigName();
            _editName = GetEditName();
            _editValue = GetEditValue();
        }

        private void ClampIndexes()
        {
            if (_currentIndex >= _gameConfigs.Count)
                _currentIndex = 0;

            if (_currentIndex < 0)
                _currentIndex = _gameConfigs.Count - 1;

            if (_editIndex >= _editableConfigs[_currentIndex].Count)
                _editIndex = 0;

            if (_editIndex < 0)
                _editIndex = _editableConfigs[_currentIndex].Count - 1;
        }

        private string GetConfigName()
        {
            return _gameConfigs[_currentIndex].GetType().Name;
        }

        private string GetEditName()
        {
            return _editableConfigs[_currentIndex][_editIndex].Name;
        }

        private object GetEditValue()
        {
            var currentConfig = _gameConfigs[_currentIndex];
            Type currentConfigType = currentConfig.GetType();
            var currentConfigFields = currentConfigType.GetFields();
            return currentConfigFields[_editIndex].GetValue(currentConfig);
        }

        public void LoadTextures()
        {
            _backdropTexture = new Texture2D(_graphicsDevice, 1, 1);
            _backdropTexture.SetData(new[] { Color.White });
        }

        public void UnloadTextures()
        {
            _backdropTexture.Dispose();
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (!IsActive)
                return;

            if (!IsEditing)
            {
                string header = "Select A Config To Edit";
                Vector2 headerSize = font.MeasureString(header);
                Vector2 headerPos = Vector2.Zero;

                Vector2 propertyNameSize = font.MeasureString(_currentName);
                Vector2 namePos = new Vector2(0, propertyNameSize.Y);

                spriteBatch.Draw(_backdropTexture, headerPos, null, Color.Blue, 0f, Vector2.Zero, headerSize, SpriteEffects.None, 0f);
                spriteBatch.Draw(_backdropTexture, namePos, null, Color.Blue, 0f, Vector2.Zero, propertyNameSize, SpriteEffects.None, 0f);
                
                spriteBatch.DrawString(font, header, headerPos, Color.White);
                spriteBatch.DrawString(font, _currentName, namePos, Color.White);
            }

            if (IsEditing)
            {
                Vector2 valuePos = new Vector2(0, font.MeasureString(_editName).Y);

                //TODO: Make work with edit stuff.
                //spriteBatch.Draw(_backdropTexture, headerPos, null, Color.Blue, 0f, Vector2.Zero, headerSize, SpriteEffects.None, 0f);
                //spriteBatch.Draw(_backdropTexture, namePos, null, Color.Blue, 0f, Vector2.Zero, propertyNameSize, SpriteEffects.None, 0f);

                spriteBatch.DrawString(font, _editName, Vector2.Zero, Color.White);
                spriteBatch.DrawString(font, _editValue.ToString(), valuePos, Color.White);
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