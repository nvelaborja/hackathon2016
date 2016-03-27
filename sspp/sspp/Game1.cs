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

        #region Textures

        Texture2D texture_goalFront;

        #endregion

        #region Game Logic Members

        private bool paused;
        

        #endregion

        #region Game Objects

        Player player1;
        Player player2;
        Overlay overlay;
        SoundManager sound;
        Background background;
        CollisionHandler collisionHandler;
        Ball ball;
        Force gravity;
        List<object> PhysicsObjects;
        

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

            player1 = new Player(1);
            player2 = new Player(2);
            background = new Background();
            sound = new SoundManager();
            ball = new Ball();
            overlay = new Overlay();

            gravity = new Force(0, 1, 60);

            PhysicsObjects = new List<object>();
            PhysicsObjects.Add(player1);
            PhysicsObjects.Add(player2);
            PhysicsObjects.Add(ball);

            collisionHandler = new CollisionHandler(ref player1, ref player2, ref ball);

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            sound.LoadContent(Content);
            background.LoadContent(Content);
            overlay.LoadContent(Content);
            player1.LoadContent(Content);
            player2.LoadContent(Content);
            ball.LoadContent(Content);
            collisionHandler.LoadContent(Content);

            texture_goalFront = Content.Load<Texture2D>("goal_front");

            sound.LoopMenu();
        }
        
        protected override void UnloadContent()
        {
          
        }
        
        protected override void Update(GameTime gameTime)
        {
            // Take out once we get menu functional
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ApplyGravity();

            player1.Update(gameTime);
            player2.Update(gameTime);
            ball.Update(gameTime);

            collisionHandler.Update(gameTime);

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            GraphicsDevice.Clear(Color.White);

            background.Draw(spriteBatch);

            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);
            ball.Draw(spriteBatch);
            
            spriteBatch.Draw(texture_goalFront, new Vector2(0,0), Color.White);

            overlay.Draw(spriteBatch);

            base.Draw(gameTime);

            spriteBatch.End();
        }

        private void ApplyGravity()
        {
            player1.AcceptForce(gravity);
            player2.AcceptForce(gravity);
            ball.AcceptForce(gravity);
        }
    }
}
