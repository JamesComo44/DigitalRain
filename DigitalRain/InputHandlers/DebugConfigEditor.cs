﻿using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DigitalRain.InputHandlers
{
    public class DebugConfigEditor : IInputHandler
    {
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

        public void EnterInputMode()
        {
            LoadTextures();
        }

        public void LeaveInputMode()
        {
            UnloadTextures();
        }

        public void HandleInput(InputController inputController)
        {
            if (inputController.WasKeyPressed(Keys.Enter))
            {
                ToggleEditingMode();
            }
            if (inputController.WasKeyPressed(Keys.Up))
            {
                IncrementTargetIndex();
            }
            if (inputController.WasKeyPressed(Keys.Down))
            {
                DecrementTargetIndex();
            }
        }

        public void ToggleEditingMode()
        {
            IsEditing = !IsEditing;
        }

        public void IncrementTargetIndex()
        {
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

        public void DecrementTargetIndex()
        {
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
            if (_backdropTexture != null)
                _backdropTexture.Dispose();
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (IsEditing)
            {
                string header = "Edit Config Value";
                Vector2 headerSize = font.MeasureString(header);
                Vector2 headerPos = Vector2.Zero;

                Vector2 editNameSize = font.MeasureString(_editName);
                Vector2 editNamePos = new Vector2(0, font.MeasureString(header).Y);

                Vector2 editValueSize = font.MeasureString(_editValue.ToString());
                Vector2 editValuePos = new Vector2(0, editNamePos.Y + font.MeasureString(_editName).Y);

                //TODO: Make work with edit stuff.
                spriteBatch.Draw(_backdropTexture, headerPos, null, Color.Blue, 0f, Vector2.Zero, headerSize, SpriteEffects.None, 0f);
                spriteBatch.Draw(_backdropTexture, editNamePos, null, Color.Blue, 0f, Vector2.Zero, editNameSize, SpriteEffects.None, 0f);
                spriteBatch.Draw(_backdropTexture, editValuePos, null, Color.Blue, 0f, Vector2.Zero, editValueSize, SpriteEffects.None, 0f);

                spriteBatch.DrawString(font, header, headerPos, Color.White);
                spriteBatch.DrawString(font, _editName, editNamePos, Color.White);
                spriteBatch.DrawString(font, _editValue.ToString(), editValuePos, Color.White);
            }

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
        }
    }
}
