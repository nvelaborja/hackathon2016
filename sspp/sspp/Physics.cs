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
    class Physics
    {
        private float mass;
        private float velocity;
        private Vector2 trajectory;
        private Force force;
        public const int ground = 620;
        private const int velocityDecay = 10;
        private Vector2 prevPosition;

        #region Constructors

        public Physics(float Mass, Vector2 Position)
        {
            mass = Mass;
            prevPosition = Position;
        }

        #endregion

        public int Ground
        {
            get { return ground; }
        }

        #region MonoGame Functions

        protected void Initialize(float mass)
        {
            velocity = 0;
            trajectory = new Vector2();
            force = new Force((int)trajectory.X, (int)trajectory.Y, mass);
        }

        protected void LoadContent(ContentManager Content)
        {


        }

        protected void UnloadContent(ContentManager Content)
        {

        }

        protected void Update(GameTime gameTime, ref Vector2 position)
        {
            velocity -= velocityDecay;

            trajectory.X = position.X - prevPosition.X;
            trajectory.Y = position.Y - prevPosition.Y;

            force.Magnitude = mass * velocity;
            force.Trajectory = trajectory;

            prevPosition = position;         
        }

        protected void Draw(SpriteBatch spriteBatch)
        {

        }

        #endregion

        #region Gameplay Functions

        public Force ApplyForce()
        {
            Force adjustedForce = force;


            return adjustedForce;
        }

        public void AcceptForce(Force force)
        {
            velocity += force.Magnitude;
            trajectory.X += force.Trajectory.X;
            trajectory.Y += force.Trajectory.Y;
        }

        public Vector2 GetNewPosition(Vector2 Position)
        {
            Vector2 newPosition = Position;

            

            return newPosition;
        }

        #endregion

        #region Helper Functions



        #endregion
    }
}
