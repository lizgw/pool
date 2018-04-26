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
        double percentFrameLeft;

        protected Color color;
        public Texture2D texture;
        public Ball()
        {

            pos = new Vector2(0, 0);
            velocity = new Vector2(0, 0);
            radius = 0;
            mass = 0;
            friction = 0;
            percentFrameLeft = 0;
            color = Color.White;
        }
        public Ball(Texture2D _texture)
        {
            pos = new Vector2(0, 0);
            velocity = new Vector2(0, 0);
            radius = 0;
            mass = 0;
            friction = 0;
            percentFrameLeft = 1;
            color = Color.White;
            texture = _texture;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, pos, color);
        }

        public Ball Copy()
        {
            Ball output = new Ball();
            output.pos = pos;
            output.velocity = velocity;
            output.radius = radius;
            output.mass = mass;
            output.friction = friction;
            output.percentFrameLeft = percentFrameLeft;
            output.color = color;
            return output;
        }

        public void SetPos(Vector2 aPos)
        {
            pos = aPos;
        }

        public void SetVelocity(Vector2 aVelocity)
        {
            velocity = aVelocity;
        }

        //probably only really useful for debugging purposes
        public void SetColor(Color aColor)
        {
            color = aColor;
        }

        public Vector2 GetPos()
        {
            return pos;
        }

        public Vector2 GetVelocity()
        {
            return velocity;
        }

        public double GetRadius()
        {
            return radius;
        }

        public double GetPercentFrameLeft()
        {
            return radius;
        }
    }
}
