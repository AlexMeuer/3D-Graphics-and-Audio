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

namespace Primitives
{
    class Camera3D
    {
        #region Properties
        Vector3 cameraPosition = new Vector3(0, 0, 2);
        float leftRightRotation = MathHelper.PiOver2;
        float upDownRotation = -MathHelper.Pi / 10.0f;
        const float rotationSpeed = 0.03f;
        const float moveSpeed = 3.0f;
        public Matrix viewMatrix;
        public MouseState originalMouseState;
        GraphicsDeviceManager device;
        public Camera3D(GraphicsDeviceManager _device)
        {
            device = _device;
            //viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 2), Vector3.Zero,
            //  Vector3.Up);
        }

        public void UpdateViewMatrix()
        {
            Matrix cameraRotation =
                Matrix.CreateRotationX(upDownRotation) * Matrix.CreateRotationY(leftRightRotation);
            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = cameraPosition + cameraRotatedTarget;

            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);
            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraFinalTarget, cameraRotatedUpVector);
        }

        public void ProcessInput(float amount)
        {

            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState != originalMouseState)
            {
                float xDifference = currentMouseState.X - originalMouseState.X;
                float yDifference = currentMouseState.Y - originalMouseState.Y;
                leftRightRotation -= rotationSpeed * xDifference * amount;
                upDownRotation -= rotationSpeed * yDifference * amount;
                Mouse.SetPosition(device.GraphicsDevice.Viewport.Width / 2, device.GraphicsDevice.Viewport.Height / 2);
                UpdateViewMatrix();
            }
            Vector3 moveVector = new Vector3(0, 0, 0);
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                moveVector += new Vector3(0, 0, -1);
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                moveVector += new Vector3(0, 0, 1);
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
                moveVector += new Vector3(1, 0, 0);
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
                moveVector += new Vector3(-1, 0, 0);
            if (keyState.IsKeyDown(Keys.Q))
                moveVector += new Vector3(0, 1, 0);
            if (keyState.IsKeyDown(Keys.Z))
                moveVector += new Vector3(0, -1, 0);
            AddToCameraPosition(moveVector * amount * 16);
        }

        public void AddToCameraPosition(Vector3 vectorToAdd)
        {
            Matrix cameraRotation = Matrix.CreateRotationX(upDownRotation) * Matrix.CreateRotationY(leftRightRotation);
            Vector3 rotatedVector = Vector3.Transform(vectorToAdd, cameraRotation);
            cameraPosition += moveSpeed * rotatedVector;
            UpdateViewMatrix();
        }
        #endregion
    }
}