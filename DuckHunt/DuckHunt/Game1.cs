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
using GameLibrary_C00165681;    //custom class library i made in first year

namespace DuckHunt
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //dimensions of game window
        const short Width = 800, Height = 480;
        const short maxDucks = 100;
        const short Border = 100; //the distance from the edge that its safe to spawn ducks

        //string to tell player how many ducks are remaining
        string ducksLeft;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        List<Duck> duckList;
        Crosshair crosshair;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //set the size of our game window
            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            crosshair = new Crosshair();

            //create the empty list of ducks
            duckList = new List<Duck>();

            Random r = new Random();

            //populate the list
            for (int i = 0; i < maxDucks; i++)
            {                                       //randomize both location and direction (direction will be normalized in duck constructor)
                duckList.Add(new Duck(new Vector2(r.Next(Border, Width - Border), r.Next(Border, Height - Border)), new Vector2(r.Next(Border, Width - Border), r.Next(Border, Height - Border))));
            }

            //the {0} will be replaced with number of ducks in the list when we tell it to draw
            ducksLeft = "{0} Ducks Remaining";

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

            //create a new spritefont for printing strings to screen
            spriteFont = Content.Load<SpriteFont>("myFont");

            //the duck texture is static so we only need to load it once (not seperately for each duck)
            Duck.LoadContent(Content, "duck");

            crosshair.LoadContent(Content, "crosshairs");
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

            crosshair.Update(Mouse.GetState());

            foreach (Duck d in duckList)
            {
                d.Update(Width, Height);
            }

            for (int i = 0; i < duckList.Count; i++)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    //check  collision and remove as necessary
                    //stop using own class lib --> if (Collision.PerPixel(crosshair.Texture, crosshair.TranslationMatrix, duckList[i].Texture, duckList[i].Transform))
                        duckList.RemoveAt(i);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (Duck d in duckList)
            {
                d.Draw(spriteBatch);    //ducks begin and end spritebatch within their draw method
            }

            crosshair.Draw(spriteBatch);    //crosshairs begin and end within

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, String.Format(ducksLeft, duckList.Count), new Vector2(10), Color.White); //draw text
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    /// <summary>
    /// brief rewrite of GameLibrary_C00165681.Collision class
    /// </summary>
    /// <remarks>see GameLibrary_C00165681.dll in object explorer for details</remarks>
    class Collision
    {
        public bool PerPixel(Matrix transformA, Texture2D textureA, Matrix transformB, Texture2D textureB)
        {
            Color[] dataA, dataB;
            bool collision = false;

            GetData(textureA, out dataA);
            GetData(textureB, out dataB);

            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            //for each row of pixels in A
            for (int yA = 0; yA < textureA.Height; yA++)
            {
                //start at the beginning of the row
                Vector2 posInB = yPosInB;

                //for each pixel in the row
                for (int xA = 0; xA < textureA.Width; xA++)
                {
                    //round to nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    //if the pixel lies within the bounds of B
                    if (0 <= xB && xB < textureB.Width &&
                        0 <= yB && yB < textureB.Height)
                    {
                        //get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        //if both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            collision =  true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            return collision;
        }

        private void GetData(Texture2D texture, out Color[] data)
        {
            data = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(data);
        }
    }
}
