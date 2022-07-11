using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DigitalRain
{
    using Config;
    using Grid;
    using Raindrop;
    using Raindrop.Raindrops;

    public enum GameMode
    {
        EnterFixedTextMode,
        DebugConfigEditorMode
    }

    public class AbortGameException : Exception
    {
        public AbortGameException(string message) : base(message) { }
    }

    public class InputController
    {
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        public InputController()
        {
            // "Prime" ourselves so both keyboard state variables have something valid.
            UpdateKeyboardState();
            UpdateKeyboardState();
        }

        public void Update(GameTime gameTime)
        {
            UpdateKeyboardState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || WasKeyPressed(Keys.Escape))
            {
                throw new AbortGameException("Bye!");
            }
        }

        public bool WasKeyPressed(Keys key)
        {
            return (Keyboard.GetState().IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key));
        }

        private void UpdateKeyboardState()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
        }
    }

    public interface IInputHandler
    {
        public void EnterInputMode();
        public void LeaveInputMode();
        public void HandleInput(InputController inputController);

        // TODO: This probably shouldn't be here.
        public void Draw(SpriteBatch spriteBatch, SpriteFont font);
    }

    public class DummyInputHandler : IInputHandler
    {
        public void EnterInputMode() { }
        public void LeaveInputMode() { }
        public void HandleInput(InputController inputController) { }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font) { }
    }

    public class FixedTextInputHandler : IInputHandler
    {
        public void EnterInputMode()
        {
            Debug.WriteLine("Entered FixedTextInputHandler mode");
        }

        public void LeaveInputMode()
        {
            Debug.WriteLine("Leave FixedTextInputHandler mode");
        }

        public void HandleInput(InputController inputController)
        {
            Debug.WriteLine("FixedTextInputHandler handled input");
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font) { }
    }

    public class DigitalRainGame : Game
    {
        public static DigitalRainConfig Config;

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

        private InputController _inputController;
        private GameMode _currentMode;
        private IInputHandler _currentInputHandler;
        private static DebugConfigEditor _configDebugEdit;
        private static FixedTextInputHandler _fixedTextInputHandler;

        public DigitalRainGame(DigitalRainConfig config)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Config = config;
        }

        protected override void Initialize()
        {
            _screenBounds = _graphics.GraphicsDevice.Viewport.Bounds;

            ConfigurationProfile configProfile = ConfigurationProfile.ConfigurationProfiles[Config.profile];
            _columnPool = new UnoccupiedColumnPool(configProfile.ColumnNumberPicker, _screenBounds);
            _streamFactory = new RaindropStreamFactory(configProfile.RaindropFactory);

            _config = Config.streamSpawner;
            _raindropStreams = new List<RaindropStream>();
            _lastRaindropStreamCreationTimeInSeconds = 0;
            _currentFontHeight = 0;

            _inputController = new InputController();

            _configDebugEdit = new DebugConfigEditor(_graphics.GraphicsDevice);
            _fixedTextInputHandler = new FixedTextInputHandler();
            _currentInputHandler = new DummyInputHandler();  // Just needed to make the first transition go smoothly.

            TransitionToMode(GameMode.EnterFixedTextMode);

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
            _configDebugEdit.UnloadTextures();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            try
            {
                _inputController.Update(gameTime);
            }
            catch (AbortGameException exc)
            {
                Exit();
            }

            AddNewRaindropStreams(gameTime);
            RemoveDeadRaindropStreams();

            foreach (var raindropStream in _raindropStreams)
            {
                raindropStream.Update(gameTime);
            }

            if (_inputController.WasKeyPressed(Keys.OemTilde))
            {
                if (_currentMode != GameMode.DebugConfigEditorMode)
                {
                    TransitionToMode(GameMode.DebugConfigEditorMode);
                }
                else
                {
                    TransitionToMode(GameMode.EnterFixedTextMode);
                }
            }
            _currentInputHandler.HandleInput(_inputController);

            base.Update(gameTime);            
        }

        private void TransitionToMode(GameMode toMode)
        {
            _currentInputHandler.LeaveInputMode();

            switch(toMode)
            {
                case GameMode.DebugConfigEditorMode:
                    _currentInputHandler = _configDebugEdit;
                    break;
                case GameMode.EnterFixedTextMode:
                    _currentInputHandler = _fixedTextInputHandler;
                    break;
                default:
                    break;
            }

            _currentMode = toMode;
            _currentInputHandler.EnterInputMode();
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

            // TODO:  Input Handler probable shouldn't know how to Draw()
            _currentInputHandler.Draw(_spriteBatch, _debugFont);
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
