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
    static class BuildingManager
    {
        static Texture2D[] buildingTextures;
        static Texture2D[] roofTextures;
        static Rectangle blockSize;
        static Game theGame;

        public static void Init(Game game, Rectangle bS)
        {
            theGame = game;
            blockSize = bS;
            LoadBuildingTextures();
        }

        public static Building MakeBuilding(int i, Vector2 position)
        {
            Building b = new Building(theGame);

            b.wallTexture = buildingTextures[i - 1];

            Random r = new Random();
            b.roofTexture = roofTextures[r.Next(4)];

            b.Position = new Vector3(position.X * blockSize.Width, 0, position.Y * blockSize.Height);

            b.size = new Rectangle(0, 0, blockSize.Width, blockSize.Height * i);

            b.Init();

            return b;
        }

        static void LoadBuildingTextures()
        {
            ContentManager c = (ContentManager)theGame.Services.GetService(typeof(ContentManager));

            #region Load Building Textures
            int i = 1;
            try
            {
                buildingTextures = new Texture2D[i];
                while (true)
                {
                    if (i == 4) i = 6;  //no building 4 or 5, not a bug, skip 'em
                    Array.Resize<Texture2D>(ref buildingTextures, i);   //make room in the array
                    buildingTextures[i - 1] = c.Load<Texture2D>(String.Format("building{0}", i++));   //load building texture
                }
            }
            catch (ContentLoadException ex)
            {
                Console.WriteLine(String.Format("EXCEPTION CAUGHT: we tried to load building{0}, but there isn't one", i - 1, ex.Message));
            } 
            #endregion

            #region Load Roof Textures
            i = 1;
            try
            {
                roofTextures = new Texture2D[i];
                while (true)
                {
                    Array.Resize<Texture2D>(ref roofTextures, i);   //make room in the array
                    roofTextures[i - 1] = c.Load<Texture2D>(String.Format("roof{0}", i++));   //load building texture
                }
            }
            catch (ContentLoadException ex)
            {
                Console.WriteLine(String.Format("EXCEPTION CAUGHT: we tried to load roof{0}, but there isn't one", i - 1, ex.Message));
            } 
            #endregion
        }
    }//end class

    class Building : GameObject
    {
        public Texture2D wallTexture { get; set; }

        public Texture2D roofTexture { get; set; }

        public Rectangle size { get; set; }

        public float rotation { get; set; }
        Matrix rotationMatrix;

        VertexPositionNormalTexture[] vertices;
        short[] indices;
        int numTriangles;
        int numVertices;

        public Building(Game game) : base(game)
        {
        }

        public override void Init()
        {
            effect = new BasicEffect(graphicsDevice);
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.EnableDefaultLighting();

            numVertices = 20;
            vertices = new VertexPositionNormalTexture[numVertices];

            //UV texture coordinates
            Vector2 textureTopLeft = new Vector2(1, 0);
            Vector2 textureTopRight = new Vector2(0, 0);
            Vector2 textureBottomLeft = new Vector2(1, 1);
            Vector2 textureBottomRight = new Vector2(0, 1);

            //declare vertices
            vertices[0].Position = new Vector3(position.X, 0, position.Z);  //front face bottom left
            vertices[0].TextureCoordinate = textureBottomLeft;

            vertices[1].Position = new Vector3(position.X, size.Height, position.Z);    //front face top left
            vertices[1].TextureCoordinate = textureTopLeft;

            vertices[2].Position = new Vector3(position.X + size.Width, size.Height, position.Z);   //front face top right
            vertices[2].TextureCoordinate = textureTopRight;

            vertices[3].Position = new Vector3(position.X + size.Width, 0, position.Z); //front face bottom right
            vertices[3].TextureCoordinate = textureBottomRight;

            vertices[4].Position = new Vector3(position.X + size.Width, size.Height, position.Z);   //right face top left
            vertices[4].TextureCoordinate = textureTopLeft;

            vertices[5].Position = new Vector3(position.X + size.Width, 0, position.Z); //right face bottom left
            vertices[5].TextureCoordinate = textureBottomLeft;

            vertices[6].Position = new Vector3(position.X + size.Width, size.Height, position.Z + size.Width);  //right face top right
            vertices[6].TextureCoordinate = textureTopRight;

            vertices[7].Position = new Vector3(position.X + size.Width, 0, position.Z + size.Width);    //right face bottom right
            vertices[7].TextureCoordinate = textureBottomRight;

            vertices[8].Position = new Vector3(position.X + size.Width, size.Height, position.Z + size.Width);  //back face top left (or right if looking from front side)
            vertices[8].TextureCoordinate = textureTopLeft;

            vertices[9].Position = new Vector3(position.X + size.Width, 0, position.Z + size.Width);    //back face bottom left
            vertices[9].TextureCoordinate = textureBottomLeft;

            vertices[10].Position = new Vector3(position.X, size.Height, position.Z + size.Width);   //back face top right
            vertices[10].TextureCoordinate = textureTopRight;

            vertices[11].Position = new Vector3(position.X, 0, position.Z + size.Width); //back face bottom right
            vertices[11].TextureCoordinate = textureBottomRight;

            vertices[12].Position = new Vector3(position.X, 0, position.Z);  //left face bottom right
            vertices[12].TextureCoordinate = textureBottomRight;

            vertices[13].Position = new Vector3(position.X, size.Height, position.Z);    //left face top right
            vertices[13].TextureCoordinate = textureTopRight;

            vertices[14].Position = new Vector3(position.X, size.Height, position.Z + size.Width);   //left face top left
            vertices[14].TextureCoordinate = textureTopLeft;

            vertices[15].Position = new Vector3(position.X, 0, position.Z + size.Width); //left face bottom left
            vertices[15].TextureCoordinate = textureBottomLeft;

            vertices[16].Position = new Vector3(position.X, size.Height, position.Z);    //top face bottom left
            vertices[16].TextureCoordinate = textureBottomLeft;

            vertices[17].Position = new Vector3(position.X + size.Width, size.Height, position.Z);   //top face bottom right
            vertices[17].TextureCoordinate = textureBottomRight;

            vertices[18].Position = new Vector3(position.X + size.Width, size.Height, position.Z + size.Width);  //top face top right
            vertices[18].TextureCoordinate = textureTopRight;

            vertices[19].Position = new Vector3(position.X, size.Height, position.Z + size.Width);   //top face top left
            vertices[19].TextureCoordinate = textureTopLeft;


            numTriangles = 10;
            indices = new short[30];

            //declare indices
            indices[0] = 0; //front face
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 0;
            indices[4] = 2;
            indices[5] = 3;

            indices[6] = 5; //right face
            indices[7] = 4;
            indices[8] = 6;
            indices[9] = 5;
            indices[10] = 6;
            indices[11] = 7;

            indices[12] = 9;    //back face
            indices[13] = 8;
            indices[14] = 10;
            indices[15] = 9;
            indices[16] = 10;
            indices[17] = 11;

            indices[18] = 12;    //left face
            indices[19] = 13;
            indices[20] = 14;
            indices[21] = 12;
            indices[22] = 14;
            indices[23] = 15;

            indices[24] = 16;    //top face
            indices[25] = 17;
            indices[26] = 18;
            indices[27] = 16;
            indices[28] = 18;
            indices[29] = 19;

            //indices[6] = 3;
            //indices[7] = 2;
            //indices[8] = 4;
            //indices[9] = 3;
            //indices[10] = 4;
            //indices[11] = 5;

            //indices[12] = 5;
            //indices[13] = 4;
            //indices[14] = 6;
            //indices[15] = 5;
            //indices[16] = 6;
            //indices[17] = 7;

            //indices[18] = 7;
            //indices[19] = 6;
            //indices[20] = 1;
            //indices[21] = 7;
            //indices[22] = 1;
            //indices[23] = 0;

            //bottom of cube
            //indices[24] = 7;
            //indices[25] = 0;
            //indices[26] = 3;
            //indices[27] = 7;
            //indices[28] = 3;
            //indices[29] = 5;

            //top of cube
            //indices[30] = 4;
            //indices[31] = 2;
            //indices[32] = 1;
            //indices[33] = 4;
            //indices[34] = 1;
            //indices[35] = 6;

            CalculateNormals();

            Vector3 centreDisplace = position + new Vector3(size.Width / 2.0f, 0, size.Height / 2.0f);

            rotationMatrix = Matrix.CreateTranslation(-centreDisplace) * Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(centreDisplace);

            //base.Init();
        }

        public override void Draw(Matrix view, Matrix proj)
        {
            effect.View = view;

            effect.Projection = proj;
            effect.World = rotationMatrix;

            //draw the walls
            effect.Texture = wallTexture;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, numVertices, indices, 0, numTriangles - 2);

            }

            //draw the roof
            effect.Texture = roofTexture;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, numVertices, indices, 24, 2);

            }
            //base.Draw(gametime, camera);
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
    }//edn class
}
