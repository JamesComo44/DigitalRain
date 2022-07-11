
using Microsoft.Xna.Framework.Graphics;

namespace DigitalRain.InputHandlers
{
    public interface IInputHandler
    {
        public void EnterInputMode();
        public void LeaveInputMode();
        public void HandleInput(InputController inputController);

        // TODO: This probably shouldn't be here.
        public void Draw(SpriteBatch spriteBatch, SpriteFont font);
    }
}
