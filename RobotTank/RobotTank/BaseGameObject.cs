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

namespace RobotTank
{
    class BaseGameObject
    {
        protected Texture2D texture;
        protected Matrix transformM, translateM, rotateM;
        static protected Matrix cameraM;

        public virtual void LoadContent(ContentManager content, string texturePath)
        {
            texture = content.Load<Texture2D>(texturePath);
        }

        public virtual void Update()
        {
            transformM = rotateM * translateM * cameraM;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transformM);
            spriteBatch.Draw(texture, Vector2.Zero, null, Color.White);
            spriteBatch.End();
        }

        #region PROPERTIES
        public Matrix TransformMatrix
        {
            get { return transformM; }
            //no set
        }
        #endregion
    }
}
