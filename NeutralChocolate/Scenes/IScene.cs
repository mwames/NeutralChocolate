using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    public interface IScene
    {
        void Update(InputState input, GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, GraphicsDevice graphicsDevice);
    }
}
