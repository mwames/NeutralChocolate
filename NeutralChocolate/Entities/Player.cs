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
        private int radius = 56;
        private float healthTimer = 0f;
        public bool isMoving = false;
        private KeyboardState kStateOld = Keyboard.GetState();
        public AnimatedSprite[] animations = new AnimatedSprite[4];
        private AnimatedSprite Animation => animations[(int)direction];
        private GamePadState gPrevious;
        private GamePadState gCurrent;
        private KeyboardState kPrevious;
        private KeyboardState kCurrent;


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
            set { health = value; }
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

        private bool WasPressed(Buttons button)
        {
            return gCurrent.IsButtonDown(button) && !gPrevious.IsButtonDown(button);
        }

        private bool IsPressed(Buttons button)
        {
            return gCurrent.IsButtonDown(button);
        }

        private bool WasPressed(Keys key)
        {
            return kCurrent.IsKeyDown(key) && !kPrevious.IsKeyDown(key);
        }

        private bool IsPressed(Keys key)
        {
            return kCurrent.IsKeyDown(key);
        }

        private void Move(Dir direction, float distance)
        {
            isMoving = true;

            var temp = position;
            if (direction == Dir.Up)
            {
                temp.Y -= distance;
            }
            if (direction == Dir.Down)
            {
                temp.Y += distance;
            }
            if (direction == Dir.Left)
            {
                temp.X -= distance;
            }
            if (direction == Dir.Right)
            {
                temp.X += distance;
            }
            if (!Obstacle.didCollide(temp, radius) && temp.Y > radius)
            {
                position = temp;
            }
        }

        private void Shoot(Dir direction)
        {
            Projectile.projectiles.Add(new Projectile(position, direction));
            MySounds.projectileSound.Play();
        }

        public void Update(GameTime gameTime, int mapW, int mapH)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (healthTimer > 0)
                healthTimer -= dt;

            gPrevious = gCurrent;
            gCurrent = GamePad.GetState(PlayerIndex.One);

            kPrevious = kCurrent;
            kCurrent = Keyboard.GetState();

            isMoving = false;
            var tempPos = position;
            var distanceToTravel = speed * dt;
            if (IsPressed(Buttons.LeftThumbstickUp) || IsPressed(Keys.Up))
                Move(Dir.Up, distanceToTravel);

            if (IsPressed(Buttons.LeftThumbstickDown) || IsPressed(Keys.Down))
                Move(Dir.Down, distanceToTravel);

            if (IsPressed(Buttons.LeftThumbstickLeft) || IsPressed(Keys.Left))
                Move(Dir.Left, distanceToTravel);

            if (IsPressed(Buttons.LeftThumbstickRight) || IsPressed(Keys.Right))
                Move(Dir.Right, distanceToTravel);

            if (isMoving)
            {
                Animation.Update(gameTime);
            }
            else
            {
                Animation.setFrame(1);
            }

            if (WasPressed(Keys.Space))
                Shoot(direction);

            if (WasPressed(Buttons.Y))
                Shoot(Dir.Up);

            if (WasPressed(Buttons.A))
                Shoot(Dir.Down);

            if (WasPressed(Buttons.X))
                Shoot(Dir.Left);

            if (WasPressed(Buttons.B))
                Shoot(Dir.Right);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch, new Vector2(Position.X - 48, Position.Y - 48));
        }

    }
}
