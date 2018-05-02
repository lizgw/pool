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
            mass = 5;
            friction = 0;
            percentFrameLeft = 1;
            color = Color.White;
            drawRect = new Rectangle(0, 0, (int)(radius * 2), (int)(radius * 2));
        }

        public Ball(Texture2D _texture) : this() //this() calls the default constructor so we don't have to write all those values twice
        {
            texture = _texture;
        }

        public Ball(Texture2D aTexture, Vector2 aPosition) : this()
        {
            pos = aPosition;
            texture = aTexture;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            drawRect.X = (int)(pos.X - radius);
            drawRect.Y = (int)(pos.Y - radius);
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

        public double GetMass()
        {
            return mass;
        }

        public double GetPercentFrameLeft()
        {
            return percentFrameLeft;
        }

        public Rectangle GetDrawRect()
        {
            return drawRect;
        }

        // moves ball along anglular path
        public void Move(float angle)
        {
            Vector2 newPos = new Vector2((float)(Math.Cos(angle) * velocity.X), (float)(Math.Sin(angle) * velocity.Y * -1));
            SetPos(new Vector2(newPos.X + pos.X, newPos.Y + pos.Y));
        }
    }
}
