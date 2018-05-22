using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Pool
{
    class GUI
    {
        public static Color[] playerColors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Yellow };

        ContentManager content;
        GameState state;

        Texture2D barTexture;
        Texture2D ballTexture;
        SpriteFont font;

        Board board;

        List<Rectangle> scoreBoxes;
        List<string> scoreTexts;
        List<Rectangle> powerBarBackgrounds;
        List<Rectangle> powerBars;
        List<Rectangle> powerupBoxes;

        int numPlayers;

        // positioning values for the score boxes
        // sb = scoreBox, st = scoreText, pb = powerBar
        int sbPadding;
        int sbMidPadding;
        int sbWidth;
        public int sbHeight;
        int stPadding;
        // powerup box background
        int boxWidth;
        int boxHeight;
        int boxOffset;
        int powerupBarHeight;
        // old power bar width
        public int pbWidth;

        public GUI(IServiceProvider serviceProvider, Board aBoard)
        {
            content = new ContentManager(serviceProvider, "Content");
            barTexture = content.Load<Texture2D>("bar");
            ballTexture = content.Load<Texture2D>("ball");
            font = content.Load<SpriteFont>("SpriteFont1");

            board = aBoard;
            state = board.state;
            numPlayers = board.players.Length;

            // set up values for calculating position
            // score box
            sbPadding = 150;
            sbMidPadding = 500;
            sbWidth = 150;
            sbHeight = 50;
            stPadding = sbPadding + 50;

            // powerup box background
            boxWidth = sbHeight;
            boxHeight = boxWidth;
            boxOffset = 5;
            powerupBarHeight = 10;

            pbWidth = 25;

            // create lists of GUI elements for each player
            scoreBoxes = new List<Rectangle>(numPlayers);
            scoreTexts = new List<string>(numPlayers);
            powerBarBackgrounds = new List<Rectangle>(numPlayers);
            powerBars = new List<Rectangle>(numPlayers);
            powerupBoxes = new List<Rectangle>(numPlayers);

            for (int playerNum = 0; playerNum < numPlayers; playerNum++)
            {
                // score background rectangle
                int rectX = sbPadding + (playerNum * (sbWidth + sbMidPadding));
                scoreBoxes.Add(new Rectangle(rectX, 0, sbWidth, sbHeight));

                // player score text
                scoreTexts.Add("" + board.players[playerNum].GetPoints());

                // power up box - should be either right/left of the player's score box
                int xPos = rectX;
                if (playerNum % 2 == 1) // player 1 & 3, goes on the left of the box
                    xPos -= (boxWidth + boxOffset);
                else // player 2 & 4, goes on the right of the box
                    xPos += scoreBoxes[playerNum].Width + boxOffset;

                powerupBoxes.Add(new Rectangle(xPos, 0, boxWidth, boxHeight));
            }
        }

        public void Update(GameTime gameTime)
        {
            // update state
            state = board.state;

            // update score text for each player
            for (int i = 0; i < board.players.Length; i++)
                scoreTexts[i] = "" + board.players[i].GetPoints();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch(state)
            {
                case GameState.Play:
                    // draw game GUI elements
                    DrawPlayGUI(spriteBatch);
                    break;
                case GameState.Pause:
                    DrawPauseGUI(spriteBatch);
                    break;
                case GameState.GameOver:
                    DrawPlayGUI(spriteBatch); // draw game overlay under it
                    DrawGameOverGUI(spriteBatch);
                    break;
                case GameState.MainMenu:
                    DrawMainMenu(spriteBatch);
                    break;
                case GameState.instructions:
                    DrawInstructionsGUI(spriteBatch);
                    break;
            }
        }

        private void DrawPlayGUI(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < numPlayers; i++) // draw GUI elems for each player
            {
                // score
                spriteBatch.Draw(barTexture, scoreBoxes[i], playerColors[i]);
                spriteBatch.DrawString(font, scoreTexts[i], new Vector2(stPadding + (i * (sbWidth + sbMidPadding)), 0), Color.White);

                // power up boxes
                spriteBatch.Draw(barTexture, powerupBoxes[i], playerColors[i]);

                // powerups
                if (board.players[i].GetPowerupType() != PowerupType.Null)
                {
                    Color c = Powerup.GetColor(board.players[i].GetPowerupType());
                    Vector2 position = new Vector2(powerupBoxes[i].X + powerupBoxes[i].Width/2, powerupBoxes[i].Y + powerupBoxes[i].Height / 2);
                    float boxSize = 75f / 2f;
                    float scale = 0.5f;
                    Vector2 origin = new Vector2(boxSize, boxSize);
                    spriteBatch.Draw(ballTexture, position, null, c, 0, origin, scale, SpriteEffects.None, 0);
                }

                // powerup timer bars
                float percentDone = board.players[i].GetPowerupPercentDone();
                if (percentDone == 0) // when no powerup, don't draw it
                    percentDone = 1;
                Rectangle barRect = new Rectangle(powerupBoxes[i].X, powerupBoxes[i].Bottom - powerupBarHeight, (int)(powerupBoxes[i].Width * (1 - percentDone)), powerupBarHeight);
                spriteBatch.Draw(barTexture, barRect, Color.White);
            }
        }

        private void DrawPauseGUI(SpriteBatch spriteBatch)
        {
            DrawPlayGUI(spriteBatch);
            //background
            spriteBatch.Draw(barTexture, new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight),Color.Gray * 0.50f);
            spriteBatch.DrawString(font,"paused", new Vector2((Game1.screenWidth / 2) - 125, 50), Color.White,0.0f,new Vector2(0,0),2f,SpriteEffects.None,0.01f);
            //spriteBatch.DrawString(font, "Paused", new Vector2((Game1.screenWidth / 2) - 55,  50), Color.White);
            //options
            //resume
            //spriteBatch.Draw(barTexture, new Rectangle((Game1.screenWidth/2)-32, (Game1.screenHeight/2)-10, 75, 20),  Color.White);
            spriteBatch.DrawString(font, "A: Resume", new Vector2((Game1.screenWidth / 2) - 75, (Game1.screenHeight / 2) - 50), Color.YellowGreen);
            spriteBatch.DrawString(font, "B: Restart", new Vector2((Game1.screenWidth / 2) - 75, (Game1.screenHeight / 2) ), Color.Red);
            spriteBatch.DrawString(font, "X: Quit to Main Menu", new Vector2((Game1.screenWidth / 2) - 75, (Game1.screenHeight / 2) +50), Color.Blue);
            // spriteBatch.DrawString(font, "Restart", new Vector2((Game1.screenWidth / 2) - 32, (Game1.screenHeight / 2) - 10), Color.White);
        }
        private void DrawInstructionsGUI(SpriteBatch spriteBatch)
        {
             //background
            spriteBatch.Draw(barTexture, new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), Color.Green);
            // game details
            string details = "Use the left stick to aim and fire.\n" +
                "Press A to use your powerup.\n" +
                "Press B to cancel your shot.\n" +
                "Press Start to pause the game.\n\n" +
                "The goal is to get the most\nballs on your side of the screen!\n"+
                "\nPress B to return to the Main Menu";
            spriteBatch.DrawString(font, details, new Vector2(50, 150), Color.White);
        }
        private void DrawGameOverGUI(SpriteBatch spriteBatch)
        {
            // semi-transparent background
            spriteBatch.Draw(barTexture, new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), playerColors[board.winningPlayer] * 0.50f);

            // winning text
            string winText = "Game Over!\nCongratulations player " + (board.winningPlayer + 1) + "!";
            spriteBatch.DrawString(font, winText, new Vector2(100, 100), Color.White);

            // Play again text
            string againText = "Press X on any controller to play again";
            spriteBatch.DrawString(font, againText, new Vector2(100, 300), Color.White);
        }
        private void DrawMainMenu(SpriteBatch spriteBatch)
        {
            //background
            spriteBatch.Draw(barTexture, new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), Color.Green);

            //title---game name
            spriteBatch.DrawString(font,"Zilliards", new Vector2(Game1.screenWidth/2-75,50), Color.Yellow,0f,new Vector2(0,0),1,SpriteEffects.None,0.0f);

            

            //main menu options
            spriteBatch.DrawString(font, "B - Play New Game", new Vector2((Game1.screenWidth/2)-150,200), Color.Red);
            spriteBatch.DrawString(font, "X - Instructions", new Vector2((Game1.screenWidth / 2) - 150, 250), Color.Blue);
        }
    }
}
