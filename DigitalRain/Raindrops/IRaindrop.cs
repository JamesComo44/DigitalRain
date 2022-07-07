using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalRain.Raindrops
{
    using GameUtilities;

    public interface IRaindrop : IGameObject
    {
        public bool IsDead();
    }
}
