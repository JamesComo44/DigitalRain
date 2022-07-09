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
            // TODO: Switch between these configurably
            //var raindropFactory = new StandardRaindropFactory();
            var raindropFactory = new PerColumnSpaceRaindropFactory();
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
    }
}
