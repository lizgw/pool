using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pool
{
    static class Physics
    {
        public static void Update(List<Ball> balls)
        {
            foreach(Ball ball in balls)
            {
                if (CouldCollide(ball, balls))
                {

                }
            }
        }

        private static bool CouldCollide(Ball ball, List<Ball> balls)
        {
            return true;
        }
    }
}
