using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    class Snake : IEnemy
    {
        private Vector2 position;
        private Collider collider;
        public Rectangle Bounds => collider.bounds;
        private int health = 3;
        private int speed = 160;
        private float healthTimer = 0;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        private double stopTime = 0d;
        private bool move = true;
        public Vector2 Position => position;
        public int Damage => 0;


        public Snake(Vector2 position)
        {
            this.position = position;
            var bounds = new Rectangle((int)position.X, (int)position.Y, Art.Snake.Width-10, Art.Snake.Height-20);
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

            collider.Update(gameTime, position, mapW, mapH);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Store.modes.currentMode == Mode.Collider)
            {
                collider.Draw(spriteBatch);
            }
            spriteBatch.Draw(Art.Snake, position, Color.White);
        }

        public void OnHit()
        {
            if (healthTimer <= 0)
            {
                health -= 1;
                move = false;
                stopTime = -0.5d;
                healthTimer = .5f;
                speed = speed + 75;
            }
        }
    }
}
