using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DigitalRain
{
    using Columns;
    using Raindrops;
    using System.Collections.Generic;
    using System.Linq;

    public class DigitalRainGame : Game
    {
        public static DigitalRainConfig Config;
        public static DebugConfigEditor ConfigDebugEdit;

        private GraphicsDeviceManager _graphics;
        private Rectangle _screenBounds;
        private SpriteBatch _spriteBatch;
        private SpriteFont _raindropFont;
        private SpriteFont _debugFont;

        private StreamSpawnerConfig _config;
        private RaindropStreamPool _streamPool;
        private List<RaindropStream> _raindropStreams;
        private double _lastRaindropStreamCreationTimeInSeconds;
        private float _currentFontHeight;

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
            var raindropFactory = new PerColumnSpaceRaindropFactory();
            var streamPool = new RaindropStreamPool(columnPool, raindropFactory);

            _config = DigitalRainGame.Config.streamSpawner;
            _streamPool = streamPool;
            _raindropStreams = new List<RaindropStream>();
            _lastRaindropStreamCreationTimeInSeconds = 0;
            _currentFontHeight = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _debugFont = Content.Load<SpriteFont>("Fonts/debug");
            _raindropFont = Content.Load<SpriteFont>("Fonts/raindrop");
            _currentFontHeight = _raindropFont.MeasureString("A").Y;
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

            AddNewRaindropStreams(gameTime);
            RemoveDeadRaindropStreams();
            foreach (var stream in _raindropStreams)
            {
                stream.Update(gameTime);
            }

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

        private void AddNewRaindropStreams(GameTime gameTime)
        {
            var timeElapsedSinceLastNewRaindropStream = gameTime.TotalGameTime.TotalSeconds - _lastRaindropStreamCreationTimeInSeconds;
            if (timeElapsedSinceLastNewRaindropStream > _config.minSecondsPerNewRaindropStream)
            {
                if (!_streamPool.IsLow)
                {
                    _lastRaindropStreamCreationTimeInSeconds = gameTime.TotalGameTime.TotalSeconds;
                    var raindropStream = _streamPool.Create(_currentFontHeight);
                    _raindropStreams.Add(raindropStream);
                }
            }
        }

        private void RemoveDeadRaindropStreams()
        {
            var deadStreams = _raindropStreams.Where((stream) => stream.IsDead).ToHashSet();
            _raindropStreams = _raindropStreams.Where((stream) => !stream.IsDead).ToList();
            _streamPool.Restore(deadStreams);
        }

        private bool WasKeyPressed(Keys key)
        {
            return (Keyboard.GetState().IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key));
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);

            foreach (var stream in _raindropStreams)
            {
                stream.Draw(_spriteBatch, _raindropFont);
            }

            ConfigDebugEdit.Draw(_spriteBatch, _debugFont);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
