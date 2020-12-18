using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    class Eye : IEnemy
    {
        private Vector2 position;
        private int health;
        private int speed;
        private float healthTimer = 0f;
        private int radius;
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        double stopTime = 0d;
        private bool move = true;
        public Vector2 Position => position;
        public int Radius => radius;

        public Eye(Vector2 position)
        {
            this.position = position;
            speed = 200;
            radius = 45;
            health = 5;
        }

        public void Update(GameTime gameTime, Vector2 playerPos, int maapW, int maapH)
        {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(move)
            {
                Vector2 moveDir = playerPos - position;
                moveDir.Normalize();
                position += moveDir * speed * dt;
            }

            if(stopTime <=0)
            {
                stopTime = stopTime + dt;
            }
            else 
            {
                 move =true;
            }

            if (healthTimer > 0)
            
                healthTimer -= dt;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Art.Eye, position, Color.White);
        }

        public void OnHit()
        {
            if (healthTimer <=0)
            {
            health -= 1;
            move = false;
            stopTime = -0.5d;
            healthTimer = .5f;
            speed = speed + 50;
            }
        }
    }
}
