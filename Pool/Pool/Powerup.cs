using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pool
{
    public enum PowerupType
    {
        Bomb,
        Stamina,
        BigBall
    }

    class Powerup : Ball
    {
        PowerupType type;
        Texture2D ptexture ;
        
        public Powerup(PowerupType aType)
        {
            texture = ptexture;
            
            type = aType;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
