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
    class PhysicsBody
    {
        internal Texture2D texture;
        internal Vector2 position;
        public Vector2 defaultPosition;
        internal Physics phyics;
        internal Vector2 center;                               // How much smaller hitbox is than texture
        internal int radius;

        #region Constructors

        public PhysicsBody()
        {
        }

        #endregion

        public Physics Physics
        {
            get { return phyics; }
        }

        public int Radius
        {
            get { return radius; }
        }

        public Vector2 Center
        {
            get { return center; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        #region MonoGame Functions

        protected virtual void Initialize()
        {

        }

        public virtual void LoadContent(ContentManager Content)
        {

        }

        public virtual void UnloadContent(ContentManager Content)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            if (position.Y > Physics.Ground) position.Y = phyics.Ground;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        #endregion

        #region Gameplay Functions
        
        public virtual void UpdatePositionX(int d)
        {
        }

        public virtual void UpdatePositionY(int d)
        {
        }

        public void AcceptForce(Force force)
        {
            phyics.AcceptForce(force);
        }

        #endregion
    }
}
