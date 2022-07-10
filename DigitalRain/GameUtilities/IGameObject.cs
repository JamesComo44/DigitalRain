using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalRain.GameUtilities
{
    public interface IGameObject : IModelObject
    {
        public void Draw(SpriteBatch spriteBatch, SpriteFont font);
    }
}
