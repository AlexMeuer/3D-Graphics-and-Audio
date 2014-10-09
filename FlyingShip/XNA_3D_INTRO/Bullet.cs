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

namespace FlyingShip
{
    class Bullet
    {
        static Model model;
        Vector3 position, direction;
        float speed;
        Matrix scaleM;
        BoundingSphere boundingSphere;

        public Bullet(float initialSpeed, Vector3 parentPosition, Vector3 parentDirection, Quaternion parentRotation)
        {
            position = parentPosition;
            speed = initialSpeed;
            direction = Vector3.Transform(parentDirection, parentRotation);
            //direction = Vector3.Transform(position, parentRotation);    //make the bullet fire in the correct direction
            //direction.Normalize();

            scaleM = Matrix.CreateScale(100f);

            boundingSphere = model.Meshes[0].BoundingSphere;
            boundingSphere.Radius *= 100f;
        }

        public static void LoadContent(ContentManager content)
        {
            model = content.Load<Model>("Cone");
        }

        public void Update()
        {
            position += direction * speed;

            boundingSphere.Center = position;
        }

        /// <param name="world">world matrix for the object</param>
        /// <param name="view">view matrix for the object</param>
        /// <param name="proj">projection matrix for the object</param>
        public void Draw(Matrix world, Matrix view, Matrix proj)
        {
            Matrix positionM = Matrix.CreateTranslation(position);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.LightingEnabled = true;
                    effect.World = world * scaleM * positionM;
                    effect.Projection = proj;
                    effect.View = view;
                }
                mesh.Draw();
            }
        }//end draw

        #region PROPERTIES
        public BoundingSphere BoundingSphere
        {
            get { return boundingSphere; }
        }
        public Vector3 Position
        {
            get { return position; }
        }
        #endregion
    }
}
