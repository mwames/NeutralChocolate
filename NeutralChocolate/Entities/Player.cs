using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NeutralChocolate
{
    class Player
    {
        private Vector2 position = new Vector2(100, 100);
        private int health = 5;
        private int speed = 300;
        private Dir direction = Dir.Down;
        private bool isMoving = false;
        private int radius = 56;
        private float healthTimer = 0f;
        private KeyboardState kStateOld = Keyboard.GetState();
        public AnimatedSprite anim;
        public AnimatedSprite[] animations = new AnimatedSprite[4];


        public float HealthTimer
        {
            get { return healthTimer; }
            set { healthTimer = value; }
        }
        public int Radius
        {
            get { return radius; }
            
        }
        public int Health
        {
            get { return health; }
            set {health = value;}
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public void setX(float newX)
        {
            position.X = newX;
        }

        public void setY(float newY)
        {
            position.Y = newY;
        }

        public void Update(GameTime gameTime, int mapW,int mapH)
        {
            KeyboardState kState = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (healthTimer > 0)
                healthTimer -= dt;

            /*   using a switch statement instead of using enum example. 
             switch (direction)
            {
                case Dir.Down:
                    anim = animations[0];
                    break;
                case Dir.Up:
                    anim = animations[1];
                    break;
                case Dir.Left:
                    anim = animations[2];
                    break;
                case Dir.Right:
                    anim = animations[3];
                    break;
                default:
                    break;
            }
            */

            // enum example. 
            anim = animations[(int)direction];


            if (isMoving)
                anim.Update(gameTime);
            else anim.setFrame(1) ;

            isMoving = false;

            if (kState.IsKeyDown(Keys.Right))
            {
                direction = Dir.Right;
                //position.X += speed * dt;
                isMoving = true;
            }
            if (kState.IsKeyDown(Keys.Left))

            {
                direction = Dir.Left;
               // position.X -= speed * dt;
                isMoving = true;
            }

            if (kState.IsKeyDown(Keys.Up))
            {
               direction = Dir.Up;
               // position.Y -= speed * dt;
                isMoving = true;

            }

            if (kState.IsKeyDown(Keys.Down))
            {
                direction = Dir.Down;
               // position.Y += speed * dt;
                isMoving = true;
            }
            if (isMoving)

            {
                Vector2 tempPos = position;
                switch (direction)
                {
                    case Dir.Right:

                        tempPos.X += speed * dt;
                        if (!Obstacle.didCollide(tempPos, radius)&& tempPos.X <mapW - radius)
                        { 
                            position.X += speed * dt;
                            

                        }
                        break;

                    case Dir.Left:
                        tempPos.X -= speed * dt;
                        if (!Obstacle.didCollide(tempPos, radius) && tempPos.X > radius)

                        {
                            position.X -= speed * dt;
                            

                        }

                        break;

                    case Dir.Down:
                         tempPos.Y += speed * dt;
                        if (!Obstacle.didCollide(tempPos, radius) && tempPos.Y <mapH - radius)
                        {
                            position.Y += speed * dt;
                            

                        }
                        break;

                    case Dir.Up:
                        tempPos.Y -= speed * dt;
                        if (!Obstacle.didCollide(tempPos, radius) && tempPos.Y > radius )
                        {
                            position.Y -= speed * dt;
                          


                        }
                        break;
                    default:
                        break;
                }
            }


            if (kState.IsKeyDown(Keys.Space)&& kStateOld.IsKeyUp(Keys.Space))
            {
                Projectile.projectiles.Add(new Projectile(position,direction));
                MySounds.projectileSound.Play();
            }
            kStateOld = kState;
        }

    }
}
