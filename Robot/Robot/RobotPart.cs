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

namespace Robot
{
    class RobotPart
    {
        #region VARIABLES
        Texture2D texture;
        Vector2 joinPoint, origin;
        Matrix transformation, rotation, joinPointAdjust, originAdjust;
        float angleRadians, rotationThreshold;
        #endregion

        #region METHODS/CONSTRUCTORS
        /// <param name="_joinPoint">the point to join to on the other object</param>
        /// <param name="_origin">the point (on this object) to rotate about</param>
        /// <param name="_rotationThreshold">the maximum rotation (in radians) allowed in either direction</param>
        public RobotPart(Vector2 _joinPoint, Vector2 _origin, float _rotationThreshold)
        {
            joinPoint = _joinPoint;
            origin = _origin;
            //transformation = Matrix.Identity;
            //rotation = Matrix.Identity;
            angleRadians = 0f;
            rotationThreshold = _rotationThreshold;
        }

        public void LoadContent(ContentManager Content, string assetName)
        {
            texture = Content.Load<Texture2D>(assetName);
        }

        /// <param name="keyBoardState">current keyboard state</param>
        /// <param name="rotatePositive">if this key is down, part will rotate clockwise</param>
        /// <param name="rotateNegative">if this key is down, part will roate counterclockwise</param>
        public void Update(KeyboardState keyBoardState, Keys rotatePositive, Keys rotateNegative, Matrix relativeTranslation)
        {
            #region RotationInput
            if (keyBoardState.IsKeyDown(rotatePositive) && angleRadians < rotationThreshold)
                angleRadians += 0.01f;
            else if (keyBoardState.IsKeyDown(rotateNegative) && angleRadians > -rotationThreshold)
                angleRadians -= 0.01f;

            Matrix.CreateRotationZ(angleRadians, out rotation);
            #endregion

            #region Adjustments for correct rotation etc.
            /*Matrix yAdjust = Matrix.CreateTranslation(new Vector3(0f, joinPoint.Y, 0f));
            Matrix xAdjust = Matrix.CreateTranslation(new Vector3(joinPoint.X, 0f, 0f));*/
            joinPointAdjust = Matrix.CreateTranslation(new Vector3(joinPoint, 0f));
            originAdjust = Matrix.CreateTranslation(new Vector3(-origin.X, -origin.Y, 0f));
            #endregion

            /* move origin to (0,0),
             * then do rotation,
             * adjust for where we attach to robot body,
             * then match match robot coordinates
             */
            transformation = originAdjust * rotation * joinPointAdjust * relativeTranslation;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transformation);
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            spriteBatch.End();
        } 
        #endregion

        #region PROPERTIES
        public Matrix Transformation
        {
            get { return transformation; }
            set { transformation = value; }
        } 
        #endregion
    }
}

