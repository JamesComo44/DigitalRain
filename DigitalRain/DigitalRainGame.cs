using System.Collections.Generic;
using System.Linq;
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
        private int _screenWidth;
        private int _screenHeight;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private float _fontHeight;
        private UnoccupiedColumnPool _columnPool;
        private RaindropStreamFactory _streamFactory;
        private RaindropStreams _raindropStreams;

        public DigitalRainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Rectangle bounds = _graphics.GraphicsDevice.Viewport.Bounds;
            _screenWidth = bounds.Width;
            _screenHeight = bounds.Height;

            var columnNumberPicker = new RandomColumnNumberPicker(columnCount: 50);
            _columnPool = new UnoccupiedColumnPool(columnNumberPicker, _graphics.GraphicsDevice.Viewport.Bounds);

            _streamFactory = new RaindropStreamFactory(_columnPool);
            _raindropStreams = new RaindropStreams();

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

            AddNewRaindropStreams(gameTime);
            RemoveDeadRaindropStreams();
            _raindropStreams.Update(gameTime);

            base.Update(gameTime);
        }

        private void AddNewRaindropStreams(GameTime gameTime)
        {
            float newRaindropStreamsPerSecond = 3;
            float secondsPerNewRaindropStream = 1 / newRaindropStreamsPerSecond;
            var numRaindropStreams = (int)(gameTime.TotalGameTime.TotalSeconds / secondsPerNewRaindropStream);
            while (!_columnPool.IsEmpty && _raindropStreams.Count < numRaindropStreams)
            {
                var raindropStream = _streamFactory.Create(_fontHeight);
                _raindropStreams.Add(raindropStream);
            }
        }

        private void RemoveDeadRaindropStreams()
        {
            var deadStreams = _raindropStreams.RemoveDeadStreams();
            var columnsToRestore = from stream in deadStreams select stream.Column;
            _columnPool.Restore(new HashSet<Column>(columnsToRestore));
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);

            _raindropStreams.Draw(gameTime, _spriteBatch, _font);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        //TODO: Test code don't forget to remove this!
        // DEBUG
        private void TestSamplerState()
        {
            string text = "Hello World S A 10 893354 I l L I";
            Vector2 textMiddlePoint = _font.MeasureString(text) / 2;
            Vector2 position = new Vector2(_screenWidth / 2f, _screenHeight / 2.3f);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            string draw_text = text + " PointClamp";
            position = new Vector2(_screenWidth / 2.5f, _screenHeight / 2.1f);
            _spriteBatch.DrawString(_font, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp);
            draw_text = text + " AnisotropicClamp";
            position = new Vector2(_screenWidth / 2.5f, _screenHeight / 2.6f);
            _spriteBatch.DrawString(_font, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp);
            draw_text = text + " LinearClamp";
            position = new Vector2(_screenWidth / 2.5f, _screenHeight / 3f);
            _spriteBatch.DrawString(_font, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap);
            draw_text = text + " PointWrap";
            position = new Vector2(_screenWidth / 2.5f, _screenHeight / 3.5f);
            _spriteBatch.DrawString(_font, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearWrap);
            draw_text = text + " LinearWrap";
            position = new Vector2(_screenWidth / 2.5f, _screenHeight / 4.5f);
            _spriteBatch.DrawString(_font, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap);
            draw_text = text + " AnisotropicWrap";
            position = new Vector2(_screenWidth / 2.5f, _screenHeight / 5.8f);
            _spriteBatch.DrawString(_font, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();
        }
    }
}
