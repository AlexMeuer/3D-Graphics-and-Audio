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
    class Robot
    {
        Texture2D bodyTexture;
        RobotPart lowerArm, upperArm;
        Matrix bodyTranslation;

        public Robot()
        {
            lowerArm = new RobotPart(new Vector2(425f,210f), new Vector2(25f,400f), 0.8f);
            upperArm = new RobotPart(new Vector2(375f, 85f), new Vector2(30f, 90f), 1.2f);
        }

        public void LoadContent(ContentManager Content, Viewport viewport)
        {
            bodyTexture = Content.Load<Texture2D>("body");
            lowerArm.LoadContent(Content, "lowerarm");
            upperArm.LoadContent(Content, "upperarm");

            //this cannot be done in constructor as the bodyTexture is null at that point
            bodyTranslation = Matrix.CreateTranslation(new Vector3(0f, viewport.Height - bodyTexture.Height, 0f));
        }

        public void Update(KeyboardState keyBoardState)
        {
            //M41 corresponds to the x
            if(keyBoardState.IsKeyDown(Keys.Right))
                bodyTranslation.M41++;

            else if (keyBoardState.IsKeyDown(Keys.Left))
                bodyTranslation.M41--;

            lowerArm.Update(keyBoardState, Keys.X, Keys.Z, bodyTranslation);
            upperArm.Update(keyBoardState, Keys.C, Keys.V, lowerArm.Transformation);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, bodyTranslation);
            spriteBatch.Draw(bodyTexture, Vector2.Zero, Color.White);
            spriteBatch.End();

            lowerArm.Draw(spriteBatch);
            upperArm.Draw(spriteBatch);
        }
    }
}
