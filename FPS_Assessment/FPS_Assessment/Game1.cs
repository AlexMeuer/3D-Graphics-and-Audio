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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;
        

        String[] map ={  
                    "╬═╦═╦════╗",
                    "║1║3║1111║",
                    "║2║2╠════╣",
                    "╠═╩═╣7777║",
                    "║666║7777║",
                    "╠═╦═╬════╣",
                    "║3║8║1111║",
                    "║3╚═╬═╗66║",
                    "║323╠═╝88║",
                    "╚═══╩════╝"          
        };

        List<Building> buildings;
        List<Street> streets;
        Rectangle blockSize;

        //Cameras
        ICamera activeCam;
        ChaseCam chaseCam;
        OrthoCam orthoCam;
        FPS_Cam fpsCam;
        


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Services.AddService(typeof(ContentManager), Content);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            blockSize = new Rectangle(0, 0, 15, 15);

            player = new Player(Vector3.Zero, 0.5f, 0.05f, 0.08f);

            streets = new List<Street>();
            buildings = new List<Building>();

            StreetFactory.Init(this, blockSize);
            BuildingManager.Init(this, blockSize);

            for (int z = 0; z < map.Length; z++)
            {
                for (int x = 0; x < map[z].Length; x++)
                {
                    char c = map[z][x];
                    if (StreetFactory.streetSymbols.Contains(c))    //if c is a street symbol...
                    {
                        Street s = StreetFactory.makeStreet(map[z][x], new Vector2(x, z));  //...make a new street
                        streets.Add(s);
                    }
                    else
                    {
                        int i;
                        if(int.TryParse(c.ToString(), out i))   //if c can be parsed into an int...
                        {
                            Building b = BuildingManager.MakeBuilding(i, new Vector2(x, z));    //...make a new building
                            buildings.Add(b);
                        }
                    }
                }
            }


            chaseCam = new ChaseCam(0.6f, graphics.GraphicsDevice.Viewport.AspectRatio, 1, 1000);
            orthoCam = new OrthoCam(1, 1000);
            fpsCam = new FPS_Cam(0.6f, graphics.GraphicsDevice.Viewport.AspectRatio, 0.5f, 1000);
            activeCam = orthoCam;

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

            player.LoadContent(Content);

            // TODO: use this.Content to load your game content here
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

            // TODO: Add your update logic here
            player.Update(Keyboard.GetState());

            if (activeCam == chaseCam)
                chaseCam.Update(player.RootTransform.Translation, player.AbsoluteTurretTransform.Forward);
            else if (activeCam == orthoCam)
                orthoCam.Update();
            else
                fpsCam.Update(player.AbsoluteTurretTransform.Translation, player.AbsoluteTurretTransform.Forward);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp; // need to do this on reach devices to allow non 2^n textures
            RasterizerState rs = RasterizerState.CullNone;
            
            GraphicsDevice.RasterizerState = rs;
            // TODO: Add your drawing code here

            player.Draw(activeCam.View, activeCam.Projection);
            
            
            foreach (Street s in streets)
            {
                s.Draw(activeCam.View, activeCam.Projection);
            }
            foreach (Building b in buildings)
            {
                b.Draw(activeCam.View, activeCam.Projection);
            }
            base.Draw(gameTime);
        }
    }
}
