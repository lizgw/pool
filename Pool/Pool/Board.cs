using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Pool
{
    class Board
    {
        Player[] players;
        List<Ball> balls;
        GUI gui;
        List<Zone> zones;

        double friction;
       
        ContentManager content;//creating content manager
        public Texture2D ball;//ball sprite holder

        public Board(int numPlayers, IServiceProvider _serviceProvider)
        {
            content = new ContentManager(_serviceProvider, "Content");//initializing the content manager
            ball = this.content.Load<Texture2D>("ball");// loading the ball sprite
            players = new Player[numPlayers];
            balls = new List<Ball>(2);
            balls = new List<Ball>();
            gui = new GUI();
            zones = new List<Zone>();

            // create all players
            for (int i = 0; i < players.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        players[i] = new Player(_serviceProvider, Color.Red, PlayerIndex.One);
                        zones.Add(new Zone(new Rectangle(0,0,400,480), players[i], _serviceProvider));
                        break;
                    case 1:
                        players[i] = new Player(_serviceProvider, Color.Blue, PlayerIndex.Two);
                        zones.Add(new Zone(new Rectangle(400,0,400,480), players[i], _serviceProvider));
                        break;
                    case 2:
                        players[i] = new Player(_serviceProvider, Color.Green, PlayerIndex.Three);
                        zones.Add(new Zone(new Rectangle(0, 0, 5, 5), players[i], _serviceProvider));
                        break;
                    case 3:
                        players[i] = new Player(_serviceProvider, Color.Yellow, PlayerIndex.Four);
                        zones.Add(new Zone(new Rectangle(0, 0, 5, 5), players[i], _serviceProvider));
                        break;
                    default:
                        Console.WriteLine("Error - there should be 1-4 players");
                        break;
                }

                balls.Add(players[i]);
            }

            balls = new List<Ball>();
            balls.Add(new Ball(ball));
            balls.Add(new Ball(ball));
            balls.Add(new Ball(ball));
            balls.Add(new Ball(ball));
            friction = 0;
           
           
        }

        public void Update(GameTime gameTime)
        {
            Physics.Update(balls);
            foreach (Player p in players)
            {
                p.Update(gameTime);
            }
            foreach (Zone z in zones)
                z.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Zone z in zones)
                z.Draw(spriteBatch);
            foreach (Player p in players)
                p.Draw(spriteBatch);

            foreach (Ball b in balls)
                b.Draw(spriteBatch);
           
        }
        
    }
}
