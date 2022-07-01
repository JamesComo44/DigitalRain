﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DigitalRain
{
    using Columns;
    using Raindrops;
    using System;

    public class DigitalRainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private Rectangle _screenBounds;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private float _fontHeight;

        private StreamSpawner _spawner;

        public DigitalRainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _lastRaindropStreamCreationTimeInSeconds = 0;
        }

        protected override void Initialize()
        {
            _screenBounds = _graphics.GraphicsDevice.Viewport.Bounds;
            _spawner = new StreamSpawner(_screenBounds);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Fonts/debug_font");
            _fontHeight = _font.MeasureString("A").Y;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _spawner.Update(gameTime, _fontHeight);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            _spawner.Draw(gameTime, _spriteBatch, _font);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        //TODO: Test code don't forget to remove this!
        // DEBUG
        private void TestSamplerState()
        {
            string text = "Hello World S A 10 893354 I l L I";
            Vector2 textMiddlePoint = _font.MeasureString(text) / 2;
            Vector2 position = new Vector2(_screenBounds.X / 2f, _screenBounds.Y / 2.3f);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            string draw_text = text + " PointClamp";
            position = new Vector2(_screenBounds.X / 2.5f, _screenBounds.Y / 2.1f);
            _spriteBatch.DrawString(_font, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp);
            draw_text = text + " AnisotropicClamp";
            position = new Vector2(_screenBounds.X / 2.5f, _screenBounds.Y / 2.6f);
            _spriteBatch.DrawString(_font, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp);
            draw_text = text + " LinearClamp";
            position = new Vector2(_screenBounds.X / 2.5f, _screenBounds.Y / 3f);
            _spriteBatch.DrawString(_font, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap);
            draw_text = text + " PointWrap";
            position = new Vector2(_screenBounds.X / 2.5f, _screenBounds.Y / 3.5f);
            _spriteBatch.DrawString(_font, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearWrap);
            draw_text = text + " LinearWrap";
            position = new Vector2(_screenBounds.X / 2.5f, _screenBounds.Y / 4.5f);
            _spriteBatch.DrawString(_font, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap);
            draw_text = text + " AnisotropicWrap";
            position = new Vector2(_screenBounds.X / 2.5f, _screenBounds.Y / 5.8f);
            _spriteBatch.DrawString(_font, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();
        }
    }
}
