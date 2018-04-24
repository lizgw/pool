using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pool
{
    class Player : Ball
    {
        int points;
        PlayerIndex playerIndex;
        GamePadState oldGamePad;

        float maxPower;

        public Player(Color aColor, PlayerIndex aPlayerIndex) : base()
        {
            points = 100;
            color = aColor;
            playerIndex = aPlayerIndex;
            oldGamePad = GamePad.GetState(playerIndex);

            maxPower = 100;
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
            // TODO: replace with physics-based movement
            Vector2 leftStick = gamePad.ThumbSticks.Left;

            float angle = (float)Math.Atan2(leftStick.Y, leftStick.X);
            Console.WriteLine(angle);

            float percentPower = 0;

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
    }
}
