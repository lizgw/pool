using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Pool
{
    class Player : Ball
    {
        int points;
        int scoreTimer;

        public PlayerIndex playerIndex;
        GamePadState oldGamePad;
        ContentManager content;
        Board board;
        float angle;
        float old_dist;

        float maxPower;
        float maxVelocity;

        float dist;
        bool fire;
        GamePadState gamePad;
        int timer; // only for testing
       // public Player(IServiceProvider serviceProvider, Color aColor, PlayerIndex aPlayerIndex) : base()


        public Player(Color aColor, PlayerIndex aPlayerIndex, Board aBoard) : base()

        {
            points = 100;
            scoreTimer = 0;

            color = aColor;
            playerIndex = aPlayerIndex;
            oldGamePad = GamePad.GetState(playerIndex);

            int offset = 200;
            // set player position based on the index
            switch(playerIndex)
            {
                case PlayerIndex.One:
                    SetPos(new Vector2(offset, offset));
                    break;
                case PlayerIndex.Two:
                    SetPos(new Vector2(Game1.screenWidth - offset, offset));
                    break;
                case PlayerIndex.Three:
                    SetPos(new Vector2(offset, Game1.screenHeight - offset));
                    break;
                case PlayerIndex.Four:
                    SetPos(new Vector2(Game1.screenWidth - offset, Game1.screenHeight - offset));
                    break;
            }

            maxPower = .3f;
            maxVelocity = 4;
            board = aBoard;
        }

        public void Update(GameTime gameTime)
        {
            HandleInput();
            base.Update(gameTime);          
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public void CollectPowerup(Powerup p)
        {
            Console.WriteLine(p.type); // TODO - change player stats somehow
            board.RemovePowerup(p);
        }

        public bool RestartButtonIsDown(PlayerIndex playerIndex)
        {
            GamePadState gamePad = GamePad.GetState(playerIndex);
            return gamePad.IsButtonDown(Buttons.X);
        }

        private void HandleInput()
        {
            GamePadState gamePad = GamePad.GetState(playerIndex);

            // basic movement
            Vector2 leftStick = gamePad.ThumbSticks.Left;

            Vector2 tentativeVelocity = GetVelocity() + new Vector2(leftStick.X * maxPower, -leftStick.Y * maxPower);
            if (tentativeVelocity.LengthSquared() < maxVelocity * maxVelocity)
                SetVelocity(tentativeVelocity);

            // start button - open pause menu
            if (gamePad.Buttons.Start.Equals(ButtonState.Pressed) &&
                !oldGamePad.Buttons.Start.Equals(ButtonState.Pressed))
            {
                Console.WriteLine("Pause menu");
            }

            // A button - use power-up
            if (gamePad.Buttons.A.Equals(ButtonState.Pressed) &&
                !oldGamePad.Buttons.A.Equals(ButtonState.Pressed))
            {
                Console.WriteLine("Use powerup");
            }

            // B button - cancel shot
            if (gamePad.Buttons.B.Equals(ButtonState.Pressed) &&
                !oldGamePad.Buttons.B.Equals(ButtonState.Pressed))
            {
                Console.WriteLine("Cancel shot");
            }

            oldGamePad = gamePad;
        }


        public int GetPoints()
        {
            return points;
        }

        // returns true if the player won
        public bool CountDown()
        {
            scoreTimer = (scoreTimer + 1) % 60;
            if (scoreTimer == 59 && points > 0)
            {
                points--;
            }

            if (points == 0)
                return true;
            else
                return false;
        }

    }
}
