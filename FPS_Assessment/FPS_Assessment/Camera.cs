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
    interface ICamera
    {
        Matrix View { get; }
        Matrix Projection { get; }
    }

    class ChaseCam : ICamera
    {
        Matrix lookAt, proj;

        float fieldOfView;
        float aspectRatio;
        float near;
        float far;

        public ChaseCam(float fieldOfView, float aspectRatio, float near, float far)
        {
            //translate = Matrix.Identity;  //unnecessary
            this.fieldOfView = fieldOfView;
            this.aspectRatio = aspectRatio;
            this.near = near;
            this.far = far;
        }

        public void Update(Vector3 target, Vector3 targetForward)
        {
            Vector3 pos = target + targetForward * 15;
            pos.Y += 5;
            target.Y += 4;
            lookAt = Matrix.CreateLookAt(pos, target, Vector3.Up);
            proj = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, near, far);
        }

        Matrix ICamera.View
        {
            get { return lookAt; }
            //no set
        }

        Matrix ICamera.Projection
        {
            get { return proj; }
            //no set
        }
    }

    class OrthoCam : ICamera
    {
        Matrix lookAt, proj;
        float near;
        float far;

        public OrthoCam(float near, float far)
        {
            this.near = near;
            this.far = far;

            lookAt = Matrix.CreateLookAt(new Vector3(75, 250, 75), new Vector3(75, 0, 75), Vector3.Forward);
        }

        public void Update()
        {
            proj = Matrix.CreateOrthographic(300, 150, near, far);
        }

        Matrix ICamera.View
        {
            get { return lookAt; }
            //no set
        }

        Matrix ICamera.Projection
        {
            get { return proj; }
            //no set
        }
    }

    class FPS_Cam : ICamera
    {
        Matrix lookAt, proj;

        float fieldOfView;
        float aspectRatio;
        float near;
        float far;

        public FPS_Cam(float fieldOfView, float aspectRatio, float near, float far)
        {
            //translate = Matrix.Identity;  //unnecessary
            this.fieldOfView = fieldOfView;
            this.aspectRatio = aspectRatio;
            this.near = near;
            this.far = far;
        }

        public void Update(Vector3 target, Vector3 targetForward)
        {
            lookAt = Matrix.CreateLookAt(target+targetForward, target, Vector3.Up);
            proj = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, near, far);
        }

        Matrix ICamera.View
        {
            get { return lookAt; }
            //no set
        }

        Matrix ICamera.Projection
        {
            get { return proj; }
            //no set
        }
    }
}
