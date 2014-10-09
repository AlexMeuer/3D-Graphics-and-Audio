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
    class Enemy
    {
        Model model;
        Vector3 position;
        Quaternion rotation;
        Matrix scaleM;
        BoundingSphere boundingSphere;

        const float TurnSpeed = 0.5f;

        public Enemy(Vector3 startPosition, float scale = 500f)
        {
            position = startPosition;
            rotation = Quaternion.Identity;
            scaleM = Matrix.CreateScale(scale);
        }

        public void LoadContent(ContentManager content)
        {
            model = content.Load<Model>("Cube");
            boundingSphere = model.Meshes[0].BoundingSphere;
            boundingSphere.Radius *= 500f;
        }

        public void Update()
        {
            Quaternion rot = Quaternion.CreateFromAxisAngle(Vector3.Right, 0f) * Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(TurnSpeed)) * Quaternion.CreateFromAxisAngle(Vector3.Backward, 0f);

            rotation *= rot;

            position += Vector3.Transform(new Vector3(0.0f, 0.0f, 2f), Matrix.CreateFromQuaternion(rotation));

            boundingSphere.Center = position;
        }

        /// <param name="world">world matrix for the ship</param>
        /// <param name="view">view matrix for the ship</param>
        /// <param name="proj">projection matrix for the ship</param>
        public void Draw(Matrix world, Matrix view, Matrix proj)
        {
            //create ship-specific matrices
            Matrix shipPos = Matrix.CreateTranslation(position);
            Matrix shipRot = Matrix.CreateFromQuaternion(rotation);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.LightingEnabled = true;
                    effect.World = world * scaleM * shipRot * shipPos;
                    effect.Projection = proj;
                    effect.View = view;
                }
                mesh.Draw();
            }
        }

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
