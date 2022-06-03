using DigitalRain.Raindrops;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DigitalRain
{
    public class DigitalRainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private int _screenWidth;
        private int _screenHeight;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        //TODO: TESTING
        StandardRaindrop raindrop;

        public DigitalRainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _screenWidth = _graphics.PreferredBackBufferWidth;
            _screenHeight = _graphics.PreferredBackBufferHeight;

            //TODO: TESTING
            raindrop = new StandardRaindrop(0.06, new Vector2(_screenWidth / 2, _screenHeight / 2), StandardRaindrop.DefaultColor);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _font = Content.Load<SpriteFont>("Fonts/default_font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            raindrop.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            raindrop.Draw(_spriteBatch, _font);
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
