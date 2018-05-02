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
                        zones.Add(new Zone(new Rectangle(0, 0, Game1.screenWidth/2, Game1.screenHeight), players[i], _serviceProvider));
                        break;
                    case 1:
                        players[i] = new Player(_serviceProvider, GUI.playerColors[1], PlayerIndex.Two);
                        zones.Add(new Zone(new Rectangle(Game1.screenWidth / 2, 0, Game1.screenWidth / 2, Game1.screenHeight), players[i], _serviceProvider));
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
            balls.Add(new Ball(ball, new Vector2(50, 50)));
            balls.Add(new Ball(ball, new Vector2(100, 500)));
            balls.Add(new Ball(ball, new Vector2(200, 100)));
            balls.Add(new Ball(ball, new Vector2(100, 700)));
            friction = 0;

            gui = new GUI(_serviceProvider, this);
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
            for (int i = 1; i < numBallsInZone.Length; i++)
            {
                if (numBallsInZone[i] > numBallsInZone[maxIndex])
                    maxIndex = i;
            }

            // return the index of the zone/player
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

            // for the winning player, count down the timer
            players[index].CountDown();
        }
        
    }
}
