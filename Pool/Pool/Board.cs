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

        Rectangle tableBounds;

        double friction;
       
        ContentManager content;//creating content manager
        public Texture2D ball;//ball sprite holder

        public Board(int numPlayers, IServiceProvider _serviceProvider)
        {
            content = new ContentManager(_serviceProvider, "Content");//initializing the content manager
            ball = content.Load<Texture2D>("ball");// loading the ball sprite
            players = new Player[numPlayers];
            balls = new List<Ball>();
            gui = new GUI();
            zones = new List<Zone>();

            tableBounds = new Rectangle(20, 20, 520, 320);

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
            Ball moveBall = new Ball(ball);
            Ball statBall = new Ball(ball);
            balls.Add(moveBall);
            balls.Add(statBall);
            moveBall.SetPos(new Vector2(200, 210));
            statBall.SetPos(new Vector2(220, 100));
            moveBall.SetVelocity(new Vector2(-1f, -4f));
            statBall.SetVelocity(new Vector2(0, 4f));

            friction = 0;
           
           
        }

        public void Update(GameTime gameTime)
        {
            Physics.Update(balls, tableBounds);
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

            //just a temporary means to see the boundaries of the table
            spriteBatch.Draw(content.Load<Texture2D>("blank"), tableBounds, Color.LightBlue);


            foreach (Player p in players)
                p.Draw(spriteBatch);

            foreach (Ball b in balls)
                b.Draw(spriteBatch);
           
        }
        
    }
}
