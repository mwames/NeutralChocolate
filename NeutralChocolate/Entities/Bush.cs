using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    public class Bush : IEntity
    {
        private Vector2 position;
        private Collider collider;
        public Rectangle Bounds => collider.bounds;
        public int Damage => 0;
        public Vector2 Position => position;

        public int Health { get => 1; set => Health = 1; }

        public Bush(Vector2 position)
        {
            this.position = position;
            var bounds = new Rectangle((int)position.X, (int)position.Y, Art.Bush.Width, Art.Bush.Height);
            this.collider = new Collider(bounds);
        }

        public void Update(GameTime gameTime, Vector2 playerPos, int mapW, int mapH) {
            collider.Update(gameTime, position, mapW, mapH);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Store.modes.currentMode == Mode.Collider)
            {
                collider.Draw(spriteBatch);
            }
            spriteBatch.Draw(Art.Bush, position, Color.White);
        }

        public void OnHit(int damage, Vector2 magnitude)
        {
        
        }
    }
}
