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
            Store.soundEffects.Get(SoundEffectName.Blip).Play();
        }

        public void Update(GameTime gameTime, Vector2 playerPos, int mapW, int mapH)
        {
            if (Input.WasPressed(Keys.Enter))
            {
                Store.scenes.ChangeScene(SceneName.Pause);
            }
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (health <= 0)
            {
                Store.songs.Stop();
            }

            if (healthTimer > 0)
            
                healthTimer -= dt;

            if (health >0)
            {   

            gPrevious = gCurrent;
            gCurrent = GamePad.GetState(PlayerIndex.One);

            kPrevious = kCurrent;
            kCurrent = Keyboard.GetState();

            isMoving = false;
            var tempPos = position;
            var distanceToTravel = speed * dt;

            if (Input.IsPressed(Buttons.LeftThumbstickUp) || Input.IsPressed(Keys.Up))
                Move(Dir.Up, distanceToTravel);

            if (Input.IsPressed(Buttons.LeftThumbstickDown) || Input.IsPressed(Keys.Down))
                Move(Dir.Down, distanceToTravel);

            if (Input.IsPressed(Buttons.LeftThumbstickLeft) || Input.IsPressed(Keys.Left))
                Move(Dir.Left, distanceToTravel);

            if (Input.IsPressed(Buttons.LeftThumbstickRight) || Input.IsPressed(Keys.Right))
                Move(Dir.Right, distanceToTravel);

            if (isMoving)
            {
                Animation.Update(gameTime);
            }
            else
            {
                Animation.setFrame(1);
            }

            if (Input.WasPressed(Keys.Space))
                Shoot(direction);

            if (Input.WasPressed(Buttons.Y))
                Shoot(Dir.Up);

            if (Input.WasPressed(Buttons.A))
                Shoot(Dir.Down);

            if (Input.WasPressed(Buttons.X))
                Shoot(Dir.Left);

            if (Input.WasPressed(Buttons.B))
                Shoot(Dir.Right);
            }
            if(position.X < 0)
            {
                position.X = 0;
            }

            if(position.Y < 0)
            {
                position.Y = 0;
            }

            if(position.X + Store.textures.Get(TextureName.PlayerRight).Width >mapW)
            {
                position.X = mapW - Store.textures.Get(TextureName.PlayerRight).Width;// still requires tweaking
            }
             if(position.Y + Store.textures.Get(TextureName.PlayerRight).Height >mapH)
            {
                position.Y = mapH - Store.textures.Get(TextureName.PlayerRight).Height;
            }
        }

        public void OnCollide() {
            if (healthTimer <= 0) {
                health--;
                healthTimer = 1.5f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch, new Vector2(Position.X, Position.Y));
        }
    }
}
