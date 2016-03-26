using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace sspp
{
    public class SplashScreen : Game
    {
        #region Class Members

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SoundManager sound;
        private Texture2D title;
        private Texture2D backGround;
        private Vector2 zeroPosition;
        private bool done;
        private double time;
        private bool splashed;
        int mAlphaValue = 1;
        int mFadeIncrement = 3;
        double mFadeDelay = .035;

        #endregion

        public SplashScreen()
        {
            #region Graphics and Directory Setup

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            #endregion

            #region Game Window Setup

            graphics.PreferredBackBufferWidth = 1920;                                   // Window Width
            graphics.PreferredBackBufferHeight = 720;                                   // Window Height
            graphics.IsFullScreen = false;                                              // Don't allow fullscreen mode
            Window.Title = "SS++";                                                      // Set window Title to game title
            Window.IsBorderless = true;                                                 // Hide top title bar
            Window.Position = new Point(2560 / 2 - 1920 / 2, 1440 / 2 - 720 / 2);       // Center screen, will need to change for non-1440p monitors

            #endregion
        }

        protected override void Initialize()                                            // Initialize member variables
        {
            sound = new SoundManager();

            sound.LoadContent(Content);
            sound.playSplash();

            done = false;
            time = 0;
            splashed = false;

            base.Initialize();
        }

        protected override void LoadContent()                                           // Load all member's content using their LoadContent functions
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Splash screen images here

            zeroPosition = new Vector2(0, 0);

            IsMouseVisible = false;
        }


        protected override void UnloadContent()                                         // Unload Content before closing game ( I think only needed when multiple scenes are used )
        {
            Content.Unload();
            base.Exit();
        }

        protected override void Update(GameTime gameTime)                               // Calls all member's update functions (happens once per frame, caps at 60fps)
        {
            if (done)
            {
                UnloadContent();
                
                // Load and run menu / game

                splashed = true;
                Exit();
            }

            time += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (time > 6000)
            {
                done = true;
            }

            mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;

            if (mFadeDelay <= 0)
            {
                mFadeDelay = 0.035;
                mAlphaValue += mFadeIncrement;

                if (mAlphaValue >= 255 || mAlphaValue <= 0)
                {
                    mFadeIncrement *= -1;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)                                 // Actually draws elements onto the window so we can see them
        {
            GraphicsDevice.Clear(Color.Gray);                                           // Clears window every frame with Gray background

            spriteBatch.Begin();                                                        // Starts spritebatch, the object that contains all the drawing functions

            //spriteBatch.Draw(/* Background */, zeroPosition, new Color(255, 255, 255, (byte)MathHelper.Clamp(mAlphaValue, 0, 255)));

            //spriteBatch.Draw(/* Title  */, zeroPosition, Color.White);

            spriteBatch.End();                                                          // Must end the spritebatch after each frame (not sure why)

            base.Draw(gameTime);                                                        // TBH, no idea what this does
        }
    }
}
