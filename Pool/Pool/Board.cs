﻿using System;
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
        public ContentManager Content
        {
            get { return content; }
        }
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
                        break;
                    case 1:
                        players[i] = new Player(_serviceProvider, Color.Blue, PlayerIndex.Two);
                        break;
                    case 2:
                        players[i] = new Player(_serviceProvider, Color.Green, PlayerIndex.Three);
                        break;
                    case 3:
                        players[i] = new Player(_serviceProvider, Color.Yellow, PlayerIndex.Four);
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
            moveBall.SetPos(new Vector2(220, 150));
            statBall.SetPos(new Vector2(200, 100));
            moveBall.SetVelocity(new Vector2(0, -5f));

            friction = 0;
            gui = new GUI();
            zones = new List<Zone>();
        }

        public void Update(GameTime gameTime)
        {
            Physics.Update(balls, tableBounds);
            foreach (Player p in players)
            {
                p.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //just a temporary means to see the boundaries of the table
            spriteBatch.Draw(content.Load<Texture2D>("blank"), tableBounds, Color.LightBlue);

            foreach (Player p in players)
                p.Draw(spriteBatch);

            foreach (Ball b in balls)
                b.Draw(spriteBatch);
        }
        
    }
}
