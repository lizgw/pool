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
        IncreasePower,
        BigBall,
        Null
    }

    class Powerup : Ball
    {
        PowerupType type;

        static Color[] colors = { Color.DarkOrange, Color.Purple, Color.LightGreen };

        //Bomb stuff
        static float blastRadius = 200;
        static float bombImpulse = 20;

        //Fast Charge stuff
        public static readonly float normalMaxPower = 4f;
        public static readonly float bigMaxPower = 8f;

        //Big Ball stuff
        public static readonly float normalRadius = 20;
        public static readonly float bigRadius = 40;
        
        public static int count; // total number of powerups created

        public Powerup(PowerupType aType) : base()
        {
            type = aType;
            color = colors[(int)type];
            count++;
        }

        public static void BombActivate(Player p)
        {
            List<Ball> balls = p.GetBoard().GetBalls();
            foreach (Ball ball in balls)
            {
                if ((ball.GetPos() - p.GetPos()).LengthSquared() <= blastRadius * blastRadius && ball != p)
                {
                    Vector2 normalizedAngleVector = ball.GetPos() - p.GetPos();
                    normalizedAngleVector.Normalize();
                    ball.SetVelocity(ball.GetVelocity() + Physics.ScalarProduct(normalizedAngleVector, bombImpulse / ball.GetMass()));
                }
            }
        }

        public PowerupType GetPowerupType()
        {
            return type;
        }

        public static Color GetColor(PowerupType p)
        {
            return colors[(int)p];
        }
    }
}
