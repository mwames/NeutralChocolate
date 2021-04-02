using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    public interface IScene
    {
        void Update(GameTime gameTime);
        void Draw(NeutralChocolate game);
    }
}
