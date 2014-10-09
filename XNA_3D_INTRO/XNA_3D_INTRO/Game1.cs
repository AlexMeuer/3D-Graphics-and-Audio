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

namespace XNA_3D_INTRO
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const float TurnSpeed = 0.5f;

        Matrix view, world, proj;

        //ship variables
        Model ship;
        Vector3 shipPosition;
        Quaternion shipRotation;

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
            shipRotation = Quaternion.Identity;
            shipPosition = Vector3.Zero;

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
            ship = Content.Load<Model>("p1_wedge");
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

            float yaw, pitch, roll;
            yaw = pitch = roll = 0;

            if(ks.IsKeyDown(Keys.D))
            {
                yaw = MathHelper.ToRadians(TurnSpeed);
            }
            else if(ks.IsKeyDown(Keys.A))
            {
                yaw = MathHelper.ToRadians(-TurnSpeed);
            }

            if (ks.IsKeyDown(Keys.W))
            {
                pitch = MathHelper.ToRadians(TurnSpeed);
            }
            else if(ks.IsKeyDown(Keys.S))
            {
                pitch = MathHelper.ToRadians(-TurnSpeed);
            }

            if (ks.IsKeyDown(Keys.E))
            {
                roll = MathHelper.ToRadians(TurnSpeed);
            }
            else if (ks.IsKeyDown(Keys.Q))
            {
                roll = MathHelper.ToRadians(-TurnSpeed);
            }


            float shipSpeed = 0f;

            if(ks.IsKeyDown(Keys.Space))
            {
                shipSpeed = -20f;
            }

              Quaternion rot = Quaternion.CreateFromAxisAngle(Vector3.Right, pitch) * Quaternion.CreateFromAxisAngle(Vector3.Up, yaw) * Quaternion.CreateFromAxisAngle(Vector3.Backward, roll);
           
            shipRotation *= rot;

            shipPosition += Vector3.Transform(new Vector3(0.0f, 0.0f, shipSpeed), Matrix.CreateFromQuaternion(shipRotation));

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //set the matrices
            view = Matrix.CreateLookAt(new Vector3(0, 10, 5000), Vector3.Zero, Vector3.Up);

            world = Matrix.CreateTranslation(Vector3.Zero);

            proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height, 0.1f, 100000);

            //create ship-specific matrices
            Matrix shipPos = Matrix.CreateTranslation(shipPosition);
            Matrix shipRot = Matrix.CreateFromQuaternion(shipRotation);

            foreach (ModelMesh mesh in ship.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.LightingEnabled = false;
                    effect.World = world * shipRot * shipPos;
                    effect.Projection = proj;
                    effect.View = view;
                }
                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
