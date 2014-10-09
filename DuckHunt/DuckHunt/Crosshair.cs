using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DuckHunt
{
    class Crosshair
    {
        Texture2D texture;
        Vector2 location, origin;
        Matrix translate;

        public Crosshair()
        {
        }

        public void LoadContent(ContentManager content, string assetName)
        {
            texture = content.Load<Texture2D>(assetName);
            //origin cannot be defined in constructor because texture may not have been loaded
            origin.X = texture.Bounds.Center.X;
            origin.Y = texture.Bounds.Center.Y;
        }

        public void Update(MouseState mouseState)
        {
            //track crosshairs to cursor position
            location.X = mouseState.X - texture.Width/2.0f;
            location.Y = mouseState.Y - texture.Height/2.0f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //calculate our matrices
            translate = Matrix.CreateTranslation(new Vector3(location, 0));

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, translate);
            spriteBatch.Draw(texture, location, Color.White);
            spriteBatch.End();
        }

        #region Properties
        public Matrix TranslationMatrix
        {
            get { return Matrix.Identity; }
        }
        public Texture2D Texture
        {
            get { return texture; }
        }
        #endregion
    }
}
