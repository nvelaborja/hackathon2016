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
    class Force
    {
        private float magnitude;
        private Vector2 trajectory;
        #region Constructors

        public Force(int x, int y, float m)
        {
            trajectory = new Vector2(x, y);
            magnitude = m;
        }

        #endregion

        public float Magnitude
        {
            get { return magnitude; }
            set { magnitude = value; }
        }

        public Vector2 Trajectory
        {
            get { return trajectory; }
            set { trajectory = value; }
        }

        #region MonoGame Functions

        protected void Initialize()
        {


        }

        protected void LoadContent(ContentManager Content)
        {


        }

        protected void UnloadContent(ContentManager Content)
        {

        }

        protected void Update(GameTime gameTime)
        {

        }

        protected void Draw(SpriteBatch spriteBatch)
        {

        }

        #endregion

        #region Gameplay Functions



        #endregion

        #region Helper Functions



        #endregion
    }
}
