using DigitalRain.Raindrops;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DigitalRain
{
    public class DigitalRainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private int _screenWidth;
        private int _screenHeight;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private RaindropStreamFactory _streamFactory;
        private RaindropStreams _raindropStreams;

        //TODO: TESTING
        List<StandardRaindrop> _raindrops;


        public DigitalRainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _screenWidth = _graphics.PreferredBackBufferWidth;
            _screenHeight = _graphics.PreferredBackBufferHeight;

            //TODO: TESTING
            int raindropsToSpawn = 50;
            _raindrops = new List<StandardRaindrop>(raindropsToSpawn);

            float screenSubdivisions = _screenWidth / raindropsToSpawn;
            for (int i = 1; i < raindropsToSpawn + 1; i++)
            {
                double lifeSpan = 10000.0;
                Vector2 initialPosition = new Vector2(i * screenSubdivisions, 0);
                Color color = StandardRaindrop.DefaultColor;
                _raindrops.Add(new StandardRaindrop(lifeSpan, initialPosition, color));
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Fonts/debug_font");

            int columnCount = 35;
            var columnNumberPicker = new RoundRobinColumnNumberPicker(columnCount);
            Rectangle bounds = _graphics.GraphicsDevice.Viewport.Bounds;
            UnoccupiedColumnPool columnPool = new UnoccupiedColumnPool(columnNumberPicker, bounds.Width);
            _streamFactory = new RaindropStreamFactory(_spriteBatch, _font, columnPool);
            _raindropStreams = new RaindropStreams();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (StandardRaindrop raindrop in _raindrops)
            {
                raindrop.Update(gameTime);
            }

            var wholeSecondsElapsed = (int)(gameTime.TotalGameTime.TotalSeconds);
            while (_raindropStreams.Count < wholeSecondsElapsed)
            {
                var raindropStream = _streamFactory.Create();
                _raindropStreams.Add(raindropStream);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);

            foreach (StandardRaindrop raindrop in _raindrops)
            {
                raindrop.Draw(_spriteBatch, _font);
            }

            _raindropStreams.Draw(gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        //TODO: Test code don't forget to remove this!
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
