using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Pool
{
    static class Physics
    {
        //It's possible that we'll want the balls to bounce off of more things than
        //other balls and a single rectangle, but we can look into doing that later.
        public static void Update(List<Ball> balls, Rectangle tableBounds)
        {
            //iterates through all the two-ball combinations, but doesn't repeat identical combinations in reverse order
            for(int b1 = 0; b1 < balls.Count; b1++)
            {
                for (int b2 = balls.Count - 1; b2 > b1; b2--)
                {
                    Ball ball1 = balls[b1];
                    Ball ball2 = balls[b2];
                    if (TryCollide(ball1, ball2))
                    {
                        ball1.SetColor(Color.Red);
                        ball2.SetColor(Color.Red);
                    }
                    else
                    {
                        ball1.SetColor(Color.Green);
                        ball2.SetColor(Color.Green);
                    }
                }
            }

            //collides off of walls and also continues motion of balls with new velocity vectors
            for (int b = 0; b < balls.Count; b++)
            {
                Ball ball = balls[b];

                //wall collision
                if ((ball.GetPos().X - ball.GetRadius() <= tableBounds.Left && ball.GetVelocity().X < 0) ||
                    (ball.GetPos().X + ball.GetRadius() >= tableBounds.Right && ball.GetVelocity().X > 0))
                {
                    Vector2 newVel = ball.GetVelocity();
                    newVel.X *= -1;
                    ball.SetVelocity(newVel);
                }
                if ((ball.GetPos().Y - ball.GetRadius() <= tableBounds.Top && ball.GetVelocity().Y < 0) ||
                    (ball.GetPos().Y + ball.GetRadius() >= tableBounds.Bottom && ball.GetVelocity().Y > 0))
                {
                    Vector2 newVel = ball.GetVelocity();
                    newVel.Y *= -1;
                    ball.SetVelocity(newVel);
                }

                //continuing motion of balls

                if (ball.GetPercentFrameLeft() == 1)
                {
                    ball.SetPos(ball.GetPos() + ScalarProduct(ball.GetVelocity(), ball.GetPercentFrameLeft()));
                    ball.SetPercentFrameLeft(1);
                }
            }
        }

        private static bool TryCollide(Ball ball1, Ball ball2)
        {
            Ball moveBall = ball1.Copy();
            Ball statBall = ball2.Copy();
            moveBall.SetVelocity(ScalarProduct(moveBall.GetVelocity(), moveBall.GetPercentFrameLeft()) - ScalarProduct(statBall.GetVelocity(), statBall.GetPercentFrameLeft()));
            statBall.SetVelocity(Vector2.Zero);

            Vector2 betweenCenters = statBall.GetPos() - moveBall.GetPos();

            double moveSpeed = moveBall.GetVelocity().Length();

            if (moveSpeed < betweenCenters.Length() - moveBall.GetRadius() - statBall.GetRadius())
                return false;

            if (DotProduct(moveBall.GetVelocity(), betweenCenters) <= 0)
                return false;

            Vector2 normalizedVel = new Vector2(moveBall.GetVelocity().X, moveBall.GetVelocity().Y);
            normalizedVel.Normalize();

            double closestDistSquared = betweenCenters.LengthSquared() - Math.Pow(DotProduct(normalizedVel, betweenCenters), 2);

            if (closestDistSquared > Math.Pow(moveBall.GetRadius() + statBall.GetRadius(), 2))
                return false;

            double closestDistToCollisionDistSquared = Math.Pow(moveBall.GetRadius() + statBall.GetRadius(), 2) - closestDistSquared;

            double collisionDist = Math.Sqrt(closestDistSquared) - Math.Sqrt(closestDistToCollisionDistSquared);

            if (moveSpeed < collisionDist)
                return false;

            double percentFrameLeft = collisionDist / moveSpeed;

            Debug.WriteLine(percentFrameLeft);
            
            ball1.SetPos(ball1.GetPos() + ScalarProduct(ball1.GetVelocity(), (ball1.GetPercentFrameLeft() - percentFrameLeft)));
            ball2.SetPos(ball2.GetPos() + ScalarProduct(ball2.GetVelocity(), (ball2.GetPercentFrameLeft() - percentFrameLeft)));

            //sets the percent frame left of both balls -
            //probably the same as its namesake here, unless the ball
            //has already collided with another ball in this frame
            ball1.SetPercentFrameLeft(ball1.GetPercentFrameLeft() * percentFrameLeft);
            ball2.SetPercentFrameLeft(ball2.GetPercentFrameLeft() * percentFrameLeft);

            return true;
        }

        private static double DotProduct(Vector2 vect1, Vector2 vect2)
        {
            return vect1.X * vect2.X + vect1.Y * vect2.Y;
        }

        private static Vector2 ScalarProduct(Vector2 vect, double scal)
        {
            return new Vector2((float)(vect.X * scal), (float)(vect.Y * scal));
        }
    }
}
