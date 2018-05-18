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
        public static Texture2D cueStickTexture;
        static Vector2 cueStickPivot;
        float cueStickPowerMultiplier = 20;

        int points = 100;
        int scoreTimer = 0;

        public PlayerIndex playerIndex;
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

        bool cancelingShot = false;

        float aimingFriction = .7f;
        float nonaimingFriction = .07f;

       
        Board board;
        
        public PowerupType powerupType = PowerupType.Null;

        int powerupEffectTimer;
        int powerupEffectTimerLimit;
        bool usingPowerup;

       public  GamePadState gamePad ;
        
        public Player(Color aColor, PlayerIndex aPlayerIndex, Board aBoard) : base()
        {
            color = aColor;
            playerIndex = aPlayerIndex;
            oldGamePad = GamePad.GetState(playerIndex);

            //should mass be greater than normal?
            SetMass(20);

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

            board = aBoard;
            
            powerupEffectTimer = 0;
            powerupEffectTimerLimit = 1200;
            usingPowerup = false;
        }

        public void Update(GameTime gameTime)
        {
            HandleInput();

            if (usingPowerup)
            {
                // update countdown timer - TODO: use gameTime so it's more stable(?)
                powerupEffectTimer = (powerupEffectTimer + 1) % (powerupEffectTimerLimit + 1);

                if (powerupType == PowerupType.BigBall)
                    ChangeRadiusOverTime(Powerup.bigRadius);

                // if the timer reaches the end
                if (powerupEffectTimer == powerupEffectTimerLimit)
                    RemovePowerupEffects();
            }

            base.Update(gameTime);

        }

        new public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (!isZero && !cancelingShot)
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

        public void CollectPowerup(Powerup p)
        {
            powerupType = p.GetPowerupType();
            board.RemovePowerup(p);
        }

        private void ApplyPowerupEffects()
        {
            // apply effects
            switch (powerupType)
            {
                case PowerupType.BigBall:
                    // increase radius
                    ChangeRadiusOverTime(Powerup.bigRadius);
                    SetMass(1000);
                    usingPowerup = true;
                    break;
                case PowerupType.Bomb:
                    Powerup.BombActivate(this);
                    RemovePowerupEffects();
                    break;
                case PowerupType.IncreasePower:
                    maxPower = Powerup.bigMaxPower;
                    usingPowerup = true;
                    break;
            }
        }

        private void RemovePowerupEffects()
        {
            // reset stats that were affected
            switch (powerupType)
            {
                case PowerupType.BigBall:
                    // reset radius
                    SetRadius(Powerup.normalRadius);
                    SetMass(5);
                    break;
                case PowerupType.IncreasePower:
                    maxPower = Powerup.normalMaxPower;
                    break;
                default:
                    break;
            }

            // remove the powerup
            powerupType = PowerupType.Null;
            usingPowerup = false;
        }
        
        private void ChangeRadiusOverTime(double maxRadius)
        {
            double percentDone = (double)powerupEffectTimer / powerupEffectTimerLimit;
            double amountToIncrease = maxRadius - Powerup.normalRadius;

            // dividing by 95 and 500 keeps it from changing size too fast

            if (percentDone < .3) // first third of transition
                SetRadius(GetRadius() + (amountToIncrease * percentDone / 95));
            else if (percentDone >= .3 && percentDone < .6) // second third
                SetRadius(maxRadius);
            else // last third
                SetRadius(GetRadius() - (amountToIncrease * percentDone / 500));
        }
        
        public bool RestartButtonIsDown(PlayerIndex playerIndex)
        {
            GamePadState gamePad = GamePad.GetState(playerIndex);
            return gamePad.IsButtonDown(Buttons.X);
        }

        private void HandleInput()
        {
            GamePadState gamePad =  GamePad.GetState(playerIndex);
           
            // basic movement
            HandleMovement(gamePad.ThumbSticks.Left);

            // start button - open pause menu
            if (gamePad.Buttons.Start.Equals(ButtonState.Pressed) &&
                !oldGamePad.Buttons.Start.Equals(ButtonState.Pressed) && board.state == GameState.Play)
            {
                board.state = GameState.Pause;
            }

            // A button - use power-up
            if (gamePad.Buttons.A.Equals(ButtonState.Pressed) &&
                !oldGamePad.Buttons.A.Equals(ButtonState.Pressed) && board.state == GameState.Play)
            {
                if (powerupType != PowerupType.Null)
                {
                    ApplyPowerupEffects();
                }
            }

            // B button - cancel shot
            if (gamePad.Buttons.B.Equals(ButtonState.Pressed) &&
                !oldGamePad.Buttons.B.Equals(ButtonState.Pressed) && board.state==GameState.Play)
            {
                cancelingShot = true;
            }
            if (board.state == GameState.MainMenu && playerIndex==PlayerIndex.One)
            {
                if (gamePad.Buttons.B.Equals(ButtonState.Pressed) &&
                !oldGamePad.Buttons.B.Equals(ButtonState.Pressed))//play game
                {
                    board.state = GameState.Play;
                }
            }
            oldGamePad = gamePad;
        }

        public void HandlePauseMenuControls()
        {
            GamePadState gamePad = GamePad.GetState(playerIndex);

            if (gamePad.Buttons.A.Equals(ButtonState.Pressed) &&
                    !oldGamePad.Buttons.A.Equals(ButtonState.Pressed))//resume
            {
                board.state = GameState.Play;
            }

            if (gamePad.Buttons.B.Equals(ButtonState.Pressed) &&
                !oldGamePad.Buttons.B.Equals(ButtonState.Pressed))//restart
            {
                board.RestartGame();
            }

            if (gamePad.Buttons.X.Equals(ButtonState.Pressed) &&
                !oldGamePad.Buttons.X.Equals(ButtonState.Pressed))//main menu
            {
                board.RestartGame();
                board.state = GameState.MainMenu;
            }
        }

        private void HandleMovement(Vector2 aThumbstick)
        {
            Vector2 thumbstick;
            thumbstick = new Vector2(aThumbstick.X, -aThumbstick.Y);

            wasZero = isZero;
            isZero = thumbstick.LengthSquared() <= 0;

            if (cancelingShot)
            {
                SetFriction(nonaimingFriction);
                power = 0;
            }

            if (isZero)
            {
                cancelingShot = false;

                SetFriction(nonaimingFriction);

                if (!wasZero)
                {
                    SetVelocity(GetVelocity() + Physics.ScalarProduct(Physics.AngleToVector2(angle), power));
                    power = 0;
                }
            }
            else if (!cancelingShot)
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

                if (thumbstick.LengthSquared() > zeroBuffer * zeroBuffer) {
                    float targetAngle;
                    if (reversedMove)
                        targetAngle = Physics.Vector2ToAngle(-thumbstick);
                    else
                        targetAngle = Physics.Vector2ToAngle(thumbstick);

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


        public int GetPoints()
        {
            return points;
        }

        public PowerupType GetPowerupType()
        {
            return powerupType;
        }

        public Board GetBoard()
        {
            return board;

        }
    }
}
