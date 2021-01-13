using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace NeutralChocolate
{
    enum Dir
    {
        Down,
        Up,
        Left,
        Right
    }

    class Player : IEntity
    {
        private readonly int RADIUS = 56;
        private readonly int WIDTH = 58;
        private readonly int HEIGHT = 62;
        public Vector2 position = new Vector2(100, 100);
        private int health = 5;
        private int speed = 300;
        private Dir direction = Dir.Down;
        private float healthTimer = 0f;
        private float shootRate = 0f;
        private bool isMoving = false;
        private KeyboardState kStateOld = Keyboard.GetState();
        private AnimatedSprite[] animations;
        private AnimatedSprite Animation => animations[(int)direction];
        private GamePadState gPrevious;
        private GamePadState gCurrent;
        private KeyboardState kPrevious;
        private KeyboardState kCurrent;
        private List<IEnemy> bullets;
        private Collider collider;

        public int Radius => RADIUS;
        public Vector2 Position => position;
        public int Health => health;

        public Player(List<IEnemy> bullets)
        {
            this.bullets = bullets;
            this.collider = new Collider(
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    WIDTH,
                    HEIGHT
                )
            );
        }

        public void Initialize()
        {
            animations = new[] {
                new AnimatedSprite(Art.PlayerDown, 1, 4),
                new AnimatedSprite(Art.PlayerUp, 1, 4),
                new AnimatedSprite(Art.PlayerLeft, 1, 4),
                new AnimatedSprite(Art.PlayerRight, 1, 4)
            };
        }

        private AnimatedSprite GetAnimationFrame(Texture2D texture)
        {
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
            if (shootRate <= 0)
            {
                bullets.Add(new Projectile(position, direction));
                Sound.Blip.Play();
                shootRate = .2f;
            }
        }

        public void Update(GameTime gameTime, Vector2 playerPos, int mapW, int mapH)
        {
            if (Input.WasPressed(Keys.Enter))
            {
                Store.scenes.ChangeScene(SceneName.Pause);
            }
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (shootRate > 0)

                shootRate -= dt;

            if (healthTimer > 0)

                healthTimer -= dt;

            if (health > 0)
            {

                gPrevious = gCurrent;
                gCurrent = GamePad.GetState(PlayerIndex.One);

                kPrevious = kCurrent;
                kCurrent = Keyboard.GetState();

                isMoving = false;
                var tempPos = position;
                var distanceToTravel = speed * dt;
                if (Input.IsPressed(Buttons.RightTrigger))
                {
                    speed = 500;
                }
                else speed = 300;



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

                else if (Input.WasPressed(Buttons.Y))
                    Shoot(Dir.Up);

                else if (Input.WasPressed(Buttons.A))
                    Shoot(Dir.Down);

                else if (Input.WasPressed(Buttons.X))
                {
                    Shoot(Dir.Left);
                    //Move(Dir.Left, distanceToTravel);

                }

                else if (Input.WasPressed(Buttons.B))
                    Shoot(Dir.Right);
            }

            if (position.X < 0)
            {
                position.X = 0;
            }

            if (position.Y < 0)
            {
                position.Y = 0;
            }

            if (position.X + HEIGHT > mapW)
            {
                position.X = mapW - WIDTH;
            }
            if (position.Y + HEIGHT > mapH)
            {
                position.Y = mapH - HEIGHT;
            }

            collider.Update(gameTime, position, mapW, mapH);
        }

        public void OnCollide()
        {
            if (healthTimer <= 0)
            {
                health--;

                healthTimer = 1.5f;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Store.modes.currentMode == Mode.Collider)
            {
                collider.Draw(spriteBatch);
            }
            Animation.Draw(spriteBatch, new Vector2(Position.X - 20, Position.Y - 17));
        }
    }
}
