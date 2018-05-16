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
        FastCharge,
        BigBall,
        Null
    }

    class Powerup : Ball
    {
        public PowerupType type;

        public static Color[] colors = { Color.DarkOrange, Color.Purple, Color.LightGreen };

        //Bomb stuff
        static float blastRadius = 100;
        static float maxBombForce = 100;

        //Fast Charge stuff

        //Big Ball stuff

        public Powerup(PowerupType aType) : base()
        {
            type = aType;
            color = colors[(int)type];
        }

        public static void Activate(Player p)
        {
            switch (p.GetPowerupType())
            {
                case PowerupType.Bomb:
                    BombActivate(p);
                    break;
                case PowerupType.FastCharge:
                    FastChargeActivate(p);
                    break;
                case PowerupType.BigBall:
                    BigBallActivate(p);
                    break;
                case PowerupType.Null:
                    break;
            }
        }

        private static void BombActivate(Player p)
        {
            List<Ball> balls = p.GetBoard().GetBalls();
            foreach (Ball ball in balls)
            {
                if ((ball.GetPos() - p.GetPos()).LengthSquared() >= blastRadius * blastRadius)
                {
                    //Vector2 normalizedThingyIDontKnow
                    //ball.SetVelocity(ball.GetVelocity() + Physics.ScalarProduct( maxBombForce / ball.GetMass())
                }
            }
        }

        private static void FastChargeActivate(Player p)
        {
            Console.WriteLine("FAST CHARGE");
        }

        private static void BigBallActivate(Player p)
        {
            Console.WriteLine("BIG BALL");
        }

        public PowerupType GetPowerupType()
        {
            return type;
        }
    }
}
