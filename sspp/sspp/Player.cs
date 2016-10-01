using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace sspp
{
    class Player : PhysicsBody
    {
        private int playerNum;
        private Vector2 top;
        private Point zeroPoint;                                            // For use with control sticks
        private int movementDamp;                                           // Multiplier for control stick movement
        private int goalBuffer;
        private int jumpCoolDown;
        private SoundManager sounds;

        #region Constructors

        public Player(int PlayerNum)
        {
            playerNum = PlayerNum;
        }

        #endregion
        
        #region MonoGame Functions

        protected override void Initialize()
        {
            switch (playerNum)
            {
                case 1:
                    defaultPosition = new Vector2(400, 500);
                    break;
                case 2:
                    defaultPosition = new Vector2(1420, 500);
                    break;
                default:
                    break;
            }

            position = defaultPosition;
            top = new Vector2(position.X + texture.Width / 2, position.Y + texture.Width / 2);
            zeroPoint = new Point(0, 0);
            movementDamp = 6;
            goalBuffer = 100;
            physics = new Physics(40, this);
            center = new Vector2(texture.Width / 2, texture.Height);
            radius = 50;
            sounds = new SoundManager();

            // Initialize phyics

        }

        public override void LoadContent(ContentManager Content)
        {
            switch (playerNum)
            {
                case 1:
                    texture = Content.Load<Texture2D>("player_1");
                    break;
                case 2:
                    texture = Content.Load<Texture2D>("player_2");
                    break;
                default:
                    break;
            }

            this.Initialize();

            sounds.LoadContent(Content);
        }

        public override void UnloadContent(ContentManager Content)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState Controller = new GamePadState();                                           // Assign controller based on player number
            if (playerNum == 1)
                Controller = GamePad.GetState(PlayerIndex.One);
            else if (playerNum == 2)
                Controller = GamePad.GetState(PlayerIndex.Two);

            KeyboardState keyboardState = Keyboard.GetState();

            //if (!Controller.IsConnected) paused = true;                                             // If either controller is disconnected, pause the game!

            Force movement = new Force(0, 0, 0);

            if (Controller.IsConnected)
            {
                if (Controller.ThumbSticks.Left.X != 0)
                {
                    int Xdir = 0;
                    if (Controller.ThumbSticks.Left.X > 0) Xdir = 1;
                    else Xdir = -1;
                    if (Xdir != 0)
                    {
                        movement = new Force(Xdir, 0, (int)(Controller.ThumbSticks.Left.X * 100 / movementDamp));
                        physics.AcceptForce(movement);
                    }
                    position = Physics.GetNewPosition(Position);
                }
            }
            else
            {
                if (playerNum == 1)
                {
                    int Xdir = 0;
                    if (keyboardState.IsKeyDown(Keys.D)) Xdir = 1;
                    else if (keyboardState.IsKeyDown(Keys.A)) Xdir = -1;
                    if (Xdir != 0)
                    {
                        movement = new Force(Xdir, 0, 100 / movementDamp);
                        physics.AcceptForce(movement);
                    }
                    position = Physics.GetNewPosition(Position);
                }
                else
                {
                    int Xdir = 0;
                    if (keyboardState.IsKeyDown(Keys.Right)) Xdir = 1;
                    else if (keyboardState.IsKeyDown(Keys.Left)) Xdir = -1;
                    if (Xdir != 0)
                    {
                        movement = new Force(Xdir, 0, 100 / movementDamp);
                        physics.AcceptForce(movement);
                    }
                    position = Physics.GetNewPosition(Position);
                }
            }


            
            base.Update(gameTime);

            if (position.X < 0 + goalBuffer) position.X = goalBuffer;
            else if (position.X > 1920 - texture.Width - goalBuffer) position.X = 1920 - texture.Width - goalBuffer;

            physics.Update(gameTime);

            top.X = position.X + texture.Width / 2;
            top.Y = position.Y + texture.Height / 2;

            center.X = position.X + texture.Width / 2;
            center.Y = position.Y + texture.Height;

            if (Controller.IsConnected)
            {
                if (Controller.IsButtonDown(Buttons.A)) Jump();
            }
            else
            {
                if (playerNum == 1)
                {
                    if (keyboardState.IsKeyDown(Keys.Space)) Jump();
                }
                else if (keyboardState.IsKeyDown(Keys.NumPad0)) Jump();
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        #endregion

        #region Gameplay Functions

        private void Jump()
        {
            Force jumpForce = new Force(0, -1, 400);

            if (position.Y == physics.Ground)                  // Only can jump if on the ground
            {
                physics.AcceptForce(jumpForce);
                sounds.playJump(playerNum);
            }

            //position.Y -= 100;
        }

        public override void UpdatePositionX(int d)
        {
            position.X += d;
            if (position.X < 0 + goalBuffer) position.X = goalBuffer;
            else if (position.X > 1920 - texture.Width - goalBuffer) position.X = 1920 - texture.Width - goalBuffer;
        }

        public override void UpdatePositionY(int d)
        {
            position.Y += d;
            if (position.Y < physics.Ground) position.Y = physics.Ground;
        }
        
        #endregion

        #region Helper Functions



        #endregion

    }
}
