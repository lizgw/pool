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

        public Color color;
        public Texture2D texture;
        Rectangle drawRect;

        public Ball()
        {

            pos = new Vector2(0, 0);
            velocity = new Vector2(0, 0);
            radius = 20;
            mass = 0;
            friction = 0;
            percentFrameLeft = 0;
            color = Color.White;
            drawRect = new Rectangle(0, 0, (int)(radius * 2), (int)(radius * 2));
        }
        public Ball(Texture2D _texture)
        {
            pos = new Vector2(0, 0);
            velocity = new Vector2(0, 0);
            radius = 20;
            mass = 0;
            friction = 0;
            percentFrameLeft = 1;
            color = Color.White;
            drawRect = new Rectangle(0, 0, (int)(radius * 2), (int)(radius * 2));
            texture = _texture;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            drawRect.X = (int)(pos.X - radius);
            drawRect.Y = (int)(pos.Y - radius);
            Console.WriteLine(drawRect.X + " " + drawRect.Y + " " + drawRect.Width + " " + drawRect.Height);
            spriteBatch.Draw(texture, drawRect, color);
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
            output.texture = texture;
            output.drawRect = drawRect;
            return output;
        }

        //SETTERS

        public void SetPos(Vector2 aPos)
        {
            pos = aPos;
        }

        public void SetVelocity(Vector2 aVelocity)
        {
            velocity = aVelocity;
        }

        public void SetPercentFrameLeft(double aPercentFrameLeft)
        {
            percentFrameLeft = aPercentFrameLeft;
        }

        //probably only really useful for debugging purposes
        public void SetColor(Color aColor)
        {
            color = aColor;
        }

        //GETTERS

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
            return percentFrameLeft;
        }

        // moves ball along anglular path
        public void Move(float angle)
        {
            Vector2 newPos = new Vector2((float)(Math.Cos(angle) * velocity.X), (float)(Math.Sin(angle) * velocity.Y * -1));
            SetPos(new Vector2(newPos.X + pos.X, newPos.Y + pos.Y));
        }
    }
}
