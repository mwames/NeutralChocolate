using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    class Eye : IEntity
    {
        private Vector2 position;
        private Collider collider;
        public Rectangle Bounds => collider.bounds;
        private readonly Vector2 colliderOffest = new Vector2(30,15);
        public int Damage => 1;
        private int speed = 200;
        private float healthTimer = 0f;
        public int Health { get; set; }
        private double stopTime = 0d;
        private bool move = true;
        public Vector2 Position => position;
        public Vector2 Location => Position + colliderOffest;

        public Eye(Vector2 position)
        {
            Health = 5;
            this.position = position;
            var bounds = new Rectangle((int)Location.X, (int)Location.Y, Art.Eye.Width-60, Art.Eye.Height-20);
            this.collider = new Collider(bounds);
        }

        public void Update(GameTime gameTime, Vector2 playerPos, int mapW, int mapH)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (move)
            {
                Vector2 moveDir = playerPos - position;
                moveDir.Normalize();
                position += moveDir * speed * dt;
            }

            if (stopTime <= 0)
            {
                stopTime = stopTime + dt;
            }
            else
            {
                move = true;
            }

            if (healthTimer > 0)
                healthTimer -= dt;

            collider.Update(gameTime, Location, mapW, mapH);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Store.modes.currentMode == Mode.Collider)
            {
                collider.Draw(spriteBatch);
            }
            spriteBatch.Draw(Art.Eye, position, Color.White);
        }

        public void OnHit(int damage, Vector2 magnitude)
        {
            if (healthTimer <= 0)
            {
                Health -= 1;
                move = false;
                stopTime = -0.5d;
                healthTimer = .5f;
                speed = speed + 50;
            }
        }
    }
}
