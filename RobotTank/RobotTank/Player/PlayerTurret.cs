using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RobotTank
{
    class PlayerTurret : BaseGameObject
    {
        #region Variables
        const float TurnSpeed = 0.05f;
        float rotationRadians;

        Vector2 direction, position;
        readonly Vector2 startDirection;    //the starting direction of the turret 
        readonly Vector2 origin;    //the point on the turret that it will rotate about

        const float gunLengthFromOrigin = 75f;

        List<Bullet> bulletList;    //list for any shells/bullets fired
        #endregion

        #region Methods/Constructors
        /// <param name="_startDirection">the forward facing direction of the tank; should be a unit vector</param>
        public PlayerTurret(Vector2 _startDirection)
        {
            startDirection = _startDirection;
            origin = new Vector2(19f, 22f);

            bulletList = new List<Bullet>();
        }

        /// <param name="parentTransform">the transformation matrix of the tank the turret is attached to</param>
        public void Update(Matrix parentTransform, KeyboardState kbs)
        {
            //take our position from parent
            position = new Vector2(parentTransform.Translation.X, parentTransform.Translation.Y);

            #region Input
            if (kbs.IsKeyDown(Keys.Left))
            {
                rotationRadians += TurnSpeed;
            }
            else if (kbs.IsKeyDown(Keys.Right))
            {
                rotationRadians -= TurnSpeed;
            }
            #endregion

            #region Matrices
            //apply the rotation float variable to a rotation matrix
            Matrix.CreateRotationZ(rotationRadians, out rotateM);

            //find out which direction the tank is facing
            direction = Vector2.Transform(startDirection, rotateM);

            //apply the position vector to a translation matrix
            translateM = Matrix.CreateTranslation(new Vector3(position, 0f));

            base.Update();

            #endregion

            #region Bullets
            if (kbs.IsKeyDown(Keys.Space))
            {
                //long line of code: basically the same thing as done in bullet constructor for direction. all this does is put the bullet at the end of the barrel
                Vector2 bulletSpawnPos = new Vector2(gunLengthFromOrigin * (float)Math.Cos(rotationRadians), gunLengthFromOrigin * (float)Math.Sin(rotationRadians));
                bulletSpawnPos += position;

                bulletList.Add(new Bullet(bulletSpawnPos, 2f, rotationRadians));
            }
            foreach (Bullet b in bulletList)
            {
                b.Update();
            } 
            #endregion
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transformM);

            //draw myself
            spriteBatch.Draw(texture, Vector2.Zero, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);

            spriteBatch.End();

            //draw bullets without using the matrix in this class
            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
