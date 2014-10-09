using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RobotTank
{
    class PlayerTank : BaseGameObject
    {
        #region Variables
        Vector2 direction, position;
        readonly Vector2 startDirection;    //the starting direction of the tank

        float tank_rot;     //rotation of the tank in radians
        const float TurnSpeed = 0.01f, MaxMoveSpeed = 1.0f;
        float currentSpeed;

        //we want the tank to turn around its centre
        Matrix originAdjustment;

        //no tank is complete without a turret on top :)
        PlayerTurret turret;

        //having these as readonly instead on const means we can apply a difficulty scale when creating our player
        //e.g. normal mode, 100 health, 3 lives;    extreme mode, 1 health, 1 life
        readonly float MaxHealth;
        float health;
        readonly byte MaxLives;
        byte lives;

        #endregion

        #region Methods/Contructors
        public PlayerTank(float _maxHealth, byte _maxLives)
        {
            //direction = new Vector2(1, 0);
            startDirection = new Vector2(1, 0);

            turret = new PlayerTurret(startDirection);

            //initialise health to full
            health = MaxHealth = _maxHealth;
            //initalise lives to max
            lives = MaxLives = _maxLives;
        }

        public override void LoadContent(ContentManager content, string texturePath)
        {
            base.LoadContent(content, texturePath);

            Vector3 origin = new Vector3(texture.Width / 2f, texture.Height / 2f, 0f);
            originAdjustment = Matrix.CreateTranslation(-origin);

            turret.LoadContent(content, "turret");
        }

        public void Update(KeyboardState kbs)
        {

		    //turn tank based on user input
            if (kbs.IsKeyDown(Keys.A))
            {
                tank_rot -= TurnSpeed;
            }
            else if (kbs.IsKeyDown(Keys.D))
            {
                tank_rot += TurnSpeed;
            }

            //apply the rotation float variable to a rotation matrix
            Matrix.CreateRotationZ(tank_rot, out rotateM);

            //find out which direction the tank is facing
            direction = Vector2.Transform(startDirection, rotateM);

            //move tank forwards/backwards
            #region Movement
		    if (kbs.IsKeyDown(Keys.W))
            {
                if (currentSpeed < MaxMoveSpeed)
                {
                    currentSpeed += 0.01f;
                }
                else
                {
                    currentSpeed = MaxMoveSpeed;
                }
                position += direction * currentSpeed;
            }
            else if (kbs.IsKeyDown(Keys.S))
            {
                if (currentSpeed < MaxMoveSpeed / 2f)
                {
                    currentSpeed += 0.01f;
                }
                else
                {
                    currentSpeed = MaxMoveSpeed / 2f;
                }
                position -= direction * MaxMoveSpeed;
            }
            else
            {
                //tank will have to accelerate to from zero again next time it moves
                currentSpeed = 0f;
            }
            #endregion

            //apply the position vector to a translation matrix
            translateM = Matrix.CreateTranslation(new Vector3(position, 0));
            turret.Update(translateM, kbs);

            //by combining these two matrices, we can still use base.Update() while keeping origin adjusted
            rotateM = originAdjustment * rotateM;
            base.Update();         
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            turret.Draw(spriteBatch);
        } 
        #endregion

        #region PROPERTIES
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public float Health
        {
            get { return health; }
            //readonly
        }
        public byte Lives
        {
            get { return lives; }
            //readonly
        }
        public bool IsDead
        {
            get
            {
                if (health >= 0)
                    return true;
                else
                    return false;
            }
            //no set
        }
        public bool NoLivesLeft
        {
            get
            {
                if (lives >= 0)
                    return true;
                else
                    return false;
            }
            //no set
        }
        #endregion
    }
}
