using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Basically use one of these buggers for collision detection
namespace NeutralChocolate
{
    public class Collider : IEntity
    {
        private Rectangle bounds;
        public Collider(Rectangle bounds)
        {
            this.bounds = bounds;
        }

        public Vector2 Position => new Vector2(bounds.Left, bounds.Top);
        public int Radius => bounds.Width;
        public Texture2D Texture => Store.textures.Get(TextureName.Pixel);

        // Draws the outline of the collider
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                bounds,
                new Rectangle(0, 0, 1, 1),
                Color.Transparent
            );

            // TOP LINE
            spriteBatch.Draw(
                Texture,
                new Rectangle(
                    bounds.X,
                    bounds.Y,
                    bounds.Width,
                    1
                ),
                new Rectangle(0, 0, 1, 1),
                Color.DarkBlue
            );

            // BOTTOM LINE
            spriteBatch.Draw(
                Texture,
                new Rectangle(
                    bounds.X,
                    bounds.Y + bounds.Height,
                    bounds.Width,
                    1
                ),
                new Rectangle(0, 0, 1, 1),
                Color.DarkBlue
            );

            // LEFT LINE
            spriteBatch.Draw(
                Texture,
                new Rectangle(
                    bounds.X,
                    bounds.Y,
                    1,
                    bounds.Height
                ),
                new Rectangle(0, 0, 1, 1),
                Color.DarkBlue
            );

            // RIGHT LINE
            spriteBatch.Draw(
                Texture,
                new Rectangle(
                    bounds.X + bounds.Width,
                    bounds.Y,
                    1,
                    bounds.Height
                ),
                new Rectangle(0, 0, 1, 1),
                Color.DarkBlue
            );
        }

        // This needs to change to just take in the parent position.
        public void Update(GameTime gameTime, Vector2 playerPos, int mapW, int mapH)
        {
            bounds.X = (int)playerPos.X;
            bounds.Y = (int)playerPos.Y;
        }
    }
}
