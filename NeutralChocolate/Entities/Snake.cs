using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    class Snake : IEnemy
    {
        private Vector2 position;
        private int health;
        private int speed;
        private int radius;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Vector2 Position => position;
        public int Radius => radius;


        public Snake(Vector2 position)
        {
            this.position = position;
            speed = 160;
            radius = 42;
            health = 3;
        }

        public void Update(GameTime gameTime, Vector2 playerPos, int mapW, int mapH)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 moveDir = playerPos - position;
            moveDir.Normalize();

            position += moveDir * speed * dt;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Store.textures.Get(TextureName.Snake), position, Color.White);
        }

        public void OnHit()
        {
            health -= 1;
        }
    }
}
