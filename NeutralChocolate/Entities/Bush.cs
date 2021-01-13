using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    public class Bush : IEntity
    {
        private readonly int SIDE_LENGTH = 57;
        private Vector2 position;
        private Collider collider;
        public Rectangle Bounds => collider.bounds;
        public int Damage => 0;
        public Vector2 Position => position;
        public Vector2 HitPosition => new Vector2(position.X + SIDE_LENGTH, position.Y + SIDE_LENGTH);

        public Bush(Vector2 position)
        {
            this.position = position;
            var bounds = new Rectangle((int)position.X, (int)position.Y, SIDE_LENGTH, SIDE_LENGTH);
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
    }
}
