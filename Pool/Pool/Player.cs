using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pool
{
    class Player : Ball
    {
        int points;
        PlayerIndex playerIndex;

        public Player(Color aColor) : base()
        {
            points = 100;
            color = aColor;
        }

        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void HandleInput()
        {

        }
    }
}
