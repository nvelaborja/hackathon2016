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
        private float velocityX;
        private float velocityY;
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

        public float VelocityX
        {
            get { return velocityX; }
            set { velocityX = value; }
        }

        public float VelocityY
        {
            get { return velocityY; }
            set { velocityY = value; }
        }

        #region MonoGame Functions

        protected void Initialize()
        {
            velocityX = 0;
            velocityY = 0;
            trajectory = new Vector2();
            force = new Force((int)trajectory.X, (int)trajectory.Y, mass);
            maxVelocity = 30;
            minVelocity = -30;
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
            if (velocityX > maxVelocity) velocityX = maxVelocity;
            else if (velocityX < minVelocity) velocityX = minVelocity;
            if (velocityY > maxVelocity) velocityY = maxVelocity;
            else if (velocityY < minVelocity) velocityY = minVelocity;

            trajectory.X = physicsBody.Position.X - prevPosition.X;
            trajectory.Y = physicsBody.Position.Y - prevPosition.Y;

            trajectory = UnitVector((int)(physicsBody.Position.X - prevPosition.X), (int)(physicsBody.Position.Y - prevPosition.Y));

            force.Magnitude = (float) Math.Sqrt(velocityX * velocityX + velocityY * velocityY);
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
            velocityX += force.Magnitude * force.Trajectory.X;
            velocityY += force.Magnitude * force.Trajectory.Y;
            trajectory += force.Trajectory;
            trajectory = UnitVector((int)trajectory.X, (int)trajectory.Y);
        }

        public Vector2 GetNewPosition(Vector2 Position)
        {
            Vector2 newPosition = Position;

            newPosition.X += velocityX * mass / movementDamp;
            newPosition.Y += velocityY * mass / movementDamp;

            return newPosition;
        }

        #endregion

        #region Helper Functions

        private void DecrementVelocity()
        {
            if (velocityX > 0)
            {
                velocityX--;
                if (velocityX < 0) velocityX = 0;
            }
            else if (velocityX < 0)
            {
                velocityX++;
                if (velocityX > 0) velocityX = 0;
            }

            if (velocityY > 0)
            {
                velocityY--;
                if (velocityY < 0) velocityY = 0;
            }
            else if (velocityY < 0)
            {
                velocityY++;
                if (velocityY > 0) velocityY = 0;
            }
        }

        private Vector2 UnitVector(int x, int y)
        {
            Vector2 unitVector = new Vector2();
            float mag = 0f;

            mag = (float)Math.Sqrt(x * x + y * y);

            if (mag == 0)
            {
                unitVector.X = 0;
                unitVector.Y = 0;

                return unitVector;
            }

            unitVector.X = x / mag;
            unitVector.Y = y / mag;

            return unitVector;
        }
        #endregion
    }
}
