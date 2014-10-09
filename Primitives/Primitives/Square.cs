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
    class Square
    {
        VertexPositionColor[] vertices = new VertexPositionColor[4];
        short[] indices = new short[6];

        public Square()
        {
            vertices[0].Position = new Vector3(0, 0, 0);
            vertices[0].Color = Color.Red;
            vertices[1].Position = new Vector3(0, 100, 0);
            vertices[1].Color = Color.Orange;
            vertices[2].Position = new Vector3(100, 100, 0);
            vertices[2].Color = Color.Black;
            vertices[3].Position = new Vector3(100, 0, 0);
            vertices[3].Color = Color.Green;

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 0;
            indices[4] = 2;
            indices[5] = 3;
        }

        public void Draw(GraphicsDeviceManager graphics, BasicEffect basicEffect)
        {
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0, 2);

            }
        }
    }
}
