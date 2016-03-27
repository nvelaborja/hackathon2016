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
    class Ball
    {
        private Texture2D texture;
        private Vector2 position;
        public Vector2 defaultPosition;
        private Physics phyics;
        private Vector2 center;
        private int radius;

        #region Constructors

        public Ball()
        {
           
        }

        #endregion

        #region MonoGame Functions

        protected void Initialize()
        {
            defaultPosition = new Vector2(960, 500);
            position = defaultPosition;
            center = new Vector2(position.X + texture.Width / 2, position.Y + texture.Width / 2);
            radius = texture.Width / 2 - 2;
            phyics = new Physics(4, position);
        }

        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("ball_1");


            Initialize();
        }

        public void UnloadContent(ContentManager Content)
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public void AcceptForce(Force force)
        {
            phyics.AcceptForce(force);
        }

        #endregion

        #region Gameplay Functions



        #endregion

        #region Helper Functions



        #endregion

    }
}
