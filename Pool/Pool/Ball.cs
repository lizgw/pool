using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pool
{
    class Ball
    {
        Vector2 pos; // center
        Vector2 velocity;
        double radius;
        double mass;
        double friction;
        double percentFrameComplete;
        protected Color color;

        public Ball()
        {
            pos = new Vector2(0, 0);
            velocity = new Vector2(0, 0);
            radius = 0;
            mass = 0;
            friction = 0;
            percentFrameComplete = 0;
            color = Color.White;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
