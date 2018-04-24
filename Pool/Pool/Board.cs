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
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;//creating content manager
        public Texture2D ball;//ball sprite holder

        public Board(int numPlayers, IServiceProvider _serviceProvider)
        {
            content = new ContentManager(_serviceProvider, "Content");//initializing the content manager
            ball = this.content.Load<Texture2D>("ball");// loading the ball sprite
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

                balls.Add(players[i]);
            }
            
            balls.Add(new Ball());
            balls.Add(new Ball());
            balls.Add(new Ball());
            balls.Add(new Ball());
            friction = 0;
            gui = new GUI();
            zones = new List<Zone>();
            for (int i=0; i<2;i++)
            {
                balls.Add(new Ball(ball));
                
            }//initializes the balls
        }

        public void Update(GameTime gameTime)
        {
            Physics.Update(balls);

            foreach (Player p in players)
            {
                p.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Player p in players)
                p.Draw(spriteBatch);

            foreach (Ball b in balls)
                b.Draw(spriteBatch);
        }
        
    }
}
