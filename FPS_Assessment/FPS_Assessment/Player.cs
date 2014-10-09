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

namespace FPS_Assessment
{
    class Player
    {
        Model model;
        Vector3 position;//, direction;
        Matrix translateM, rotateM;
        float moveSpeed, turnSpeed, turretSpeed;

        public Player(Vector3 startingPosition/*, Vector3 startingDirection*/, float _moveSpeed, float _turnSpeed, float _turretSpeed)
        {
            position = startingPosition;
            //direction = startingDirection;
            moveSpeed = _moveSpeed;
            turnSpeed = _turnSpeed;
            turretSpeed = _turretSpeed;

            translateM = rotateM = Matrix.Identity;
        }

        public void LoadContent(ContentManager content)
        {
            model = content.Load<Model>("tank");
        }

        public void Update(KeyboardState kbs)
        {
            #region Moving And Turning
            if (kbs.IsKeyDown(Keys.A))
            {
                rotateM *= Matrix.CreateRotationY(turnSpeed);
                model.Bones[2].Transform = Matrix.CreateRotationX(moveSpeed) * model.Bones[2].Transform;
                model.Bones[4].Transform = Matrix.CreateRotationX(moveSpeed) * model.Bones[4].Transform;
                model.Bones[6].Transform = Matrix.CreateRotationX(-moveSpeed) * model.Bones[6].Transform;
                model.Bones[8].Transform = Matrix.CreateRotationX(-moveSpeed) * model.Bones[8].Transform;
            }

            if (kbs.IsKeyDown(Keys.D))
            {
                rotateM *= Matrix.CreateRotationY(-turnSpeed);
                model.Bones[2].Transform = Matrix.CreateRotationX(-moveSpeed) * model.Bones[2].Transform;
                model.Bones[4].Transform = Matrix.CreateRotationX(-moveSpeed) * model.Bones[4].Transform;
                model.Bones[6].Transform = Matrix.CreateRotationX(moveSpeed) * model.Bones[6].Transform;
                model.Bones[8].Transform = Matrix.CreateRotationX(moveSpeed) * model.Bones[8].Transform;
            }

            if (kbs.IsKeyDown(Keys.W))
            {
                translateM *= Matrix.CreateTranslation(model.Root.Transform.Forward * -moveSpeed);
                model.Bones[2].Transform = Matrix.CreateRotationX(moveSpeed) * model.Bones[2].Transform;
                model.Bones[4].Transform = Matrix.CreateRotationX(moveSpeed) * model.Bones[4].Transform;
                model.Bones[6].Transform = Matrix.CreateRotationX(moveSpeed) * model.Bones[6].Transform;
                model.Bones[8].Transform = Matrix.CreateRotationX(moveSpeed) * model.Bones[8].Transform;

            }

            if (kbs.IsKeyDown(Keys.S))
            {
                translateM *= Matrix.CreateTranslation(model.Root.Transform.Backward * -moveSpeed);
                model.Bones[2].Transform = Matrix.CreateRotationX(-moveSpeed) * model.Bones[2].Transform;
                model.Bones[4].Transform = Matrix.CreateRotationX(-moveSpeed) * model.Bones[4].Transform;
                model.Bones[6].Transform = Matrix.CreateRotationX(-moveSpeed) * model.Bones[6].Transform;
                model.Bones[8].Transform = Matrix.CreateRotationX(-moveSpeed) * model.Bones[8].Transform;
            } 
            #endregion

            if (kbs.IsKeyDown(Keys.E))
                model.Bones["turret_geo"].Transform *= Matrix.CreateRotationY(turretSpeed);

            if (kbs.IsKeyDown(Keys.Q))
                model.Bones["turret_geo"].Transform *= Matrix.CreateRotationY(-turretSpeed);

            model.Root.Transform = rotateM * translateM;
        }

        public void Draw(Matrix view, Matrix proj)
        {

            Matrix[] transforms = new Matrix[model.Bones.Count];

            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index];
                    effect.View = view;
                    effect.Projection = proj;
                }

                mesh.Draw();
            }
        }

        public Matrix RootTransform
        {
            get { return model.Root.Transform; }
            //no set
        }

        public Matrix TurretTransform
        {
            get { return model.Bones["turret_geo"].Transform; }
            //no set
        }

        public Matrix AbsoluteTurretTransform
        {
            get { return model.Bones["turret_geo"].Transform * model.Root.Transform; }
            //no set
        }
    }
}
