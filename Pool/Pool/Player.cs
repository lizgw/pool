﻿using System;
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
        public static Texture2D cueStickTexture;
        static Vector2 cueStickPivot;
        float cueStickPowerMultiplier = 20;

        int points = 100;
        int scoreTimer = 0;

        PlayerIndex playerIndex;
        GamePadState oldGamePad;
        
        bool reversedMove = true;
        float maxPowerPerFrame = 0.05f;
        float maxPower = 4f;

        float angle;
        float angleCorrectionRate = 0.04f;
        float zeroBuffer = .2f; //only will record normalizedThumbstick if pulled back far enough
        float power = 0;
        bool isZero = true;
        bool wasZero = true;

        float aimingFriction = .7f;
        float nonaimingFriction = .07f;

        public Player(Color aColor, PlayerIndex aPlayerIndex) : base()

        {
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
        }

        public void Update(GameTime gameTime)
        {
            HandleInput();
            base.Update(gameTime);          
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (!isZero)
                DrawCueStick(spriteBatch);
        }

        public static void SetCueStickTexture(Texture2D aTexture)
        {
            cueStickTexture = aTexture;
            cueStickPivot = new Vector2(0, aTexture.Bounds.Height / 2);
        }

        private void DrawCueStick(SpriteBatch spriteBatch)
        {
            Vector2 cueStickPos = GetPos() - Physics.ScalarProduct(Physics.AngleToVector2(angle), GetRadius() + power * cueStickPowerMultiplier);
            spriteBatch.Draw(cueStickTexture, cueStickPos, cueStickTexture.Bounds, Color.White, angle + (float)Math.PI, cueStickPivot, 1, SpriteEffects.None, 1);
        }

        private void HandleInput()
        {
            GamePadState gamePad = GamePad.GetState(playerIndex);

            // basic movement
            HandleMovement(gamePad.ThumbSticks.Left);

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

        private void HandleMovement(Vector2 aThumbstick)
        {
            Vector2 thumbstick;
            thumbstick = new Vector2(aThumbstick.X, -aThumbstick.Y);

            wasZero = isZero;
            isZero = thumbstick.LengthSquared() <= 0;

            if (isZero)
            {
                SetFriction(nonaimingFriction);

                if (!wasZero)
                {
                    SetVelocity(GetVelocity() + Physics.ScalarProduct(Physics.AngleToVector2(angle), power));
                    power = 0;
                }
            }
            else 
            {
                SetFriction(aimingFriction);

                if (wasZero)
                {
                    if (reversedMove)
                        angle = Physics.Vector2ToAngle(-thumbstick);
                    else
                        angle = Physics.Vector2ToAngle(thumbstick);
                }

                power += maxPowerPerFrame * thumbstick.Length();
                if (power > maxPower)
                    power = maxPower;

                //this needs to be implemented into the GUI later on, as well as stamina
                //Console.WriteLine(power);

                if (thumbstick.LengthSquared() > zeroBuffer * zeroBuffer) {
                    float targetAngle;
                    if (reversedMove)
                        targetAngle = Physics.Vector2ToAngle(-thumbstick);
                    else
                        targetAngle = Physics.Vector2ToAngle(thumbstick);

                    //Console.WriteLine("Angle: " + angle);
                    //Console.WriteLine("Target Angle: " + targetAngle);

                    float angleDifference = AngleDifference(angle, targetAngle);

                    angle += angleCorrectionRate * angleDifference;
                }
            }
        }

        private float AngleDifference(float startAngle, float endAngle)
        {
            //assume that startAngle is zero, then convert endAngle to fit within [-pi, pi]
            float output = endAngle - startAngle;

            while (output <= -Math.PI - 0.01)
                output += (float)(2 * Math.PI);
            while (output >= Math.PI + 0.01)
                output -= (float)(2 * Math.PI);

            return output;
        }

        public int GetPoints()
        {
            return points;
        }

        // returns true if the player won
        public bool CountDown()
        {
            //I don't understand why, but it seems to be incrementing the scoreTimer twice as fast as it should be - so 120 instead of 60
            scoreTimer = (scoreTimer + 1) % 120;
            if (scoreTimer == 119 && points > 0)
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
