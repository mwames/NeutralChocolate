using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NeutralChocolate
{
    class Player : IEntity
    {
        private readonly int RADIUS = 56;
        private Vector2 position = new Vector2(100, 100);
        private int health = 5;
        private int speed = 300;
        private Dir direction = Dir.Down;
        private float healthTimer = 0f;
        private bool isMoving = false;
        private KeyboardState kStateOld = Keyboard.GetState();
        private AnimatedSprite[] animations;
        private AnimatedSprite Animation => animations[(int)direction];
        private GamePadState gPrevious;
        private GamePadState gCurrent;
        private KeyboardState kPrevious;
        private KeyboardState kCurrent;
        private List<IEnemy> bullets;

        public int Radius => RADIUS;
        public Vector2 Position => position;
        public int Health => health;

        public Player(List<IEnemy> bullets) {
            this.bullets = bullets;
        }

        public void Initialize() {
            animations = new[] {
                new AnimatedSprite(Store.textures.Get(TextureName.PlayerDown), 1, 4),
                new AnimatedSprite(Store.textures.Get(TextureName.PlayerUp), 1, 4),
                new AnimatedSprite(Store.textures.Get(TextureName.PlayerLeft), 1, 4),
                new AnimatedSprite(Store.textures.Get(TextureName.PlayerRight), 1, 4)
            };
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

        private AnimatedSprite GetAnimationFrame(Texture2D texture) {
            return new AnimatedSprite(texture, 1, 4);
        }

        private void Move(Dir direction, float distance)
        {
            isMoving = true;

            this.direction = direction;
            if (direction == Dir.Up)
            {
                position.Y -= distance;
            }
            if (direction == Dir.Down)
            {
                position.Y += distance;
            }
            if (direction == Dir.Left)
            {
                position.X -= distance;
            }
            if (direction == Dir.Right)
            {
                position.X += distance;
            }
        }

        private void Shoot(Dir direction)
        {
            bullets.Add(new Projectile(position, direction));
        }

        public void Update(GameTime gameTime, Vector2 playerPos)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (health <= 0)
            {
                Store.songs.Stop();
            }

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

        public void OnCollide() {
            if (healthTimer <= 0) {
                health--;
                healthTimer = 1.5f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch, new Vector2(Position.X - 48, Position.Y - 48));
        }
    }
}
