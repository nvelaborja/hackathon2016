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
    class Player
    {
        private int playerNum;
        private Texture2D texture;
        private Vector2 position;
        private Vector2 positionHitbox;
        public Vector2 defaultPosition;
        private Physics phyics;
        private Vector2 center;
        private Rectangle hitbox;
        private int hitboxBuff;                                             // How much smaller hitbox is than texture
        private Point zeroPoint;                                            // For use with control sticks
        private int movementDamp;                                           // Multiplier for control stick movement
        private int goalBuffer;

        #region Constructors

        public Player(int PlayerNum)
        {
            playerNum = PlayerNum;
        }

        #endregion

        #region MonoGame Functions

        protected void Initialize()
        {
            switch (playerNum)
            {
                case 1:
                    defaultPosition = new Vector2(400, 520);
                    break;
                case 2:
                    defaultPosition = new Vector2(1420, 520);
                    break;
                default:
                    break;
            }

            position = defaultPosition;
            center = new Vector2(position.X + texture.Width / 2, position.Y + texture.Width / 2);
            hitboxBuff = 10;
            positionHitbox = new Vector2(position.X + hitboxBuff / 2, position.Y + hitboxBuff / 2);
            hitbox = new Rectangle((int) positionHitbox.X, (int) positionHitbox.Y, texture.Width - hitboxBuff, texture.Height - hitboxBuff);
            zeroPoint = new Point(0, 0);
            movementDamp = 4;
            goalBuffer = 100;
            phyics = new Physics(23, position);

            // Initialize phyics

        }
        
        public void LoadContent(ContentManager Content)
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
            Initialize();
        }

        public void UnloadContent(ContentManager Content)
        {
            
        }

        public void Update(GameTime gameTime)
        {
            GamePadState Controller = new GamePadState();                                           // Assign controller based on player number
            if (playerNum == 1) Controller = GamePad.GetState(PlayerIndex.One);
            else if (playerNum == 2) Controller = GamePad.GetState(PlayerIndex.Two);

            //if (!Controller.IsConnected) paused = true;                                             // If either controller is disconnected, pause the game!

            if (Controller.ThumbSticks.Left.X != 0)
            {
                UpdatePositionX((int)(Controller.ThumbSticks.Left.X * 100 / movementDamp));
            }


            center.X = position.X + texture.Width / 2;
            center.Y = position.Y + texture.Height / 2;

            positionHitbox.X = position.X + hitboxBuff / 2;
            positionHitbox.Y = position.Y + hitboxBuff / 2;

            hitbox.X = (int)positionHitbox.X;
            hitbox.Y = (int)positionHitbox.Y;

            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        #endregion

        #region Gameplay Functions

        public void UpdatePositionX(int d)
        {
            position.X += d;
            if (position.X < 0 + goalBuffer) position.X = goalBuffer;
            else if (position.X > 1920 - texture.Width - goalBuffer) position.X = 1920 - texture.Width - goalBuffer;
        }

        public void UpdatePositionY(int d)
        {
            position.Y += d;
            if (position.Y < phyics.Ground) position.Y = phyics.Ground;
        }

        public void AcceptForce(Force force)
        {
            phyics.AcceptForce(force);
        }

        #endregion

        #region Helper Functions



        #endregion

    }
}
