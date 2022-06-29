using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DigitalRain
{
    public class DigitalRainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private Vector2 _helloWorldPosition;
        private Vector2 _helloWorldShadowPosition;
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
            _helloWorldPosition = new Vector2(-20, -20);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteFont = Content.Load<SpriteFont>("default");

            int columnCount = 35;
            var columnNumberPicker = new RoundRobinColumnNumberPicker(columnCount);
            Rectangle bounds = _graphics.GraphicsDevice.Viewport.Bounds;
            UnoccupiedColumnPool columnPool = new UnoccupiedColumnPool(columnNumberPicker, bounds.Width);
            _streamFactory = new RaindropStreamFactory(_spriteBatch, _spriteFont, columnPool);
            _raindropStreams = new RaindropStreams();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _helloWorldPosition = _helloWorldPosition + new Vector2(1, 1);
            Rectangle bounds = _graphics.GraphicsDevice.Viewport.Bounds;
            if (_helloWorldPosition.X > bounds.Width || _helloWorldPosition.Y > bounds.Height)
            {
                _helloWorldPosition = new Vector2(-20, -20);
            }
            _helloWorldShadowPosition = _helloWorldPosition - new Vector2(2, 2);

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

            _spriteBatch.Begin();

            _raindropStreams.Draw(gameTime);
            //_spriteBatch.DrawString(_spriteFont, "Hello, World!", _helloWorldShadowPosition, new Color(Color.YellowGreen, 0.2f));
            //_spriteBatch.DrawString(_spriteFont, "Hello, World!", _helloWorldPosition, Color.GreenYellow);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
