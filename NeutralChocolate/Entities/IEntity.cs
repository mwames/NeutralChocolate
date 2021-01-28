using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    public interface IEntity
    {
        Rectangle Bounds { get; }
        Vector2 Position { get; }
        int Health { get; set; }
        int Damage { get; }
        void Update(GameTime gameTime, Vector2 playerPos, int mapW, int mapH);
        void Draw(SpriteBatch spriteBatch);
        void OnHit(int damage, Vector2 magnitude);
    }
}
