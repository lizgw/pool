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
                        SetNewVelocities(ball1, ball2);
                    }
                }
            }

            //collides off of walls and also continues motion of balls with new velocity vectors
            for (int b = 0; b < balls.Count; b++)
            {
                Ball ball = balls[b];

                //wall collision - not continuous like ball-ball collision, but maybe fix later
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
                
                ball.SetPos(ball.GetPos() + ScalarProduct(ball.GetVelocity(), ball.GetPercentFrameLeft()));
                ball.SetPercentFrameLeft(1);
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

            double distToClosestPoint = Math.Sqrt(betweenCenters.LengthSquared() - closestDistSquared);

            double collisionDist = distToClosestPoint - Math.Sqrt(closestDistToCollisionDistSquared);

            if (moveSpeed < collisionDist)
                return false;

            double percentFrameDone = collisionDist / moveSpeed;

            //I'm actually not totally sure about this, but maybe it won't be a problem?
            ball1.SetPos(ball1.GetPos() + ScalarProduct(ball1.GetVelocity(), ball1.GetPercentFrameLeft() * percentFrameDone));
            ball2.SetPos(ball2.GetPos() + ScalarProduct(ball2.GetVelocity(), ball2.GetPercentFrameLeft() * percentFrameDone));
            
            ball1.SetPercentFrameLeft(ball1.GetPercentFrameLeft() * (1 - percentFrameDone));
            ball2.SetPercentFrameLeft(ball2.GetPercentFrameLeft() * (1 - percentFrameDone));

            return true;
        }

        //pre-condition: ball1 and ball2 are touching each other
        private static void SetNewVelocities(Ball ball1, Ball ball2)
        {
            Vector2 n = ball1.GetPos() - ball2.GetPos();
            n.Normalize();

            double a1 = DotProduct(ball1.GetVelocity(), n);
            double a2 = DotProduct(ball2.GetVelocity(), n);
            
            double optimizedP = (2.0 * (a1 - a2)) / (ball1.GetMass() + ball2.GetMass());
            
            Vector2 newV1 = ball1.GetVelocity() - ScalarProduct(n, optimizedP * ball1.GetMass());
            Vector2 newV2 = ball2.GetVelocity() - ScalarProduct(n, optimizedP * ball2.GetMass());

            ball1.SetVelocity(newV1);
            ball2.SetVelocity(newV2);
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
