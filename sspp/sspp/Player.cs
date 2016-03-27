﻿using System;
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
            phyics = new Physics(23, this);
            center = new Vector2(texture.Width / 2, texture.Height);
            radius = 50;

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

            //if (!Controller.IsConnected) paused = true;                                             // If either controller is disconnected, pause the game!

            

            if (Controller.ThumbSticks.Left.X != 0)
            {
                int Xdir = 0;
                if (Controller.ThumbSticks.Left.X > 0) Xdir = 1;
                else Xdir = -1;
                Force movement = new Force(Xdir, 0, (int)(Controller.ThumbSticks.Left.X * 100 / movementDamp));
                //UpdatePositionX((int)(Controller.ThumbSticks.Left.X * 100 / movementDamp));
                //Physics.Velocity = Controller.ThumbSticks.Left.X * 100 / movementDamp;
                phyics.AcceptForce(movement);
                position = Physics.GetNewPosition(Position);
                //phyics.AcceptForce(new Force(-(int)(Controller.ThumbSticks.Left.X * 100 / movementDamp), 0, 1));
            }
            base.Update(gameTime);

            if (position.X < 0 + goalBuffer) position.X = goalBuffer;
            else if (position.X > 1920 - texture.Width - goalBuffer) position.X = 1920 - texture.Width - goalBuffer;

            phyics.Update(gameTime);

            top.X = position.X + texture.Width / 2;
            top.Y = position.Y + texture.Height / 2;

            center.X = position.X + texture.Width / 2;
            center.Y = position.Y + texture.Height;

            if (Controller.IsButtonDown(Buttons.A))
            {
                Jump();
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
            Force jumpForce = new Force(0, -1, 25);

            if (position.Y == phyics.Ground)                  // Only can jump if on the ground
            {
                phyics.AcceptForce(jumpForce);

            }
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
            if (position.Y < phyics.Ground) position.Y = phyics.Ground;
        }
        
        #endregion

        #region Helper Functions



        #endregion

    }
}
