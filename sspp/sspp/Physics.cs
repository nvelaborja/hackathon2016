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
        public const int ground = 500;
        private const int velocityDecay = 10;
        private Vector2 prevPosition;
        private int movementDamp = 100;

        #region Constructors

        public Physics(float Mass, Vector2 Position)
        {
            mass = Mass;
            prevPosition = Position;
            Initialize();
        }

        #endregion

        public int Ground
        {
            get { return ground; }
        }

        public float Velocity
        {
            get { return velocity; }
        }

        #region MonoGame Functions

        protected void Initialize()
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

        public void Update(GameTime gameTime, ref Vector2 position)
        {
            velocity = (float) Math.Sqrt((double)((Math.Pow(MathHelper.Distance(position.X, prevPosition.X), 2) + Math.Pow(MathHelper.Distance(position.Y, prevPosition.Y), 2))));

            trajectory.X = position.X - prevPosition.X;
            trajectory.Y = position.Y - prevPosition.Y;

            force.Magnitude = mass * velocity;
            force.Trajectory = trajectory;

            prevPosition = position;         
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

            newPosition.X += velocity * trajectory.X / movementDamp;
            newPosition.Y += velocity * trajectory.Y / movementDamp;

            return newPosition;
        }

        #endregion

        #region Helper Functions



        #endregion
    }
}
