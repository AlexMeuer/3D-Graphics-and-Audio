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

namespace SolarSystem
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        OrbitalObject sun, earth, moon, mars;
        List<OrbitalObject> marsMoons;

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
            marsMoons = new List<OrbitalObject>();

            //Viewport viewport = graphics.GraphicsDevice.Viewport;
            sun = new OrbitalObject(0.0f, 0.0f, 0.0f);
            earth = new OrbitalObject(0.01f, 100.0f, 3.65f);
            moon = new OrbitalObject(-0.1f, 50.0f, 0.0f);
            mars = new OrbitalObject(-0.015f, 250.0f, -0.05f);
            marsMoons.Add(new OrbitalObject(0.03f, 50.0f, 0.1f));
            marsMoons.Add(new OrbitalObject(-0.05f, 70.0f, -0.2f));

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

            Viewport viewport = graphics.GraphicsDevice.Viewport;
            Vector3 screenCentre = new Vector3(viewport.Width / 2.0f, viewport.Height / 2.0f, 0);
            sun.LoadContent(Content, "sun", screenCentre);

            earth.LoadContent(Content, "earth");
            moon.LoadContent(Content, "moon");
            mars.LoadContent(Content, "mars");

            foreach (OrbitalObject m in marsMoons)
                m.LoadContent(Content, "moon");
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

            sun.Update();
            earth.Update(sun.TranslationMatrix);
            moon.Update(earth.TranslationMatrix);
            mars.Update(sun.TranslationMatrix);

            foreach (OrbitalObject m in marsMoons)
                m.Update(mars.TranslationMatrix);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            sun.Draw(spriteBatch, Color.Orange);
            earth.Draw(spriteBatch, Color.Blue);
            moon.Draw(spriteBatch, Color.White);
            mars.Draw(spriteBatch, Color.DarkRed);

            foreach (OrbitalObject m in marsMoons)
                m.Draw(spriteBatch, Color.Gray);

            base.Draw(gameTime);
        }
    }
}
