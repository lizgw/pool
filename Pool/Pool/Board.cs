using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

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
        IServiceProvider serviceProvider;

        public Texture2D ball;//ball sprite holder

        public int winningPlayer; // for the GUI to access

        Random rnd;
        int powerupTimer;
        int powerupInterval; // when it resets, this value changes randomly
        int powerupTimerMax;
        int powerupTimerMin;
        int viberation_timer;
       
       

        int pbWidth;

        public Board(int numPlayers, IServiceProvider aServiceProvider)
        {
            viberation_timer = 0;
            serviceProvider = aServiceProvider;
            content = new ContentManager(serviceProvider, "Content");//initializing the content manager
            Ball.defaultTexture = content.Load<Texture2D>("ball");// loading the ball sprite
            Player.SetCueStickTexture(content.Load<Texture2D>("cueStick"));

            players = new Player[numPlayers];
            state = GameState.MainMenu; // set this dynamically later
            zones = new List<Zone>();
            balls = new List<Ball>();

            // create all players
            CreatePlayers();

            // GUI & table bounds
            gui = new GUI(serviceProvider, this);
            tableBounds = new Rectangle(gui.pbWidth, gui.sbHeight,
                Game1.screenWidth - (gui.pbWidth*2), Game1.screenHeight - (gui.sbHeight*2));

            // Add the non-player balls
            CreateBalls();

            friction = 0.10;

            winningPlayer = -1;

            rnd = new Random();
            powerupTimer = 0;
            powerupTimerMax = 720;
            powerupTimerMin = 360;
            powerupInterval = rnd.Next(powerupTimerMin, powerupTimerMax + 1);
          
            gui = new GUI(serviceProvider, this);
        }

        private void CreateBalls()
        {
            AddBallTriangle(new Vector2(tableBounds.Center.X, tableBounds.Center.Y), 4, new Ball());
        }

        private void AddBallTriangle(Vector2 centerPos, int sideLength, Ball ballPrefab)
        {
            double radius = ballPrefab.GetRadius();

            double firstBallX = centerPos.X;
            double firstballY = centerPos.Y - radius * 3;

            for (int row = 0; row < sideLength; row++)
            {
                for (int col = 0; col <= row; col++)
                {
                    Vector2 ballPos = new Vector2((float)(firstBallX + col * radius * 2), (float)(firstballY + radius * 1.732 * row)); // 1.732 is sqrt(3)
                    Ball ballToAdd = ballPrefab.Copy();
                    ballToAdd.SetPos(ballPos);
                    balls.Add(ballToAdd);
                }
                firstBallX -= radius;
            }
        }

        private void CreatePowerup()
        {
            // pick a random type
            int randType = rnd.Next(0, 3);

            // create the powerup
            Powerup p = new Powerup((PowerupType)randType);

            // move it to a random location in the table bounds
            bool foundPos = false;
            do
            {
                // move it
                int pSize = p.GetDrawRect().Width;
                int xPos = rnd.Next(tableBounds.X + pSize, tableBounds.Right - pSize);
                int yPos = rnd.Next(tableBounds.Y + pSize, tableBounds.Bottom - pSize);
                p.SetPos(new Vector2(xPos, yPos));

                // check if it intersects a pre-existing ball
                foundPos = !PowerupIntersection(p);
            } while (!foundPos);

            // add it to the list
            balls.Add(p);

            // reset the timer
            powerupInterval = rnd.Next(powerupTimerMin, powerupTimerMax + 1);
        }

        private void CreatePlayers()
        {
            for (int i = 0; i < players.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        players[i] = new Player(GUI.playerColors[0], PlayerIndex.One, this);
                        zones.Add(new Zone(serviceProvider, new Rectangle(0, 0, Game1.screenWidth / 2-100, Game1.screenHeight), players[i]));
                        break;
                    case 1:
                        players[i] = new Player(GUI.playerColors[1], PlayerIndex.Two, this);
                        zones.Add(new Zone(serviceProvider, new Rectangle(Game1.screenWidth / 2+100, 0, Game1.screenWidth / 2, Game1.screenHeight), players[i]));
                        break;
                    case 2:
                        players[i] = new Player(GUI.playerColors[2], PlayerIndex.Three, this);
                        zones.Add(new Zone(serviceProvider, new Rectangle(0, 0, 5, 5), players[i]));
                        break;
                    case 3:
                        players[i] = new Player(GUI.playerColors[3], PlayerIndex.Four, this);
                        zones.Add(new Zone(serviceProvider, new Rectangle(0, 0, 5, 5), players[i]));
                        break;
                    default:
                        Console.WriteLine("Error - there should be 1-4 players");
                        break;
                }

                balls.Add(players[i]);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (state == GameState.Play)
            {
                Physics.Update(balls, tableBounds, friction);

                foreach (Player p in players)
                    p.Update(gameTime);

                foreach (Zone z in zones)
                    z.Update(gameTime);

                UpdateScores();

                // create powerups
                powerupTimer = (powerupTimer + 1) % (powerupInterval + 1);
                if (powerupTimer == powerupInterval && Powerup.count < 8)
                    CreatePowerup();
               
            }
            else if (state == GameState.GameOver)
            {
                // listen for the "restart game" button
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].RestartButtonIsDown(players[i].playerIndex))
                    {
                        RestartGame();
                    }
                }
            }
            else if (state == GameState.Pause)
            {
                foreach (Player p in players)
                    p.Update(gameTime);
            }
            else if (state == GameState.MainMenu)
            {
                foreach (Player p in players)
                    p.Update(gameTime);
            }
            viberation_timer = (viberation_timer + 1) % 61;
            if (viberation_timer == 60)
            {
                foreach (Player p in players)
                {
                    GamePad.SetVibration(p.playerIndex, 0f, 0f);
                }
            }
            gui.Update(gameTime);
           

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Zone z in zones)
                z.Draw(spriteBatch);

            // draw table bounds
            spriteBatch.Draw(content.Load<Texture2D>("blank"), tableBounds, Color.Green * 0.75f);

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
                    if (indexOfZone != -1)
                        numBallsInZone[indexOfZone]++;
                }
            }

            // find the zone that contains the most balls
            int maxIndex = 0;
            bool tie = false;
            for (int i = 1; i < numBallsInZone.Length; i++)
            {
                if (numBallsInZone[i] == numBallsInZone[maxIndex] )
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
                winningPlayer = index;
                state = GameState.GameOver;
            }
        }
        
        public void RestartGame()
        {
            // reset board vars
            state = GameState.Play;
            winningPlayer = -1;

            // empty the lists
            balls.Clear();
            players = new Player[players.Length];

            // create the players & balls again
            CreatePlayers();
            CreateBalls();
        }

        public void RemovePowerup(Powerup p)
        {
            balls.Remove(p);
            p = null;
            Powerup.count--;
        }
        
        public List<Ball> GetBalls()
        {
            return balls;
        }

        private bool PowerupIntersection(Powerup p)
        {
            int i = 0;
            
            while (i < balls.Count())
            {
                Vector2 dist = p.GetPos() - balls[i].GetPos();
                if (Math.Abs(dist.X) < p.GetRadius()*2 && Math.Abs(dist.Y) < p.GetRadius()*2)
                    return true;
                i++;
            }

            return false;
        }

        public static void vibrate(Player player, float index)
        {
            
            GamePad.SetVibration(player.playerIndex, index, index);
           
            
        }

    }
}