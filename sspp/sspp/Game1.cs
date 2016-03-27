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
        private bool menu;

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

        #region Menu Objects

        Texture2D mainMenu;
        Texture2D pauseMenu;
        Texture2D highligher;
        Vector2 playPos;
        Vector2 exitPos;
        int menuSelector = 1;
        int buttonCoolDown = 0;

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
            menu = true;

            #endregion

            player1 = new Player(1);
            player2 = new Player(2);
            background = new Background();
            sound = new SoundManager();
            ball = new Ball();
            overlay = new Overlay();

            gravity = new Force(0, 1, 4);

            PhysicsObjects = new List<object>();
            PhysicsObjects.Add(player1);
            PhysicsObjects.Add(player2);
            PhysicsObjects.Add(ball);

            collisionHandler = new CollisionHandler(ref player1, ref player2, ref ball);

            playPos = new Vector2(550, 300);
            exitPos = new Vector2(550, 400);

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
            mainMenu = Content.Load<Texture2D>("menu");
            pauseMenu = Content.Load<Texture2D>("pause");
            highligher = Content.Load<Texture2D>("highlight");

            sound.LoopMenu();
        }
        
        protected override void UnloadContent()
        {
          
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (menu || paused)
            {
                GamePadState Controller1 = GamePad.GetState(PlayerIndex.One);
                GamePadState Controller2 = GamePad.GetState(PlayerIndex.Two);

                if (Controller1.ThumbSticks.Left.Y == 1 || Controller2.ThumbSticks.Left.Y == 1)
                {
                    if (menuSelector == 2)
                    {
                        menuSelector = 1;
                        sound.playButtonHover();
                    }
                }
                else if (Controller1.ThumbSticks.Left.Y == -1 || Controller2.ThumbSticks.Left.Y == -1)
                {
                    if (menuSelector == 1)
                    {
                        menuSelector = 2;
                        sound.playButtonHover();
                    }
                }

                if (Controller1.IsButtonDown(Buttons.A) || Controller2.IsButtonDown(Buttons.A))
                {
                    if (buttonCoolDown < 0)
                    {
                        buttonCoolDown = 60;
                        sound.playButtonClick();
                        Select();
                    }
                    
                }
                buttonCoolDown--;
            }
            else
            {
                GamePadState Controller1 = GamePad.GetState(PlayerIndex.One);
                GamePadState Controller2 = GamePad.GetState(PlayerIndex.Two);

                if (Controller1.IsButtonDown(Buttons.Start) || Controller2.IsButtonDown(Buttons.Start))
                {
                    sound.playButtonClick();
                    paused = true;
                }

                ApplyGravity();

                player1.Update(gameTime);
                player2.Update(gameTime);
                ball.Update(gameTime);

                collisionHandler.Update(gameTime);
            }
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

            if (menu)
            {
                if (menuSelector == 1) spriteBatch.Draw(highligher, playPos, Color.White);
                else if (menuSelector == 2) spriteBatch.Draw(highligher, exitPos, Color.White);
                spriteBatch.Draw(mainMenu, new Vector2(0, 0), Color.White);
            }

            if (paused)
            {
                if (menuSelector == 1) spriteBatch.Draw(highligher, playPos, Color.White);
                else if (menuSelector == 2) spriteBatch.Draw(highligher, exitPos, Color.White);
                spriteBatch.Draw(pauseMenu, new Vector2(0, 0), Color.White);
            }

            base.Draw(gameTime);

            spriteBatch.End();
        }

        private void ApplyGravity()
        {
            player1.AcceptForce(gravity);
            player2.AcceptForce(gravity);
            ball.AcceptForce(gravity);
        }

        private void Select()
        {
            if (menu)
            {
                if (menuSelector == 1)
                {
                    menu = false;
                }
                else if (menuSelector == 2)
                {
                    System.Threading.Thread.Sleep(3500);
                    Exit();
                }
            }

            if (paused)
            {
                if (menuSelector == 1)
                {
                    paused = false;
                }
                else if (menuSelector == 2)
                {
                    paused = false;
                    menu = true;
                }
            }
        }
    }
}
