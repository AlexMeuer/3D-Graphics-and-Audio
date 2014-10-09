using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FPS_Assessment
{
    abstract class GameObject
    {
        Game theGame;
        protected ContentManager contentManger;
        protected GraphicsDevice graphicsDevice;
        protected BasicEffect effect;
       
        protected Vector3 position;
        

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public GameObject(Game game)
        {
            theGame = game;
            contentManger = (ContentManager)theGame.Services.GetService(typeof(ContentManager));
            GraphicsDeviceManager gdm = (GraphicsDeviceManager)theGame.Services.GetService(typeof(IGraphicsDeviceManager));
            graphicsDevice = gdm.GraphicsDevice;

        }
        public virtual void Init()
        {

        }

        public virtual void Update(GameTime gametime)
        {
        }

        public virtual void Draw(Matrix view, Matrix proj)
        {
        }
    }
}
