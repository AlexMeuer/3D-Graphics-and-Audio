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
    class Camera : BaseGameObject
    {
        Viewport viewport;
        Vector2 centre;
        Vector2 bounds;
        float scale;

        public Camera(Viewport vP, float arenaWidth, float arenaHeight)
        {
            viewport = vP;

            bounds = new Vector2(arenaWidth - viewport.Width / 2f, arenaHeight - viewport.Height / 2f);

            scale = 1f;
        }

        /// <param name="centreOnThis">vector to try centre the camera on</param>
        public void Update(Vector3 centreOnThis)
        {
#if DEBUG
            //allow the camera to zoom
            KeyboardState kbs = Keyboard.GetState();
            if(kbs.IsKeyDown(Keys.Z))
                scale += 0.01f;
            else if(kbs.IsKeyDown(Keys.X))
                scale -= 0.01f;
#endif
            //dont allow camera to leave game area
            centreOnThis.X = MathHelper.Clamp(centreOnThis.X, -bounds.X / 2f, bounds.X /2f);
            centreOnThis.Y = MathHelper.Clamp(centreOnThis.Y, -bounds.Y /2f , bounds.Y /2f);

            centre = new Vector2(-centreOnThis.X - viewport.X / 2f, -centreOnThis.Y - viewport.Y / 2f);

            cameraM = Matrix.CreateTranslation(new Vector3(centre.X, centre.Y, 0f)) *
                      Matrix.CreateScale(scale) *
                      Matrix.CreateTranslation(new Vector3(viewport.Width / 2f, viewport.Height / 2f, 0f));
        }
        /// <param name="centreOnThis">vector to try centre the camera on</param>
        public void Update(Vector2 centreOnThis)
        {
            //avoid duplicate code, call the existing method
            this.Update(new Vector3(centreOnThis, 0f));
        }

        public Matrix Matrix
        {
            get { return cameraM; }
            //no set
        }
    }
}
