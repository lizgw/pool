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
        public static void Update(List<Ball> balls)
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
        }

        private static bool TryCollide(Ball ball1, Ball ball2)
        {
            Ball moveBall = ball1.Copy();
            Ball statBall = ball2.Copy();
            moveBall.SetVelocity(ScalarProduct(moveBall.GetVelocity(), moveBall.GetPercentFrameLeft()) - ScalarProduct(statBall.GetVelocity(), statBall.GetPercentFrameLeft()));
            statBall.SetVelocity(Vector2.Zero);

            Vector2 betweenCenters = statBall.GetPos() - moveBall.GetPos();

            if (moveBall.GetVelocity().Length() < betweenCenters.Length() - moveBall.GetRadius() - statBall.GetRadius())
                return false;

            if (DotProduct(moveBall.GetVelocity(), betweenCenters) <= 0)
                return false;

            Vector2 normalizedVel = new Vector2(moveBall.GetVelocity().X, moveBall.GetVelocity().Y);
            normalizedVel.Normalize();

            double closestDistSquared = betweenCenters.LengthSquared() - Math.Pow(DotProduct(normalizedVel, betweenCenters), 2);

            if (closestDistSquared > Math.Pow(moveBall.GetRadius() + statBall.GetRadius(), 2))
                return false;



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
