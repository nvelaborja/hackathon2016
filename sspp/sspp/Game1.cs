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
        Texture2D texture_goalFront;
        SpriteFont font;
        private bool controllerMode1 = false;
        private bool controllerMode2 = false;

        #region Game Logic Members

        private bool paused;
        private bool menu;
        private int score1 = 0;
        private int score2 = 0;
        private Vector2 score1Pos;
        private Vector2 score2Pos;

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
        Rectangle goal1 = new Rectangle(100, 320, 88, 208);
        Rectangle goal2 = new Rectangle(1707, 320, 88, 208);
        Rectangle oob1 = new Rectangle(240, -85, 3, 379);
        Rectangle oob2 = new Rectangle(1675, -85, 3, 379);

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

            score1Pos = new Vector2(572, 30);
            score2Pos = new Vector2(1300, 30);

            

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

            font = Content.Load<SpriteFont>("Font");

            sound.LoopMenu();
        }
        
        protected override void UnloadContent()
        {
          
        }
        
        protected override void Update(GameTime gameTime)
        {
            SearchForControllers();

            if (menu || paused)
            {
                ProcessMenuInput();
            }
            else
            {
                ProcessGameInput();

                ApplyGravity();

                player1.Update(gameTime);
                player2.Update(gameTime);
                ball.Update(gameTime);

                collisionHandler.Update(gameTime);

                CheckGoals();
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

            spriteBatch.DrawString(font, score1.ToString(), score1Pos, Color.White);
            spriteBatch.DrawString(font, score2.ToString(), score2Pos, Color.White);

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

            spriteBatch.DrawString(font, ".", new Vector2(goal1.X, goal1.Y), Color.Red);
            spriteBatch.DrawString(font, ".", new Vector2(goal1.X + goal1.Width, goal1.Y), Color.Red);
            spriteBatch.DrawString(font, ".", new Vector2(goal1.X, goal1.Y + goal1.Height), Color.Red);
            spriteBatch.DrawString(font, ".", new Vector2(goal1.X + goal1.Width, goal1.Y + goal1.Height), Color.Red);

            spriteBatch.DrawString(font, ".", new Vector2(goal2.X, goal2.Y), Color.Red);
            spriteBatch.DrawString(font, ".", new Vector2(goal2.X + goal2.Width, goal2.Y), Color.Red);
            spriteBatch.DrawString(font, ".", new Vector2(goal2.X, goal2.Y + goal2.Height), Color.Red);
            spriteBatch.DrawString(font, ".", new Vector2(goal2.X + goal2.Width, goal2.Y + goal2.Height), Color.Red);

            spriteBatch.DrawString(font, ".", new Vector2(oob1.X, oob1.Y), Color.Blue);
            spriteBatch.DrawString(font, ".", new Vector2(oob1.X + oob1.Width, oob1.Y), Color.Blue);
            spriteBatch.DrawString(font, ".", new Vector2(oob1.X, oob1.Y + oob1.Height), Color.Blue);
            spriteBatch.DrawString(font, ".", new Vector2(oob1.X + oob1.Width, oob1.Y + oob1.Height), Color.Blue);

            spriteBatch.DrawString(font, ".", new Vector2(oob2.X, oob2.Y), Color.Blue);
            spriteBatch.DrawString(font, ".", new Vector2(oob2.X + oob2.Width, oob2.Y), Color.Blue);
            spriteBatch.DrawString(font, ".", new Vector2(oob2.X, oob2.Y + oob2.Height), Color.Blue);
            spriteBatch.DrawString(font, ".", new Vector2(oob2.X + oob2.Width, oob2.Y + oob2.Height), Color.Blue);

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
                    Reset();
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

        private void Score(int playerNum)
        {
            sound.playGoal();

            if (playerNum == 1) score1++;
            else if (playerNum == 2) score2++;

            System.Threading.Thread.Sleep(3000);

            Reset();

            if (score1 == 5 || score2 == 5)
            {
                GameOver();
            }
        }

        private void CheckGoals()
        {
            if (DistanceFormulaVector2(ball.center, new Vector2(goal1.Right, ball.Center.Y)) < ball.Radius) Score(2);
            else if (DistanceFormulaVector2(ball.center, new Vector2(goal2.Left, ball.Center.Y)) < ball.Radius) Score(1);
            else if (DistanceFormulaVector2(ball.center, new Vector2(oob1.Right, ball.Center.Y)) < ball.Radius) Reset();
            else if (DistanceFormulaVector2(ball.center, new Vector2(oob2.Left, ball.Center.Y)) < ball.Radius) Reset();
        }

        private int DistanceFormulaVector2(Vector2 point1, Vector2 point2)
        {
            if (!GoodPoint(point1) || !GoodPoint(point2)) return -1;
            int distance = 0;

            distance = (int)Math.Sqrt(((Math.Pow(MathHelper.Distance(point2.X, point1.X), 2) + Math.Pow(MathHelper.Distance(point2.Y, point1.Y), 2))));

            return distance;
        }

        private void Reset()
        {
            player1.position = player1.defaultPosition;
            player2.position = player2.defaultPosition;
            ball.position = ball.defaultPosition;
        }

        private void ResetScore()
        {
            score1 = 0;
            score2 = 0;
        }

        private void GameOver()
        {
            sound.playWhistle();
            menu = true;
            ResetScore();
        }

        private bool GoodPoint(Vector2 point)
        {
            if (double.IsNaN(point.X) || double.IsNaN(point.Y)) return false;
            return true;
        }

        private bool GoodPoint(Point point)
        {
            if (double.IsNaN(point.X) || double.IsNaN(point.Y)) return false;
            return true;
        }

        private void ProcessMenuInput()
        {
            GamePadState Controller1 = GamePad.GetState(PlayerIndex.One);
            GamePadState Controller2 = GamePad.GetState(PlayerIndex.Two);
            KeyboardState keyboardState = Keyboard.GetState();
            bool controller1Up = false;
            bool controller1Down = false;
            bool controller2Up = false;
            bool controller2Down = false;
            bool select = false;

            if (keyboardState.IsKeyDown(Keys.I)) sound.playButtonClick();

            if (controllerMode1)
            {
                if (Controller1.ThumbSticks.Left.Y == 1) controller1Up = true;
                else if (Controller1.ThumbSticks.Left.Y == -1) controller1Down = true;
                if (Controller1.IsButtonDown(Buttons.A)) select = true;
            }
            else
            {
                if (keyboardState.IsKeyDown(Keys.W)) controller1Up = true;
                else if (keyboardState.IsKeyDown(Keys.S)) controller1Down = true;
                if (keyboardState.IsKeyDown(Keys.Space)) select = true;
            }
            if (controllerMode2)
            {
                if (Controller2.ThumbSticks.Left.Y == 1) controller2Up = true;
                else if (Controller2.ThumbSticks.Left.Y == -1) controller2Down = true;
                if (Controller2.IsButtonDown(Buttons.A)) select = true;
            }
            else
            {
                if (keyboardState.IsKeyDown(Keys.Up)) controller2Up = true;
                else if (keyboardState.IsKeyDown(Keys.Down)) controller2Down = true;
                if (keyboardState.IsKeyDown(Keys.NumPad0)) select = true;
            }

            if (controller1Up || controller2Up)
            {
                if (menuSelector == 2)
                {
                    menuSelector = 1;
                    sound.playButtonHover();
                }
            }
            else if (controller1Down || controller2Down)
            {
                if (menuSelector == 1)
                {
                    menuSelector = 2;
                    sound.playButtonHover();
                }
            }

            if (select)
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

        private void ProcessGameInput()
        {
            GamePadState Controller1 = GamePad.GetState(PlayerIndex.One);
            GamePadState Controller2 = GamePad.GetState(PlayerIndex.Two);
            KeyboardState keyboardState = Keyboard.GetState();

            if (Controller1.IsConnected && Controller2.IsConnected)
            {
                if (Controller1.IsButtonDown(Buttons.Start) || Controller2.IsButtonDown(Buttons.Start))
                {
                    sound.playButtonClick();
                    paused = true;
                }
                return;
            }

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                sound.playButtonClick();
                paused = true;
            }
        }

        private void SearchForControllers()
        {
            GamePadState Controller1 = GamePad.GetState(PlayerIndex.One);
            GamePadState Controller2 = GamePad.GetState(PlayerIndex.Two);

            if (Controller1.IsConnected) controllerMode1 = true;
            else controllerMode1 = false;
            if (Controller2.IsConnected) controllerMode2 = true;
            else controllerMode2 = false;
        }
    }
}
