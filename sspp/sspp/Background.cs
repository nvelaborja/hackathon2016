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
    class Background
    {
        Texture2D texture;
        Vector2 position;

        #region Constructors

        public Background()
        {
            Initialize();
        }

        #endregion

        #region MonoGame Functions

        protected void Initialize()
        {
            position = new Vector2(0, 0);
        }

        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("field");
        }

        protected void UnloadContent(ContentManager Content)
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
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
