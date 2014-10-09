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

namespace SolarSystem
{
    class OrbitalObject
    {
        Texture2D texture;
        //orbitOffset is a translationMatrix along the x-axis; orbit and spin are both rotation matrices.
        Matrix transform, orbitOffset, orbit, spin;
        float orbitRadians, orbitSpeed;
        float spinRadians, spinSpeed;

        #region Constructors
        /*public OrbitalObject()
        {
            //transform = Matrix.Identity;  unnecessary, assigned to in Update()
            translate = Matrix.Identity;
            rotate = Matrix.Identity;
        }*/

        /// <remarks>origin of rotation is set in LoadContent(), because it is defined using texture parameters</remarks>
        /// <param name="_rotateFirst">the order in which to apply the rotation and transformation matrices</param>
        /// <param name="_orbSpeed">the speed (in radians) of the orbit</param>
        /// <param name="offset">the distance at which the body should orbit</param>
        /// <param name="spinSpeed">the speed atwhich the object spin around its own centre z-axis</param>
        public OrbitalObject(float _orbSpeed/*, Viewport viewport*/, float offset, float _spinSpeed)
        {
            orbitRadians = 0;
            spinRadians = 0;
            orbitSpeed = _orbSpeed;
            spinSpeed = _spinSpeed;
            //transform = Matrix.Identity;  unnecessary, assigned to in Update()
            //translate = Matrix.CreateTranslation(new Vector3(viewport.Width / 2.0f, viewport.Height / 2.0f, 0f));
            orbitOffset = Matrix.CreateTranslation(new Vector3(offset, 0f, 0f));
            orbit = Matrix.CreateRotationZ(orbitRadians);
            spin = Matrix.CreateRotationZ(spinSpeed);
        }

        #endregion

        /*  cannot reference texture before Loadcontent() has been called
        public OribitalObject(Vector2 startPosition)
        {
            transform = Matrix.Identity;
            translate = Matrix.Identity;
            rotate = Matrix.Identity;

            //centre the orbital object at its translation
            Vector3 centredPosition = new Vector3(startPosition.X - texture.Width / 2.0f, startPosition.Y - texture.Height / 2.0f, 0);
            translate = Matrix.CreateTranslation(centredPosition);
        }*/

        #region LoadContent
        public void LoadContent(ContentManager content, string assetName)
        {
            texture = content.Load<Texture2D>(assetName);
        }

        public void LoadContent(ContentManager content, string assetName, Vector3 startingPosition)
        {
            texture = content.Load<Texture2D>(assetName);

            //centre the orbital object at its translation
            //Vector3 centredPosition = new Vector3(startingPosition.X - texture.Width / 2.0f, startingPosition.Y - texture.Height / 2.0f, 0.0f);
            orbitOffset = Matrix.CreateTranslation(startingPosition);
        } 
        #endregion

        //orbits about the origin (0,0)
        public void Update()
        {
            orbitRadians += orbitSpeed;
            spinRadians += spinSpeed;
            Matrix.CreateRotationZ(spinRadians, out spin);
            Matrix.CreateRotationZ(orbitRadians, out orbit);

            transform = spin * orbitOffset * orbit;
        }

        //orbits around a relative translation
        public void Update(Matrix relativeTranslation)
        {
            orbitRadians += orbitSpeed;
            spinRadians += spinSpeed;
            Matrix.CreateRotationZ(spinRadians, out spin);
            Matrix.CreateRotationZ(orbitRadians, out orbit);

            //rotation * translation * rotation
            transform = spin * orbitOffset * orbit;

            transform *= relativeTranslation;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transform);
            //we adjust here for the size of the texture(so it rotates around centre not corner)
            //so that we don't mess up our translation matrix (was stuck here for a while)
            spriteBatch.Draw(texture, new Vector2(0 - texture.Width / 2f, 0 - texture.Height / 2f), /*null,*/ Color.White/*, radians, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0f*/);
            spriteBatch.End();
        }

        #region Properties
        public Matrix TransformationMatrix
        {
            get { return transform; }
            set { transform = value; }
        }
        public Matrix TranslationMatrix
        {
            get { return Matrix.CreateTranslation(transform.Translation); }
            //set { translate = value; }
        }
        #endregion
    }
}
