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
    class Bullet : BaseGameObject
    {
        static Texture2D staticTexture;
        readonly Vector2 direction; //dont want bullet to change direction after constructor
        //Vector2 position;
        float speed;

        public Bullet(Vector2 startPos, float _speed, float parentRotation)
        {
            direction = new Vector2((float)Math.Cos(parentRotation), (float)Math.Sin(parentRotation));
            //turn that into a unit vector
            //direction.Normalize();    redundant now that we're using cos and sin

            speed = _speed;

            rotateM = Matrix.CreateRotationZ(parentRotation);
            translateM = Matrix.CreateTranslation(new Vector3(startPos, 0f));
        }

        public static void LoadContent(ContentManager content)
        {
            staticTexture = content.Load<Texture2D>("tank_shell");
        }

        public override void Update()
        {
            translateM *= Matrix.CreateTranslation(new Vector3(direction * speed, 0));
            base.Update();
        }

        //cannot draw using base class because we're using a static texture
        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transformM);
            spriteBatch.Draw(staticTexture, Vector2.Zero, null, Color.White);
            spriteBatch.End();
        }
    }
}
