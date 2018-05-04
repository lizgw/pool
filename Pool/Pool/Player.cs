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
        float angle;
        float old_dist;
        float maxPower;

        float dist;
        bool fire;
        GamePadState gamePad;
        int timer; // only for testing
       // public Player(IServiceProvider serviceProvider, Color aColor, PlayerIndex aPlayerIndex) : base()


        public Player(Color aColor, PlayerIndex aPlayerIndex) : base()

        {
            points = 100;
            scoreTimer = 0;

            color = aColor;
            playerIndex = aPlayerIndex;
            oldGamePad = GamePad.GetState(playerIndex);

            maxPower = 5;
        }

        public void Update(GameTime gameTime)
        {
           
            HandleInput();
            // SetVelocity(new Vector2(1 * maxPower, 1 * maxPower));
            //Move((float)Math.PI);
            if (fire)
                Move(angle);
            timer = (timer + 1) % 30;
            if (timer==29)
                 SetVelocity(new Vector2(0, 0));
            if (velocity.X == 0 && velocity.Y == 0)
            {
                angle = 0;
                old_dist = 0;
                dist = 0;
                fire = false;
            }
            base.Update(gameTime);
            //old_angle = angle;
          
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void HandleInput()
        {
            
            gamePad = GamePad.GetState(playerIndex);

            // basic movement
            Vector2 leftStick = gamePad.ThumbSticks.Left;
            Console.WriteLine(""+leftStick.X+" "+leftStick.Y);
            if (leftStick.Y != 0 || leftStick.X != 0)
                angle = (float)Math.Atan2(leftStick.Y, leftStick.X) + (float)Math.PI; // in radians
                                                                     // no negative angles, just around the unit circle (0 to 360 degrees)


            //if (angle < 0)
            //    angle += (float)(Math.PI * 2);

            // distance between center of stick axis and it's current position
            //if ((leftStick.Y >= .4 || leftStick.Y <=-.4)||(leftStick.X>=.4 || leftStick.X<=-.4))
                dist = (float)Math.Sqrt(Math.Pow(leftStick.X, 2) + Math.Pow(leftStick.Y, 2));
            if (dist > old_dist)
                old_dist = dist;
            if (dist == 0 && old_dist > 0)
                fire = true;
            float percentPower = 0;

            // set velocity as a percentage of the max power\

           
                SetVelocity(new Vector2((old_dist * maxPower), (old_dist * maxPower)));
            // move based on angle and velocity
            Console.WriteLine("" + angle);
           
          
            Console.WriteLine("called Move(angle)");

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

        //private void HandleInput()
        //{
        //    GamePadState gamePad = GamePad.GetState(playerIndex);

        //    // basic movement
        //    Vector2 leftStick = gamePad.ThumbSticks.Left;

        //    angle = (float)Math.Atan2(leftStick.Y, leftStick.X); // in radians
        //    // no negative angles, just around the unit circle (0 to 360 degrees)
        //    if (angle < 0)
        //        angle += (float)(Math.PI * 2);

        //    // distance between center of stick axis and it's current position
        //    float dist = (float)Math.Sqrt(Math.Pow(leftStick.X, 2) + Math.Pow(leftStick.Y, 2));
        //    float percentPower = 0;

        //    // set velocity as a percentage of the max power
        //    SetVelocity(new Vector2(dist * maxPower, dist * maxPower));

        //    // move based on angle and velocity
        //    Move(angle);

        //    // start button - open pause menu
        //    if (gamePad.Buttons.Start.Equals(ButtonState.Pressed) &&
        //        !oldGamePad.Buttons.Start.Equals(ButtonState.Pressed))
        //    {
        //        Console.WriteLine("Pause menu");
        //    }

        //    // A button - use power-up
        //    if (gamePad.Buttons.A.Equals(ButtonState.Pressed) &&
        //        !oldGamePad.Buttons.A.Equals(ButtonState.Pressed))
        //    {
        //        Console.WriteLine("Use powerup");
        //    }

        //    // B button - cancel shot
        //    if (gamePad.Buttons.B.Equals(ButtonState.Pressed) &&
        //        !oldGamePad.Buttons.B.Equals(ButtonState.Pressed))
        //    {
        //        Console.WriteLine("Cancel shot");
        //    }

        //    oldGamePad = gamePad;
        //}


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
