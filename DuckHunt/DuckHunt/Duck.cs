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
    class Duck
    {
        static Texture2D texture;
        SpriteEffects spriteEffects;
        static Vector2 origin = new Vector2();
        Vector3 location, direction;
        Matrix transform, translation, rotation;

        /// <param name="_location">location for duck to spawn</param>
        /// <param name="_direction">direction of travel. can be a destination or a normalized vector, doesnt matter which</param>
        public Duck(Vector2 _location, Vector2 _direction)
        {
            location = new Vector3(_location, 0);
            direction = new Vector3(_direction, 0);
            direction.Normalize();//make sure direction variable holds a vector of length 1
            Matrix translation = Matrix.CreateTranslation(new Vector3(_location,0));
        }

        /// <param name="assetName">the asset name of the texture to be loaded</param>
        public static void LoadContent(ContentManager c, string assetName)
        {
            texture = c.Load<Texture2D>(assetName);
            //origin cannot be defined in constructor because texture may not have been loaded
            origin.X = texture.Bounds.Center.X;
            origin.Y = texture.Bounds.Center.Y;
        }

        public void Update(int gameWidth, int gameHeight)
        {
            //update location of duck
            location += direction;

            if ((location.X - texture.Width / 2) <= 0 || (location.X + texture.Width / 2) >= gameWidth)
            {
                direction.X *= -1;
            }//end if

            if ((location.Y - texture.Height / 2) <= 0 || (location.Y + texture.Height / 2) >= gameHeight)
            {
                direction.Y *= -1;
            }//end if

        }//end update()

        public void Draw(SpriteBatch sb)
        {
            //dont leave the duck upside down
            if (direction.X < 0)
            {
                spriteEffects = SpriteEffects.FlipVertically;
            }
            else
            {
                spriteEffects = SpriteEffects.None;
            }

            //calculate our matrices
            Matrix.CreateRotationZ((float)Math.Atan2(direction.Y, direction.X), out rotation);
            Matrix.CreateTranslation(ref location, out translation);
            transform = rotation * translation;

            sb.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transform);
            sb.Draw(texture, Vector2.Zero, null, Color.White, 0.0f, origin, Vector2.One, spriteEffects, 0.0f);
            sb.End();
        }

        #region Properties
        public Matrix Transform
        {
            get { return rotation; }
        }
        public Texture2D Texture
        {
            get { return texture; }
        }
        #endregion

    }//end class Duck
}
