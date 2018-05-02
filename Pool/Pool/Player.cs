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

        PlayerIndex playerIndex;
        GamePadState oldGamePad;
        ContentManager content;

        float maxPower;

        public Player(IServiceProvider serviceProvider, Color aColor, PlayerIndex aPlayerIndex) : base()
        {
            points = 100;
            scoreTimer = 0;

            color = aColor;
            playerIndex = aPlayerIndex;
            oldGamePad = GamePad.GetState(playerIndex);

            content = new ContentManager(serviceProvider, "Content");
            texture = content.Load<Texture2D>("ball");

            maxPower = 5;
        }

        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            HandleInput();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void HandleInput()
        {
            GamePadState gamePad = GamePad.GetState(playerIndex);

            // basic movement
            Vector2 leftStick = gamePad.ThumbSticks.Left;

            float angle = (float)Math.Atan2(leftStick.Y, leftStick.X); // in radians
            // no negative angles, just around the unit circle (0 to 360 degrees)
            if (angle < 0)
                angle += (float)(Math.PI*2);

            // distance between center of stick axis and it's current position
            float dist = (float)Math.Sqrt(Math.Pow(leftStick.X, 2) + Math.Pow(leftStick.Y, 2));
            float percentPower = 0;

            // set velocity as a percentage of the max power
            SetVelocity(new Vector2(dist * maxPower, dist * maxPower));
            
            // move based on angle and velocity
            Move(angle);

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

        public void CountDown()
        {
            scoreTimer = (scoreTimer + 1) % 60;
            if (scoreTimer == 59)
            {
                points--;
            }
        }
    }
}
