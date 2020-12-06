using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    public class Tree : IObstacle
    {
        private readonly int WIDTH = 64;
        private readonly int HEIGHT = 150;
        private Vector2 position;
        public int Radius => 20;
        public Vector2 Position => position;
        public Vector2 HitPosition => new Vector2(position.X + WIDTH, position.Y + HEIGHT);

        public Tree(Vector2 position) {
            this.position = position;
        }
        public void Update(GameTime gameTime, Vector2 playerPos) { }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Store.textures.Get(TextureName.Tree), position, Color.White);
        }
    }
}
