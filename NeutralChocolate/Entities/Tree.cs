using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    public class Tree : IEntity
    {
        private readonly int WIDTH = 64;
        private readonly int HEIGHT = 150;
        private Collider collider;
        public Rectangle Bounds => collider.bounds;
        public int Damage => 0;
        public int Health { get => 1; set => Health = 1; }
        public Vector2 Position { get; set; }
        public Vector2 HitPosition => new Vector2(Position.X + WIDTH, Position.Y + HEIGHT);

        private readonly Vector2 colliderOffest = new Vector2(-20 ,-140);
        public Vector2 Location => Position + colliderOffest;

        public Tree(Vector2 position)
        {
            Position = position;
            var bounds = new Rectangle((int)position.X, (int)position.Y, Art.Tree.Width - 40, Art.Tree.Height / 4);
            this.collider = new Collider(bounds);
        }
        public void Update(GameTime gameTime, Vector2 playerPos, int mapW, int mapH) {}
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Store.modes.currentMode == Mode.Collider)
            {
                collider.Draw(spriteBatch);
            }
            spriteBatch.Draw(Art.Tree, Location, Color.White);
        }

        public void OnHit(int damage, Vector2 magnitude)
        {
        
        }
    }
}
