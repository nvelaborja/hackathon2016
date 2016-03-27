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
        private float maxVelocity;
        private float minVelocity;
        private Vector2 trajectory;
        private Force force;
        public const int ground = 500;
        private const int velocityDecay = 10;
        private Vector2 prevPosition;
        private int movementDamp = 100;
        private PhysicsBody physicsBody;

        #region Constructors

        public Physics(float Mass, PhysicsBody PhysicsBody)
        {
            mass = Mass;
            physicsBody = PhysicsBody;
            prevPosition = physicsBody.Position;
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
            set { velocity = value; }
        }

        #region MonoGame Functions

        protected void Initialize()
        {
            velocity = 0;
            trajectory = new Vector2();
            force = new Force((int)trajectory.X, (int)trajectory.Y, mass);
            maxVelocity = 16;
            minVelocity = -16;
        }

        protected void LoadContent(ContentManager Content)
        {
            
        }

        protected void UnloadContent(ContentManager Content)
        {

        }

        public void Update(GameTime gameTime)
        {
            //velocity = (float) Math.Sqrt((double)((Math.Pow(MathHelper.Distance(physicsBody.Position.X, prevPosition.X), 2) + Math.Pow(MathHelper.Distance(physicsBody.Position.Y, prevPosition.Y), 2))));
            DecrementVelocity();
            if (velocity > maxVelocity) velocity = maxVelocity;
            else if (velocity < minVelocity) velocity = minVelocity;

            trajectory.X = physicsBody.Position.X - prevPosition.X;
            trajectory.Y = physicsBody.Position.Y - prevPosition.Y;

            force.Magnitude = mass * velocity;
            force.Trajectory = trajectory;

            prevPosition = physicsBody.Position;         
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
            trajectory += force.Trajectory;
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

        private void DecrementVelocity()
        {
            if (velocity > 0)
            {
                velocity--;
                if (velocity < 0) velocity = 0;
            }
            else if (velocity < 0)
            {
                velocity++;
                if (velocity > 0) velocity = 0;
            }
        }
        #endregion
    }
}
