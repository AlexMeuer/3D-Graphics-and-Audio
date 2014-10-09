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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PlayerTank playerTank;
        Camera camera;

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
            playerTank = new PlayerTank(100f, 3);   //TODO: have these values scale off difficulty

            //camera = new Camera(graphics.GraphicsDevice.Viewport, Background.Texture.Width, Background.Texture.Height);   //cannot reference null texture

            GUI.Initialize();

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

            //load static content
            Bullet.LoadContent(Content);
            Background.Texture = Content.Load<Texture2D>("terrain");

            //load content for objects
            playerTank.LoadContent(Content, "tank_body");

            //initialize objects that require loaded content in constructor(s)
            camera = new Camera(graphics.GraphicsDevice.Viewport, Background.Texture.Width, Background.Texture.Height);

            GUI.LoadContent(Content);
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

            Background.Update(camera.Matrix);

            playerTank.Update(Keyboard.GetState());

            camera.Update(playerTank.Position);

            //tell the gui to update and pass it the game and player
            GUI.Update(this, playerTank);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Background.Draw(spriteBatch);

            playerTank.Draw(spriteBatch);

            GUI.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }//end Game1 class

    struct Background
    {
        static Texture2D texture;
        static Matrix transform = Matrix.Identity;
        static Vector2 offset;

        public static void Update(Matrix cameraMatrix)
        {
            transform = cameraMatrix;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transform);
            spriteBatch.Draw(texture, offset, Color.White);
            spriteBatch.End();
        }

        public static Texture2D Texture
        {
            get { return texture;}
            set {
                    texture = value;
                    offset = new Vector2(texture.Width / -2f, texture.Height / -2f);
                }
        }
    }//end Background struct
}//end RobotTank namespace
