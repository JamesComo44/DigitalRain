using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DigitalRain.InputHandlers
{
    using Raindrop;
    using Config;

    public class RotateColorInputHandler : IInputHandler
    {
        private int _currentIndex;
        private List<IRaindropFactory> _raindropFactories;

        private DigitalRainGame _game;
        private RaindropStreamFactory StreamFactory { get { return _game.StreamFactory; } }
        private List<RaindropStream> RaindropStreams { get { return _game.RaindropStreams; } }

        public RotateColorInputHandler(DigitalRainGame game)
        {
            _game = game;
            _currentIndex = 0;
            _raindropFactories = new List<IRaindropFactory>
            {
                ConfigurationProfile.RaindropFactories["randomgreen"],
                ConfigurationProfile.RaindropFactories["randompink"],
                ConfigurationProfile.RaindropFactories["randomred"]
            };
            SetRaindropFactory();
        }

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
            if (inputController.WasKeyPressed(Keys.Space) || inputController.WasKeyPressed(Keys.Enter))
            {
                _currentIndex = (_currentIndex + 1) % _raindropFactories.Count;
                SetRaindropFactory();
            }

            if(inputController.WasKeyPressed(Keys.Enter))
            {
                // The "current" ColorCalculatorFactory is going to be used for all
                // future raindrop streams.  But here we're trying to:
                //     1. give all existing raindrop streams a new raindrop factory
                //     2. give all existing raindrops a new ColorCalculator
                // This will tide them over until the end of their life.  We base
                // the new objects off the one we'll be using for future streams.

                foreach (var raindropStream in RaindropStreams)
                {
                    raindropStream.RaindropFactory = CurrentRaindropFactory;

                    foreach (var raindrop in raindropStream._raindrops)
                    {
                        raindrop.ColorCalculator = new ColorCalculator(
                            timespan: raindrop.Lifespan,
                            startColor: raindrop.Color,
                            endColor: CurrentRaindropFactory.ColorCalculatorFactory.EndColor,
                            lerpTime: CurrentRaindropFactory.ColorCalculatorFactory.LerpTime
                        );
                    }
                }
            }
        }

        private IRaindropFactory CurrentRaindropFactory 
        {
            get { return _raindropFactories[_currentIndex]; } 
        }

        private void SetRaindropFactory()
        {
            StreamFactory.RaindropFactory = CurrentRaindropFactory;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font) { }
    }
}
