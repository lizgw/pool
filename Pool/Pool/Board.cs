using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pool
{
    class Board
    {
        Player[] players;
        List<Ball> balls;
        GUI gui;
        List<Zone> zones;

        double friction;

        public Board(int numPlayers)
        {
            players = new Player[numPlayers];
            balls = new List<Ball>(2);
            balls.Add(new Ball());
            balls.Add(new Ball());
            balls.Add(new Ball());
            balls.Add(new Ball());
            friction = 0;

            gui = new GUI();
            zones = new List<Zone>();
        }

        public void Update(GameTime gameTime)
        {
            Physics.Update(balls);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
