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
    class Cube
    {
        VertexPositionColorNormal[] vertices = new VertexPositionColorNormal[8];
        short[] indices = new short[36];

        public Cube()
        {
            vertices[0].Position = new Vector3(0, 0, 0);
            vertices[0].Color = Color.Gray;
            vertices[1].Position = new Vector3(0, 100, 0);
            vertices[1].Color = Color.Gray;
            vertices[2].Position = new Vector3(100, 100, 0);
            vertices[2].Color = Color.Gray;
            vertices[3].Position = new Vector3(100, 0, 0);
            vertices[3].Color = Color.Gray;
            vertices[4].Position = new Vector3(100, 100, -100);
            vertices[4].Color = Color.Gray;
            vertices[5].Position = new Vector3(100, 0, -100);
            vertices[5].Color = Color.Gray;
            vertices[6].Position = new Vector3(0, 100, -100);
            vertices[6].Color = Color.Gray;
            vertices[7].Position = new Vector3(0, 0, -100);
            vertices[7].Color = Color.Gray;

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 0;
            indices[4] = 2;
            indices[5] = 3;

            indices[6] = 3;
            indices[7] = 2;
            indices[8] = 4;
            indices[9] = 3;
            indices[10] = 4;
            indices[11] = 5;

            indices[12] = 5;
            indices[13] = 4;
            indices[14] = 6;
            indices[15] = 5;
            indices[16] = 6;
            indices[17] = 7;

            indices[18] = 7;
            indices[19] = 6;
            indices[20] = 1;
            indices[21] = 7;
            indices[22] = 1;
            indices[23] = 0;

            indices[24] = 7;
            indices[25] = 0;
            indices[26] = 3;
            indices[27] = 7;
            indices[28] = 3;
            indices[29] = 5;

            indices[30] = 4;
            indices[31] = 2;
            indices[32] = 1;
            indices[33] = 4;
            indices[34] = 1;
            indices[35] = 6;

            CalculateNormals();
        }

        public void Draw(GraphicsDeviceManager graphics, BasicEffect basicEffect)
        {
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 8, indices, 0, 12, VertexPositionColorNormal.VertexDeclaration);

            }
        }

        private void CalculateNormals()
        {
            //clear the normals on each of the vertices
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal = new Vector3(0, 0, 0);

            //calculate the normal of each of the triangles and it it to the normal of each of the triangles vertices
            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                //get two sides of the triangle
                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);   //cross product gives us the normal

                vertices[index1].Normal += normal;  //add the traingle's normal to each of it's vertices
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;
            }

            //right now, all the normals are HUGE.
            for (int i = 0; i < vertices.Length; i++)
            vertices[i].Normal.Normalize(); //we want unit vectors, so we normaize them
        }
    }
}
