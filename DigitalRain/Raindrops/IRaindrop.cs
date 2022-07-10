using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalRain.Raindrops
{
    public interface IModelObject
    {
        public void Update(GameTime gameTime);
    }

    public interface IRaindrop : IModelObject
    {
        public bool IsDead { get; }
        public string Symbol { get; }
        public Color Color { get; }
        public Vector2 Position { get; }
    }
}
