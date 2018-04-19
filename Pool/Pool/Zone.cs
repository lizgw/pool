using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pool
{
    class Zone
    {
        Rectangle bounds;
        PlayerIndex playerIndex;

        public Zone(Rectangle aBounds, PlayerIndex aPlayerIndex)
        {
            bounds = aBounds;
            playerIndex = aPlayerIndex;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
