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
            trajectory = UnitVector(x, y);
            Magnitude = m;
        }

        #endregion

        public float Magnitude
        {
            get { return magnitude; }
            set
            {
                magnitude = value;
                if (magnitude < 0) magnitude = -magnitude;
            }
        }

        public Vector2 Trajectory
        {
            get { return trajectory; }
            set
            {
                trajectory = value;
                trajectory = UnitVector((int)trajectory.X, (int)trajectory.Y);
            }
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

        private Vector2 UnitVector(int x, int y)
        {
            Vector2 unitVector = new Vector2();
            float mag = 0f;

            mag = (float)Math.Sqrt(x * x + y * y);

            unitVector.X = x / mag;
            unitVector.Y = y / mag;

            return unitVector;
        }

        #endregion
    }
}
