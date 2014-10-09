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

namespace FlyingShip
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D backgroundImage;

        //const float TurnSpeed = 0.5f;

        Matrix view, world, proj;

        SpaceShip myShip;
        //ship variables
        //Model ship;
        //Vector3 shipPosition;
        //Quaternion shipRotation;
        Enemy[] eArray;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //shipRotation = Quaternion.Identity;
            //shipPosition = Vector3.Zero;
            myShip = new SpaceShip();

            eArray = new Enemy[20];
            Random gen = new Random();

            for (int i = 0; i < eArray.Length; i++)
            {
                //randomize enemy placement
                eArray[i] = new Enemy(new Vector3(gen.Next(-32000, 32000), 0f, gen.Next(-20000, 20000)));
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load the model for the ship
            //ship = Content.Load<Model>("p1_wedge");
            myShip.LoadContent(Content);

            foreach (Enemy e in eArray)
            {
                e.LoadContent(Content);
            }

            backgroundImage = Content.Load<Texture2D>("space");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState ks = Keyboard.GetState();

            myShip.Update(ks);

            for(int i = 0; i < eArray.Length; i++)
            {
                if (eArray[i] != null)
                {
                    eArray[i].Update();

                    for(int j = 0; j < myShip.Bullets.Count; j++)
                    {
                        //if collision occurs...
                        if (CheckCollision(eArray[i], myShip.Bullets[j]))
                        {
                            //...destroy enemy and bullet and break out of loop
                            eArray[i] = null;
                            myShip.Bullets.RemoveAt(j);
                            break;
                        }//end if
                    }//end for
                }//end if
            }//end for

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //draw the background
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundImage, Vector2.Zero, Color.White);
            spriteBatch.End();

            //set the matrices
            view = Matrix.CreateLookAt(new Vector3(0, 5000, 5000), Vector3.Zero, Vector3.Up);

            world = Matrix.CreateTranslation(Vector3.Zero);

            proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height, 0.1f, 100000);


            myShip.Draw(world, view, proj);

            foreach (Enemy e in eArray)
            {
                if(e != null)
                    e.Draw(world, view, proj);
            }
            ////create ship-specific matrices
            //Matrix shipPos = Matrix.CreateFromQuaternion(shipRotation);
            //Matrix shipRot = Matrix.CreateTranslation(shipPosition);

            //foreach (ModelMesh mesh in ship.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.LightingEnabled = false;
            //        effect.World = world * shipRot * shipPos;
            //        effect.Projection = proj;
            //        effect.View = view;
            //    }
            //    mesh.Draw();
            //}

            base.Draw(gameTime);
        }

        private bool CheckCollision(Enemy e, Bullet b)
        {
            bool collision = false;

            //if diistance between them is small
            if(GetVector3Distance(e.Position, b.Position) <= e.BoundingSphere.Radius*500)
            {
                //if their bounding spheres intersect
                if(e.BoundingSphere.Intersects(b.BoundingSphere))
                {
                    collision = true;
                }
            }

            return collision;
        }

        private double GetVector3Distance(Vector3 a, Vector3 b)
        {
            double X, Y, Z;

            X = Math.Pow(a.X - b.X, 2);

            Y = Math.Pow(a.Y - b.Y, 2);

            Z = Math.Pow(a.Z - b.Z, 2);

            return Math.Sqrt(X + Y + Z);
        }
    }
}
