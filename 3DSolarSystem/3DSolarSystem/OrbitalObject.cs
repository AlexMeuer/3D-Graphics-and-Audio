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

namespace _3DSolarSystem
{
    class OrbitalObject
    {
        Model model;
		
        //orbitOffset is a translationMatrix along the x-axis; orbit and spin are both rotation matrices.
        Matrix transform, orbitOffset, orbit, spin;	//matrices concerning location, orbit and spin
		
		//view and projection matrices
		Matrix view, proj;
        float orbitRadians, orbitSpeed;
        float spinRadians, spinSpeed;

        #region Constructor(s)
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

        #region LoadContent
        public void LoadContent(ContentManager content, string assetName)
        {
            model = content.Load<Model>(assetName);
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

        public void Draw(SpriteBatch spriteBatch, Color colour, GraphicsDeviceManager graphics)
        {
            //set the matrices
            view = Matrix.CreateLookAt(new Vector3(40, 40, 20), Vector3.Zero, Vector3.Up);

            proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height, 0.1f, 100000);


            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.LightingEnabled = false;
                    effect.World = transform;
                    effect.Projection = proj;
                    effect.View = view;
                    effect.DiffuseColor = colour.ToVector3();
                }
                mesh.Draw();
            }
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
