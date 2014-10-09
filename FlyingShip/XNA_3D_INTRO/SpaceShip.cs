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
    class SpaceShip
    {
        Model model;
        Vector3 position;
        Quaternion rotation;

        List<Bullet> bulletList;

        const float TurnSpeed = 0.5f;

        public SpaceShip()
        {
            rotation = Quaternion.Identity;
            position = Vector3.Zero;
            bulletList = new List<Bullet>();
        }

        public void LoadContent(ContentManager content)
        {
            model = content.Load<Model>("p1_wedge");
            Bullet.LoadContent(content);
        }

        public void Update(KeyboardState ks)
        {
            float yaw, pitch, roll;
            yaw = pitch = roll = 0;

            #region Keyboard Input (Movement)
            if (ks.IsKeyDown(Keys.D))
            {
                yaw = MathHelper.ToRadians(TurnSpeed);
            }
            else if (ks.IsKeyDown(Keys.A))
            {
                yaw = MathHelper.ToRadians(-TurnSpeed);
            }

            if (ks.IsKeyDown(Keys.W))
            {
                pitch = MathHelper.ToRadians(TurnSpeed);
            }
            else if (ks.IsKeyDown(Keys.S))
            {
                pitch = MathHelper.ToRadians(-TurnSpeed);
            }

            if (ks.IsKeyDown(Keys.E))
            {
                roll = MathHelper.ToRadians(TurnSpeed);
            }
            else if (ks.IsKeyDown(Keys.Q))
            {
                roll = MathHelper.ToRadians(-TurnSpeed);
            }


            float shipSpeed = 0f;

            if (ks.IsKeyDown(Keys.LeftShift))
            {
                shipSpeed = -20f;
            }
            #endregion

            Quaternion rot = Quaternion.CreateFromAxisAngle(Vector3.Right, pitch) * Quaternion.CreateFromAxisAngle(Vector3.Up, yaw) * Quaternion.CreateFromAxisAngle(Vector3.Backward, roll);

            rotation *= rot;

            position += Vector3.Transform(new Vector3(0.0f, 0.0f, shipSpeed), rotation);

            //fire a bullet if space key is pressed
            if (ks.IsKeyDown(Keys.Space))
            {
                bulletList.Add(new Bullet(100f, position, model.Root.Transform.Forward, rotation));
            }

            foreach (Bullet b in bulletList)
            {
                b.Update();
            }
        }//end update

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
                    effect.LightingEnabled = true; // turn on the lighting subsystem.
                    effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
                    effect.EmissiveColor = new Vector3(1, 0, 0);
                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0, 0); // a red light
                    effect.DirectionalLight0.Direction = new Vector3(1, 0, 0);  // coming along the x-axis
                    effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights

                    effect.FogEnabled = true;
                    effect.FogColor = Color.Teal.ToVector3(); // For best results, make this color whatever your background is.  
                    effect.FogStart = 8000.75f;
                    effect.FogEnd = 16000.25f;

                    effect.World = world * shipRot * shipPos;
                    effect.Projection = proj;
                    effect.View = view;
                }
                mesh.Draw();
            }

            foreach (Bullet b in bulletList)
            {
                b.Draw(world, view, proj);
            }
        }//end draw

        public List<Bullet> Bullets
        {
            get { return bulletList; }
        }
    }//end class
}//end namespace
