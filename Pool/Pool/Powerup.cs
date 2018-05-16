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
        static float blastRadius = 100;
        static float maxBombForce = 100;

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

        private static void IncreasePowerActivate(Player p)
        {
            Console.WriteLine("INCREASE POWER");
        }

        private static void BigBallActivate(Player p)
        {
            Console.WriteLine("BIG BALL");
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
