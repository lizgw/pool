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
        // power bars
        public int pbWidth;
        int pbHeight;
        int pbTopOffset;
        // power bar fill
        int fillOffset;
        int fillWidth;
        int fillHeight;
        int fillTopOffset;
        // powerup box background
        int boxWidth;
        int boxHeight;
        int boxOffset;

        public GUI(IServiceProvider serviceProvider, Board aBoard)
        {
            content = new ContentManager(serviceProvider, "Content");
            barTexture = content.Load<Texture2D>("bar");
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

            // power bars
            pbWidth = 25;
            pbHeight = 250;
            pbTopOffset = 125;

            // power bar fill
            fillOffset = 4;
            fillWidth = pbWidth - (fillOffset * 2);
            fillHeight = pbHeight - (fillOffset * 2);
            fillTopOffset = pbTopOffset + fillOffset;

            // powerup box background
            boxWidth = sbHeight;
            boxHeight = boxWidth;
            boxOffset = 5;

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

                // background of power bar
                int xPos = (playerNum % 2) * (Game1.screenWidth - pbWidth); // either left side or right side depending on the playerNum
                powerBarBackgrounds.Add(new Rectangle(xPos, pbTopOffset, pbWidth, pbHeight));

                // fill of power bar
                powerBars.Add(new Rectangle(xPos + fillOffset, fillTopOffset, fillWidth, fillHeight));

                // power up box - should be either right/left of the player's score box
                xPos = rectX;
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
                    // draw pause GUI elements
                    break;
                case GameState.GameOver:
                    DrawPlayGUI(spriteBatch); // draw game overlay under it
                    DrawGameOverGUI(spriteBatch);
                    break;
            }
        }

        private void DrawPlayGUI(SpriteBatch spriteBatch)
        {
            for (int playerNum = 0; playerNum < numPlayers; playerNum++)
            {
                // score
                spriteBatch.Draw(barTexture, scoreBoxes[playerNum], playerColors[playerNum]);
                spriteBatch.DrawString(font, scoreTexts[playerNum], new Vector2(stPadding + (playerNum * (sbWidth + sbMidPadding)), 0), Color.White);

                // power bars
                spriteBatch.Draw(barTexture, powerBarBackgrounds[playerNum], playerColors[playerNum] * 0.25f);
                spriteBatch.Draw(barTexture, powerBars[playerNum], playerColors[playerNum]);

                // power up boxes
                spriteBatch.Draw(barTexture, powerupBoxes[playerNum], playerColors[playerNum]);
            }
        }

        private void DrawPauseGUI(SpriteBatch spriteBatch)
        {
            
        }

        private void DrawGameOverGUI(SpriteBatch spriteBatch)
        {
            // semi-transparent background
            spriteBatch.Draw(barTexture, new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), playerColors[board.winningPlayer] * 0.50f);

            // winning text
            string winText = "Game Over!\nCongratulations player " + (board.winningPlayer + 1) + "!";
            spriteBatch.DrawString(font, winText, new Vector2(100, 100), Color.White);
        }
    }
}
