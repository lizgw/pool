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

        public static Color[] colors = { Color.DarkOrange, Color.Purple, Color.LightGreen };
        
        public Powerup(PowerupType aType) : base()
        {
            type = aType;
            color = colors[(int)type];
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
