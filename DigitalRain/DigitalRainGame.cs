using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DigitalRain
{
    using Columns;
    using Raindrops;

    public class DigitalRainGame : Game
    {
        public static DigitalRainConfig Config;
        public static DebugConfigEditor ConfigDebugEdit;

        private GraphicsDeviceManager _graphics;
        private Rectangle _screenBounds;
        private SpriteBatch _spriteBatch;
        private SpriteFont _raindropFont;
        private SpriteFont _debugFont;

        private StreamSpawner _spawner;

        public DigitalRainGame(DigitalRainConfig config)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            DigitalRainGame.Config = config;
        }

        protected override void Initialize()
        {
            DigitalRainGame.ConfigDebugEdit = new DebugConfigEditor(_graphics.GraphicsDevice);

            _screenBounds = _graphics.GraphicsDevice.Viewport.Bounds;

            var columnNumberPickerFactory = new ColumnNumberPickerFactory();
            var columnNumberPicker = columnNumberPickerFactory.Create();
            var columnPool = new UnoccupiedColumnPool(columnNumberPicker, _screenBounds);
            var raindropFactory = new StandardRaindropFactory();
            var streamPool = new RaindropStreamPool(columnPool, raindropFactory);
            _spawner = new StreamSpawner(streamPool);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _debugFont = Content.Load<SpriteFont>("Fonts/debug");
            _raindropFont = Content.Load<SpriteFont>("Fonts/raindrop");
            _spawner.SetFontHeight(_raindropFont.MeasureString("A").Y);
        }

        protected override void UnloadContent()
        {
            ConfigDebugEdit.UnloadTextures();
            base.UnloadContent();
        }

        KeyboardState previousKeyboardState;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _spawner.Update(gameTime);

            // In-Game Debug Controls
            if (WasKeyPressed(Keys.OemTilde))
                ConfigDebugEdit.ToggleActiveMode();
            if (WasKeyPressed(Keys.Enter))
                ConfigDebugEdit.ToggleEditingMode();
            if (WasKeyPressed(Keys.Up))
                ConfigDebugEdit.IncrementTargetIndex();
            if (WasKeyPressed(Keys.Down))
                ConfigDebugEdit.DecrementTargetIndex();

            previousKeyboardState = Keyboard.GetState();
            base.Update(gameTime);
        }

        private bool WasKeyPressed(Keys key)
        {
            return (Keyboard.GetState().IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key));
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            _spawner.Draw(_spriteBatch, _raindropFont);
            ConfigDebugEdit.Draw(_spriteBatch, _debugFont);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        //TODO: Test code don't forget to remove this!
        // DEBUG
        private void TestSamplerState()
        {
            string text = "Hello World S A 10 893354 I l L I";
            Vector2 textMiddlePoint = _raindropFont.MeasureString(text) / 2;
            Vector2 position;

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            string draw_text = text + " PointClamp";
            position = new Vector2(_screenBounds.X / 2.5f, _screenBounds.Y / 2.1f);
            _spriteBatch.DrawString(_raindropFont, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp);
            draw_text = text + " AnisotropicClamp";
            position = new Vector2(_screenBounds.X / 2.5f, _screenBounds.Y / 2.6f);
            _spriteBatch.DrawString(_raindropFont, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp);
            draw_text = text + " LinearClamp";
            position = new Vector2(_screenBounds.X / 2.5f, _screenBounds.Y / 3f);
            _spriteBatch.DrawString(_raindropFont, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap);
            draw_text = text + " PointWrap";
            position = new Vector2(_screenBounds.X / 2.5f, _screenBounds.Y / 3.5f);
            _spriteBatch.DrawString(_raindropFont, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearWrap);
            draw_text = text + " LinearWrap";
            position = new Vector2(_screenBounds.X / 2.5f, _screenBounds.Y / 4.5f);
            _spriteBatch.DrawString(_raindropFont, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap);
            draw_text = text + " AnisotropicWrap";
            position = new Vector2(_screenBounds.X / 2.5f, _screenBounds.Y / 5.8f);
            _spriteBatch.DrawString(_raindropFont, draw_text, position, Color.GreenYellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();
        }
    }
}
