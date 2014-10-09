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
    abstract class GUI
    {
        static RenderTarget2D guiTarget;
        //static List<GUI_Element> elements;    not using a list anymore
        static MiniMap miniMap;
        static LivesDisplay lifeDisplay;

        public static void Initialize()
        {
            miniMap = new MiniMap(null);
            lifeDisplay = new LivesDisplay(new Vector2(10));
        }

        public static void LoadContent(ContentManager content)
        {
            lifeDisplay.Texture = content.Load<Texture2D>("green_heart");
            //miniMap.Texture = content.Load<Texture2D>("
        }

        public static void Update(Game game, PlayerTank player)
        {
            miniMap.Update(game, player);
            lifeDisplay.Update(game, player);
            
        }

        public static void Draw(SpriteBatch sb)
        {
            sb.Begin();
            lifeDisplay.Draw(sb);
            miniMap.Draw(sb);
            sb.End();
        }
    }

    class GUI_Element
    {
        protected Vector2 position;
        protected Texture2D texture;

        //create the gui element. set position to 0,0 if null location is passed.
        public GUI_Element(Vector2? _position/*, Texture2D _texture*/)
        {
            if (_position.HasValue)
                position = _position.Value;
            else
                position = Vector2.Zero;

            //texture = _texture;
        }

        public virtual void Update(Game game, PlayerTank player)
        {
            //override this with object specific update logic
        }

        public virtual void Draw(SpriteBatch sb)
        {
            if (texture != null)
            {
                sb.Draw(texture, position, Color.White);
            }
            //else the derived class should do its thing
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
    }

    class MiniMap : GUI_Element
    {
        public MiniMap(Vector2? position)
            : base(position)
        {
        }

        public override void Update(Game game, PlayerTank player)
        {
            //update the minimap
        }
    }//end minimap class

    class LivesDisplay : GUI_Element
    {
        byte livesToDraw;

        public LivesDisplay(Vector2? position)
            : base(position)
        {
        }

        public override void Update(Game game, PlayerTank player)
        {
            livesToDraw = player.Lives;
            //base.Update(game);
        }

        public override void Draw(SpriteBatch sb)
        {
            Vector2 drawPos = position;

            if (livesToDraw > 0)
            {
                for (int i = 0; i < livesToDraw; i++)
                {
                    //draw the life image
                    sb.Draw(texture, drawPos, Color.White);
                    //move over for next one
                    drawPos.X += texture.Width;
                }
            }
            //base.Draw(sb);
        }
    }//end lives class
}
