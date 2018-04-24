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
            // create all players
            for (int i = 0; i < players.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        players[i] = new Player(Color.Red, PlayerIndex.One);
                        break;
                    case 1:
                        players[i] = new Player(Color.Blue, PlayerIndex.Two);
                        break;
                    case 2:
                        players[i] = new Player(Color.Green, PlayerIndex.Three);
                        break;
                    case 3:
                        players[i] = new Player(Color.Yellow, PlayerIndex.Four);
                        break;
                    default:
                        Console.WriteLine("Error - there should be 1-4 players");
                        break;
                }
            }

            balls = new List<Ball>(2);
            friction = 0;

            gui = new GUI();
            zones = new List<Zone>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Player p in players)
            {
                p.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Player p in players)
            {
                p.Draw(spriteBatch);
            }
        }
    }
}
