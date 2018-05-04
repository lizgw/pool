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
        public Player[] players;
        List<Ball> balls;
        GUI gui;
        List<Zone> zones;

        public GameState state;
        Rectangle tableBounds;

        double friction;
       
        ContentManager content;//creating content manager
        public Texture2D ball;//ball sprite holder

        public Board(int numPlayers, IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider, "Content");//initializing the content manager
            Ball.defaultTexture = content.Load<Texture2D>("ball");// loading the ball sprite

            players = new Player[numPlayers];
            state = GameState.Play; // set this dynamically later
            zones = new List<Zone>();
            balls = new List<Ball>();

            tableBounds = new Rectangle(20, 20, 520, 320);

            // create all players
            for (int i = 0; i < players.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        players[i] = new Player(GUI.playerColors[0], PlayerIndex.One);
                        zones.Add(new Zone(serviceProvider, new Rectangle(0, 0, Game1.screenWidth/2, Game1.screenHeight), players[i]));
                        break;
                    case 1:
                        players[i] = new Player(GUI.playerColors[1], PlayerIndex.Two);
                        zones.Add(new Zone(serviceProvider, new Rectangle(Game1.screenWidth / 2, 0, Game1.screenWidth / 2, Game1.screenHeight), players[i]));
                        break;
                    case 2:
                        players[i] = new Player(GUI.playerColors[2], PlayerIndex.Three);
                        zones.Add(new Zone(serviceProvider, new Rectangle(0, 0, 5, 5), players[i]));
                        break;
                    case 3:
                        players[i] = new Player(GUI.playerColors[3], PlayerIndex.Four);
                        zones.Add(new Zone(serviceProvider, new Rectangle(0, 0, 5, 5), players[i]));
                        break;
                    default:
                        Console.WriteLine("Error - there should be 1-4 players");
                        break;
                }

                balls.Add(players[i]);
            }

            gui = new GUI(serviceProvider, this);

            // physics debug
            Ball moveBall = new Ball();
            Ball statBall = new Ball();
            moveBall.SetPos(new Vector2(200, 210));
            statBall.SetPos(new Vector2(200, 100));
            moveBall.SetVelocity(new Vector2(0, -4f));
            statBall.SetVelocity(new Vector2(-1f, 4f));
            balls.Add(moveBall);
            balls.Add(statBall);

            friction = 0;

            gui = new GUI(serviceProvider, this);
        }

        public void Update(GameTime gameTime)
        {

            Physics.Update(balls, tableBounds);


            foreach (Player p in players)
                p.Update(gameTime);

            foreach (Zone z in zones)
                z.Update(gameTime);

            gui.Update(gameTime);

            UpdateScores();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Zone z in zones)
                z.Draw(spriteBatch);

            //just a temporary means to see the boundaries of the table
            //spriteBatch.Draw(content.Load<Texture2D>("blank"), tableBounds, Color.LightBlue);

            foreach (Player p in players)
                p.Draw(spriteBatch);

            foreach (Ball b in balls)
                b.Draw(spriteBatch);

            gui.Draw(spriteBatch);
        }

        // returns the index of the player with the most number of balls in their zone
        private int FindWinningPlayer()
        {
            // 1 index for each player/zone
            int[] numBallsInZone = new int[players.Length];

            for (int i = 0; i < balls.Count(); i++)
            {
                Ball b = balls[i];

                // if it's not a player (just a normal ball)
                if (b.GetType() != typeof(Player))
                {
                    // find which zone it's in
                    int indexOfZone = FindZone(b);

                    // increment the count for that zone
                    numBallsInZone[indexOfZone]++;
                }
            }

            // find the zone that contains the most balls
            int maxIndex = 0;
            bool tie = false;
            for (int i = 1; i < numBallsInZone.Length; i++)
            {
                if (numBallsInZone[i] == numBallsInZone[maxIndex])
                {
                    tie = true;
                }
                else if (numBallsInZone[i] > numBallsInZone[maxIndex])
                {
                    maxIndex = i;
                    tie = false;
                }
            }

            // return the index of the zone/player
            if (tie)
                return -1;
            else
                return maxIndex;
        }

        // returns the index of the zone that b is in
        private int FindZone(Ball b)
        {
            for (int i = 0; i < zones.Count(); i++)
            {
                if (zones[i].GetBounds().Intersects(b.GetDrawRect()))
                    return i;
            }

            // this shouldn't happen - each ball should be in a zone
            return -1;
        }

        private void UpdateScores()
        {
            // find the winning player's index
            int index = FindWinningPlayer();

            bool wonGame = false;

            // -1 is returned if there's a tie
            if (index >= 0)
            {
                // count down the timer for the winning player
                wonGame = players[index].CountDown();
            }

            if (wonGame)
            {
                // do some game over thing
                Console.WriteLine("Game over! Player " + (index + 1) + " won!");
            }
        }
        
    }
}
