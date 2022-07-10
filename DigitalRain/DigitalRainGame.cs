using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DigitalRain
{
    using Config;
    using Grid;
    using Raindrop;
    using Raindrop.Raindrops;

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
        private UnoccupiedColumnPool _columnPool;
        private RaindropStreamFactory _streamFactory;
        private List<RaindropStream> _raindropStreams;
        private double _lastRaindropStreamCreationTimeInSeconds;
        private float _currentFontHeight;

        public DigitalRainGame(DigitalRainConfig config)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Config = config;
        }

        protected override void Initialize()
        {
            ConfigDebugEdit = new DebugConfigEditor(_graphics.GraphicsDevice);

            _screenBounds = _graphics.GraphicsDevice.Viewport.Bounds;

            ConfigurationProfile configProfile = ConfigurationProfile.ConfigurationProfiles[Config.profile];
            _columnPool = new UnoccupiedColumnPool(configProfile.ColumnNumberPicker, _screenBounds);
            _streamFactory = new RaindropStreamFactory(configProfile.RaindropFactory);

            _config = Config.streamSpawner;
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

            foreach (var raindropStream in _raindropStreams)
            {
                raindropStream.Update(gameTime);
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
                if (!_columnPool.IsLow)
                {
                    _lastRaindropStreamCreationTimeInSeconds = gameTime.TotalGameTime.TotalSeconds;
                    var column = _columnPool.PickOne();
                    var raindropStream = _streamFactory.Create(column, _currentFontHeight);
                    _raindropStreams.Add(raindropStream);
                }
            }
        }

        private void RemoveDeadRaindropStreams()
        {
            var columnsToRestore = _raindropStreams
                .Where(stream => stream.IsDead)
                .Select(stream => stream.Column)
                .ToHashSet();

            _raindropStreams = _raindropStreams
                .Where(stream => !stream.IsDead)
                .ToList();

            _columnPool.Restore(columnsToRestore);
        }

        private bool WasKeyPressed(Keys key)
        {
            return (Keyboard.GetState().IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key));
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);

            foreach (var raindropStream in _raindropStreams)
            {
                foreach (var raindrop in raindropStream)
                {
                    DrawRaindrop(raindrop);
                }
            }

            ConfigDebugEdit.Draw(_spriteBatch, _debugFont);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawRaindrop(IRaindrop raindrop)
        {
            var coordinates = raindrop.Coordinates;
            var position = new Vector2(coordinates.ColumnNumber * _columnPool.ColumnWidth, coordinates.RowNumber * _currentFontHeight);
            _spriteBatch.DrawString(_raindropFont, raindrop.Symbol, position, raindrop.Color);
        }
    }
}
