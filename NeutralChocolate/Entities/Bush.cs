using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    public class Bush : IObstacle
    {
        private readonly int SIDE_LENGTH = 57;
        private Vector2 position;
        public Vector2 Position => position;
        public Vector2 HitPosition => new Vector2(position.X + SIDE_LENGTH, position.Y + SIDE_LENGTH);
        public int Radius => 32;

        public Bush(Vector2 position) {
            this.position = position;
        }
        public void Update(GameTime gameTime, Vector2 playerPos, int maapW, int maapH) { }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Art.Bush, position, Color.White);
        }
    }
}
