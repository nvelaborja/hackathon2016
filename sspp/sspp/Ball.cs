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
    class Ball : PhysicsBody
    {

        #region Constructors

        public Ball()
        {
           
        }

        #endregion
        
        #region MonoGame Functions

        protected override void Initialize()
        {
            defaultPosition = new Vector2(935, 520);
            position = defaultPosition;
            center = new Vector2(position.X + texture.Width / 2, position.Y + texture.Width / 2);
            radius = texture.Width / 2 - 2;
            phyics = new Physics(4, this);
        }

        public override void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("ball_1");


            Initialize();
        }

        public override void UnloadContent(ContentManager Content)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            phyics.Update(gameTime);

            position = phyics.GetNewPosition(position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
        

        #endregion

        #region Gameplay Functions



        #endregion

        #region Helper Functions



        #endregion

    }
}
