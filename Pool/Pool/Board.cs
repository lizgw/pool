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
        public Player[] players;
        List<Ball> balls;
        GUI gui;
        List<Zone> zones;

        public GameState state;
        Rectangle tableBounds;

        double friction;
       
        ContentManager content;//creating content manager
        public Texture2D ball;//ball sprite holder

        public Board(int numPlayers, IServiceProvider _serviceProvider)
        {
            content = new ContentManager(_serviceProvider, "Content");//initializing the content manager
            ball = this.content.Load<Texture2D>("ball");// loading the ball sprite

            players = new Player[numPlayers];
            state = GameState.Play; // set this dynamically later
            gui = new GUI(_serviceProvider, this);
            zones = new List<Zone>();
            balls = new List<Ball>();

            tableBounds = new Rectangle(20, 20, 520, 320);

            // create all players
            for (int i = 0; i < players.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        players[i] = new Player(_serviceProvider, GUI.playerColors[0], PlayerIndex.One);
                        zones.Add(new Zone(new Rectangle(0,0,400,480), players[i], _serviceProvider));
                        break;
                    case 1:
                        players[i] = new Player(_serviceProvider, GUI.playerColors[1], PlayerIndex.Two);
                        zones.Add(new Zone(new Rectangle(400,0,400,480), players[i], _serviceProvider));
                        break;
                    case 2:
                        players[i] = new Player(_serviceProvider, GUI.playerColors[2], PlayerIndex.Three);
                        zones.Add(new Zone(new Rectangle(0, 0, 5, 5), players[i], _serviceProvider));
                        break;
                    case 3:
                        players[i] = new Player(_serviceProvider, GUI.playerColors[3], PlayerIndex.Four);
                        zones.Add(new Zone(new Rectangle(0, 0, 5, 5), players[i], _serviceProvider));
                        break;
                    default:
                        Console.WriteLine("Error - there should be 1-4 players");
                        break;
                }

                balls.Add(players[i]);
            }
            balls.Add(new Ball(ball));
            balls.Add(new Ball(ball));
            balls.Add(new Ball(ball));
            balls.Add(new Ball(ball));
            friction = 0;

            // physics debug
            /*Ball moveBall = new Ball(ball);
            Ball statBall = new Ball(ball);
            balls.Add(moveBall);
            balls.Add(statBall);
            moveBall.SetPos(new Vector2(220, 150));
            statBall.SetPos(new Vector2(200, 100));
            moveBall.SetVelocity(new Vector2(0, -5f));*/
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
            gui.Draw(spriteBatch);

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
