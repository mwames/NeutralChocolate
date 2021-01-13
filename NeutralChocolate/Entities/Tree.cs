using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    public class Tree : IEntity
    {
        private readonly int WIDTH = 64;
        private readonly int HEIGHT = 150;
        private Vector2 position;
        private Collider collider;
        public Rectangle Bounds => collider.bounds;
        public int Damage => 0;
        public Vector2 Position => position;
        public Vector2 HitPosition => new Vector2(position.X + WIDTH, position.Y + HEIGHT);

        public Tree(Vector2 position)
        {
            this.position = position;
            var bounds = new Rectangle((int)position.X, (int)position.Y, Art.Tree.Width, Art.Tree.Height);
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
            spriteBatch.Draw(Art.Tree, position, Color.White);
        }
    }
}
