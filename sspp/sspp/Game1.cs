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
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Game Logic Members

        private bool paused;

        #endregion

        public Game1()
        {
            #region Graphics and Directory Setup

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            #endregion

            #region Game Window Setup

            graphics.PreferredBackBufferWidth = 1920;                                   // Window Width
            graphics.PreferredBackBufferHeight = 720;                                   // Window Height
            graphics.IsFullScreen = false;                                              // Don't allow fullscreen mode
            Window.Title = "SSPP";                                                      // Set window Title to game title
            Window.IsBorderless = true;                                                 // Hide top title bar
            Window.Position = new Point(2560 / 2 - 1920 / 2, 1440 / 2 - 720 / 2);       // Center screen, will need to change for non-1440p monitors

            #endregion
            
            
        }

        protected override void Initialize()
        {
            #region Game Logic Initialization

            paused = false;

            #endregion

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            
        }
        
        protected override void UnloadContent()
        {
          
        }
        
        protected override void Update(GameTime gameTime)
        {
            GamePadState Controller1 = GamePad.GetState(PlayerIndex.One);
            GamePadState Controller2 = GamePad.GetState(PlayerIndex.Two);

            // Take out once we get menu functional
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!Controller1.IsConnected || !Controller2.IsConnected) paused = true;               // If either controller is disconnected, pause the game!



            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);



            base.Draw(gameTime);
        }
    }
}
