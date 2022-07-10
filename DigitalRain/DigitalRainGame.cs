using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

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
        private UnoccupiedColumnPool _columnPool;
        private RaindropStreamFactory _streamFactory;
        private List<(Column, RaindropStream)> _raindropStreams;
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
            _columnPool = new UnoccupiedColumnPool(columnNumberPicker, _screenBounds);
            //var raindropFactory = new PerColumnSpaceRaindropFactory();
            var raindropFactory = new StandardRaindropFactory();
            var streamFactory = new RaindropStreamFactory(raindropFactory);

            _config = DigitalRainGame.Config.streamSpawner;
            _streamFactory = streamFactory;
            _raindropStreams = new List<(Column, RaindropStream)>();
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

            foreach (var (_, raindropStream) in _raindropStreams)
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
                    _raindropStreams.Add((column, raindropStream));
                }
            }
        }

        private void RemoveDeadRaindropStreams()
        {
            var columnsToRestore = _raindropStreams
                .Where(item => {
                    var (_, stream) = item;
                    return stream.IsDead;
                })
                .Select(item => {
                    var (column, _) = item;
                    return column;
                })
                .ToHashSet();

            _raindropStreams = _raindropStreams
                .Where(item => {
                    var (_, stream) = item;
                    return !stream.IsDead;
                })
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

            foreach (var (column, raindropStream) in _raindropStreams)
            {
                foreach (var (raindrop, rowNumber) in raindropStream.Select((raindrop, rowNumber) => (raindrop, rowNumber)))
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
            _spriteBatch.DrawString(_raindropFont, raindrop.Symbol, raindrop.Position, raindrop.Color);
        }
    }
}
