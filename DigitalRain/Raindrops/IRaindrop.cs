using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalRain.Raindrops
{
    interface IRaindrop
    {
        char[] SymbolPool { get; }
        char Symbol { get; }
        double LifeRemaining { get; }
        static Color DefaultColor { get; }

        bool IsDead();
        string SymbolAsStr();
        char GetSymbolFromPool();

        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, SpriteFont font);
    }
}
